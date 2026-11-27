// ****************************************************************************
// Project:  HSMS.Logging
// File:     FileLoggerProvider.cs
// Author:   Latency McLaughlin
// Date:     06/30/2021
// ****************************************************************************

using System.Globalization;
using Microsoft.Extensions.Logging;
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
using Microsoft.Extensions.Options;
#endif

namespace Logging.RollingFile;

/// <summary>
///     An <see cref="ILoggerProvider" /> that writes each record to a daily rolling file. Every record is flushed to the
///     operating system before the log call returns.
/// </summary>
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
[ProviderAlias("FileLoggerProvider")]
public sealed class FileLoggerProvider : ILoggerProvider, ISupportExternalScope
#else
public sealed class FileLoggerProvider : IDisposable
#endif
{
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
    private readonly bool _includeScopes;
    private IExternalScopeProvider? _scopeProvider;
#endif

    /// <summary>
    ///     Creates a new instance of <see cref="FileLoggerProvider" />.
    /// </summary>
    /// <param name="options">The options to use when creating a provider.</param>
    #if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
    public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> options)
    {
        var currentValue = options.CurrentValue;
        _includeScopes = currentValue.IncludeScopes;
        Writer = new DailyFileWriter(currentValue);
    }
#else
    public FileLoggerProvider(FileLoggerOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));
        if (options.IncludeScopes)
            throw new NotSupportedException("Scopes require Microsoft.Extensions.Logging support.");
        Writer = new DailyFileWriter(options);
    }
#endif

    internal RollingFileWriter Writer { get; }

#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
    internal IExternalScopeProvider? ScopeProvider => _includeScopes ? _scopeProvider : null;
#endif

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new FileLogger(this);

    #if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
    /// <inheritdoc />
    void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider? scopeProvider) => _scopeProvider = scopeProvider;
#endif

    /// <inheritdoc />
    public void Dispose() => Writer.Dispose();

    private sealed class DailyFileWriter : RollingFileWriter
    {
        private readonly bool _aaecLayout;
        private readonly bool _throwOnFailure;
        private readonly bool _closeAfterWrite;
        private readonly string? _mutexName;
        private readonly int? _filesPerDayLimit;
        private readonly int? _retainedDays;

        public DailyFileWriter(FileLoggerOptions options) : base(options.LogDirectory, string.Empty, options.FileName)
        {
            if (options.FilesPerDayLimit <= 0)
                throw new ArgumentOutOfRangeException(nameof(options.FilesPerDayLimit));
            if (options.CoordinateProcesses && !options.CloseFileAfterWrite)
                throw new ArgumentException("Cross-process coordination requires CloseFileAfterWrite.", nameof(options));
            _mutexName = options.CoordinateProcesses ? CreateMutexName(BaseDirectory, BaseFileName) : null;
            _aaecLayout = options.UseAaecFileLayout;
            _throwOnFailure = options.ThrowOnWriteFailure;
            _closeAfterWrite = options.CloseFileAfterWrite;
            _filesPerDayLimit = options.FilesPerDayLimit;
            _retainedDays = options.RetainedFileCountLimit;
            FileExtension = ".txt";
            UseDailyFile  = true;
            MaxFileSize   = options.FileSizeLimit ?? 0;
            MaxDaysOld    = options.RetentionDays ?? 0;
            MaxNumFiles   = options.RetainedFileCountLimit ?? 0;
        }

        protected override string GetStem(DateTime timestamp) =>
            Path.Combine(BaseDirectory, $"{BaseFileName}{timestamp.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}");

        protected override IDisposable? AcquireWriteLock()
        {
            if (_mutexName is null)
                return null;

            var mutex = new System.Threading.Mutex(false, _mutexName);
            try
            {
                try
                {
                    if (!mutex.WaitOne(TimeSpan.FromSeconds(30)))
                        throw new TimeoutException("Timed out waiting for the shared log file lock.");
                }
                catch (System.Threading.AbandonedMutexException)
                {
                    // The abandoned mutex is owned by this thread after WaitOne throws.
                }
                return new HeldMutex(mutex);
            }
            catch
            {
                mutex.Dispose();
                throw;
            }
        }

        private static string CreateMutexName(string directory, string prefix)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                (directory.StartsWith(@"\\", StringComparison.Ordinal) || new DriveInfo(Path.GetPathRoot(directory)!).DriveType == DriveType.Network))
                throw new NotSupportedException("Cross-process file coordination requires a local directory.");
            var identity = Path.Combine(directory, prefix).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
            using var sha = System.Security.Cryptography.SHA256.Create();
            var digest = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(identity));
            return @"Global\HSMS.Logging." + BitConverter.ToString(digest).Replace("-", string.Empty);
        }

        private sealed class HeldMutex : IDisposable
        {
            private readonly System.Threading.Mutex _mutex;
            public HeldMutex(System.Threading.Mutex mutex) => _mutex = mutex;
            public void Dispose()
            {
                try { _mutex.ReleaseMutex(); }
                finally { _mutex.Dispose(); }
            }
        }

        protected override bool CloseAfterWrite => _closeAfterWrite;

        protected override System.Text.Encoding TextEncoding => _aaecLayout ? new System.Text.UTF8Encoding(false) : base.TextEncoding;

        protected override bool ShouldRollAfterWrite(long length) =>
            _aaecLayout ? MaxFileSize > 0 && length > MaxFileSize : base.ShouldRollAfterWrite(length);

        protected override string ResolvePath(string stem)
        {
            if (!_aaecLayout)
                return base.ResolvePath(stem);

            var latest = Directory.GetFiles(BaseDirectory, Path.GetFileName(stem) + ".*" + FileExtension)
                .OrderByDescending(path => path).FirstOrDefault();
            var index = 0;
            if (latest != null)
            {
                var suffix = latest.Substring(stem.Length + 1);
                var dot = suffix.IndexOf('.');
                if (dot >= 0)
                    int.TryParse(suffix.Substring(0, dot), out index);
            }
            for (; !_filesPerDayLimit.HasValue || index < _filesPerDayLimit.Value; index++)
            {
                var path = $"{stem}.{index}{FileExtension}";
                if (MaxFileSize <= 0 || !File.Exists(path) || new FileInfo(path).Length <= MaxFileSize)
                    return path;
            }
            throw new IOException($"Daily log file limit reached for {stem}.");
        }

        protected override void RunMaintenance()
        {
            if (!_aaecLayout)
            {
                base.RunMaintenance();
                return;
            }
            if (!_retainedDays.HasValue)
                return;

            var prefix = BaseFileName;
            var groups = Directory.GetFiles(BaseDirectory, prefix + "*" + FileExtension)
                .Select(path => new { Path = path, Name = Path.GetFileName(path) })
                .Where(file => file.Name.Length > prefix.Length + 8 &&
                    file.Name[prefix.Length + 8] == '.' &&
                    DateTime.TryParseExact(file.Name.Substring(prefix.Length, 8), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .GroupBy(file => file.Name.Substring(prefix.Length, 8))
                .OrderByDescending(group => group.Key)
                .Skip(_retainedDays.Value);
            foreach (var group in groups)
                foreach (var file in group)
                    if (!string.Equals(file.Path, FileName, StringComparison.OrdinalIgnoreCase))
                        File.Delete(file.Path);
        }

        protected override void OnWriteFailed(Exception exception)
        {
            if (_throwOnFailure)
                throw new IOException("Failed to write a log record.", exception);
            base.OnWriteFailed(exception);
        }
    }
}

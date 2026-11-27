// ****************************************************************************
// Project:  Logging
// File:     RollingFileWriter.cs
// Author:   Latency McLaughlin
// Date:     11/20/2026
// ****************************************************************************

using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace Logging.RollingFile;

/// <summary>
///     Writes records to rolling text files. Each record is written and flushed to the operating system before the write
///     call returns, so nothing waits in a queue and nothing is lost if the process exits or a remote client disconnects.
/// </summary>
/// <remarks>
///     Files roll when the day changes (<see cref="UseDailyFile" />) and when they reach <see cref="MaxFileSize" />.
///     Retention (<see cref="MaxNumFiles" />, <see cref="MaxDaysOld" />) and compression (<see cref="CompressClosedFiles" />)
///     run each time a file opens. The file being written is never removed or compressed.
/// </remarks>
public abstract class RollingFileWriter : IDisposable
{
    private const string TimestampFormat = "yyyyMMdd@HHmmss";

    private readonly object _gate = new();
    private          DateTime _fileTimestamp = DateTime.MinValue;
    private volatile bool     _disposed;

    /// <summary>The writer for the open file, or <see langword="null" /> when no file is open.</summary>
    protected StreamWriter? Stream;

    protected RollingFileWriter(string baseDirectory, string dateSubDirectory, string baseFileName)
    {
        BaseDirectory    = baseDirectory;
        DateSubDirectory = dateSubDirectory;
        BaseFileName     = Path.GetFileName(baseFileName);
    }

    /// <summary><see langword="true" /> once <see cref="Dispose" /> has been called.</summary>
    protected bool Disposed => _disposed;

    private string _baseDirectory = string.Empty;

    /// <summary>Setting the directory closes the open file, so the next write opens a file in the new directory.</summary>
    public string BaseDirectory
    {
        get => _baseDirectory;
        set
        {
            lock (_gate)
            {
                CloseStream();
                _baseDirectory = Path.GetFullPath(value);
                Directory.CreateDirectory(_baseDirectory);
            }
        }
    }

    public string BaseFileName { get; }

    public string DateSubDirectory { get; }

    public string FileExtension { get; protected set; } = string.Empty;

    /// <summary>The path of the open file, or <see langword="null" /> when no file is open.</summary>
    public string? FileName { get; private set; }

    private string? _fileStamp;

    /// <summary>
    ///     The text that replaces the open-time stamp in the file name, so that logs from related writers can share one name.
    ///     <see langword="null" /> or empty uses the time the file opens. Setting it closes the open file.
    /// </summary>
    public string? FileStamp
    {
        get => _fileStamp;
        set
        {
            lock (_gate)
            {
                CloseStream();
                _fileStamp = string.IsNullOrEmpty(value) ? null : value;
            }
        }
    }

    /// <summary><see langword="true" /> truncates an existing file of the same name; <see langword="false" /> appends to it.</summary>
    public bool OverwriteOnOpen { get; set; }

    /// <summary><see langword="true" /> starts a new file when the timestamp's date changes.</summary>
    public bool UseDailyFile { get; set; }

    /// <summary>The size in bytes at which a file is closed and the next numbered file is opened. <c>0</c> for no limit.</summary>
    public long MaxFileSize { get; set; }

    /// <summary>The maximum number of files to keep. <c>0</c> for no limit.</summary>
    public int MaxNumFiles { get; set; }

    /// <summary>The age in days after which a closed file is removed. <c>0</c> for no limit.</summary>
    public int MaxDaysOld { get; set; }

    /// <summary><see langword="true" /> gzip-compresses closed files when a new file opens.</summary>
    public bool CompressClosedFiles { get; set; }

    /// <summary>
    ///     Writes <paramref name="text" /> to the file for <paramref name="timestamp" /> and flushes it before returning.
    /// </summary>
    /// <returns><see langword="false" /> once the writer is disposed.</returns>
    public bool Write(DateTime timestamp, string text) => WriteLog(timestamp, () => Stream!.Write(text));

    /// <summary>
    ///     Runs <paramref name="write" /> against the file for <paramref name="timestamp" /> and flushes it before returning.
    /// </summary>
    /// <returns><see langword="false" /> once the writer is disposed.</returns>
    protected bool WriteLog(DateTime timestamp, Action write)
    {
        lock (_gate)
        {
            if (_disposed)
                return false;

            try
            {
                using (AcquireWriteLock())
                {
                    try
                    {
                        if (Stream is not null && UseDailyFile && timestamp.Date > _fileTimestamp.Date)
                            CloseStream();

                        if (Stream is null)
                            OpenStream(timestamp);

                        write();
                        Stream!.Flush();

                        if (CloseAfterWrite || ShouldRollAfterWrite(Stream.BaseStream.Length))
                            CloseStream();
                    }
                    catch (Exception exception)
                    {
                        if (CloseAfterWrite)
                        {
                            try { CloseStream(); }
                            catch (Exception closeException) { throw new AggregateException(exception, closeException); }
                        }
                        throw;
                    }
                }
            }
            catch (Exception exception)
            {
                OnWriteFailed(exception);
            }

            return true;
        }
    }

    /// <summary>Flushes the open file to the operating system.</summary>
    public void Flush()
    {
        lock (_gate)
        {
            if (!_disposed)
                FlushCore();
        }
    }

    /// <summary>Closes the open file. The next write opens a new one.</summary>
    public void CloseCurrentFile()
    {
        lock (_gate)
        {
            if (!_disposed)
                CloseStream();
        }
    }

    public void Dispose()
    {
        lock (_gate)
        {
            if (_disposed)
                return;

            _disposed = true;
            CloseStream();
        }

        GC.SuppressFinalize(this);
    }

    public string[] GetLogFileNames()
    {
        try
        {
            return Directory.GetFiles(BaseDirectory, $"{BaseFileName}*{FileExtension}*", SearchOption.AllDirectories);
        }
        catch (IOException)
        {
            return [];
        }
    }

    public int GetNumLogFiles() => GetLogFileNames().Length;

    public void RemoveExcessFiles()
    {
        if (MaxNumFiles <= 0)
            return;

        foreach (var path in GetLogFileNames().OrderByDescending(p => File.GetLastWriteTimeUtc(p)).Skip(MaxNumFiles).Where(IsClosedFile))
            PurgeLogFile(path);
    }

    public void RemoveOlderFiles()
    {
        if (MaxDaysOld <= 0)
            return;

        var cutoff = DateTime.UtcNow.AddDays(-MaxDaysOld);
        foreach (var path in GetLogFileNames().Where(IsClosedFile).Where(p => File.GetLastWriteTimeUtc(p) < cutoff))
            PurgeLogFile(path);
    }

    public void CompressFiles()
    {
        foreach (var path in GetLogFileNames().Where(IsClosedFile))
        {
            if (path.EndsWith(".gz", StringComparison.OrdinalIgnoreCase) || !path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase))
                continue;

            var compressed = $"{path}.gz";
            try
            {
                using (var input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var output = new GZipStream(new FileStream(compressed, FileMode.Create, FileAccess.Write), CompressionLevel.Optimal))
                    input.CopyTo(output);

                File.Delete(path);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                TryDelete(compressed);
            }
        }
    }

    /// <summary>Acquires optional coordination for the complete file transaction.</summary>
    protected virtual IDisposable? AcquireWriteLock() => null;

    /// <summary>Whether the current file should close after a write.</summary>
    protected virtual bool ShouldRollAfterWrite(long length) => MaxFileSize > 0L && length >= MaxFileSize;

    /// <summary>Whether to close the file after each record.</summary>
    protected virtual bool CloseAfterWrite => false;

    /// <summary>Encoding used for newly opened files.</summary>
    protected virtual Encoding TextEncoding => Encoding.UTF8;

    /// <summary>Called when a file operation fails. The failed record is dropped and the caller is not interrupted.</summary>
    protected virtual void OnWriteFailed(Exception exception) => Debug.WriteLine(exception);

    /// <summary>Returns the path of the file for <paramref name="timestamp" />, without its roll suffix or extension.</summary>
    protected virtual string GetStem(DateTime timestamp)
    {
        var directory = string.IsNullOrEmpty(DateSubDirectory)
                            ? BaseDirectory
                            : Path.Combine(BaseDirectory, timestamp.ToString(DateSubDirectory, CultureInfo.InvariantCulture));
        var stamp = _fileStamp ?? timestamp.ToString(TimestampFormat, CultureInfo.InvariantCulture);
        return Path.Combine(directory, $"{BaseFileName}.{stamp}");
    }

    protected virtual void FlushCore() => Stream?.Flush();

    protected virtual void NewFileOpened()
    { }

    protected virtual void ClosingFile()
    { }

    private bool IsClosedFile(string path) => !string.Equals(path, FileName, StringComparison.OrdinalIgnoreCase);

    private void OpenStream(DateTime timestamp)
    {
        var path = ResolvePath(GetStem(timestamp));
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        // Unbuffered and write-through: each record reaches the disk, not just the file-system cache.
        var file = new FileStream(path, OverwriteOnOpen ? FileMode.Create : FileMode.Append, FileAccess.Write, FileShare.Read, bufferSize: 1, FileOptions.WriteThrough);
        try
        {
            Stream         = new(file, TextEncoding) { AutoFlush = true };
            FileName       = path;
            _fileTimestamp = timestamp;
            NewFileOpened();
            RunMaintenance();
        }
        catch (Exception exception)
        {
            try
            {
                if (Stream is null)
                    file.Dispose();
                else
                    CloseStream();
            }
            catch (Exception closeException)
            {
                throw new AggregateException(exception, closeException);
            }
            throw;
        }
    }

    /// <summary>Returns the first path for <paramref name="stem" /> that is new or still below <see cref="MaxFileSize" />.</summary>
    protected virtual string ResolvePath(string stem)
    {
        for (var index = 0;; index++)
        {
            var path = $"{stem}{(index == 0 ? string.Empty : $"_{index}")}{FileExtension}";
            if (MaxFileSize <= 0L || !File.Exists(path) || new FileInfo(path).Length < MaxFileSize)
                return path;
        }
    }

    private void CloseStream()
    {
        if (Stream is null)
            return;

        Exception? failure = null;
        try
        {
            ClosingFile();
        }
        catch (Exception exception)
        {
            failure = exception;
        }

        try
        {
            Stream.Dispose();
        }
        catch (Exception exception)
        {
            failure = failure is null ? exception : new AggregateException(failure, exception);
        }
        finally
        {
            Stream   = null;
            FileName = null;
        }

        if (failure is not null)
            OnWriteFailed(failure);
    }

    protected virtual void RunMaintenance()
    {
        try
        {
            RemoveExcessFiles();
            RemoveOlderFiles();
            if (CompressClosedFiles)
                CompressFiles();
        }
        catch (Exception exception)
        {
            OnWriteFailed(exception);
        }
    }

    private void PurgeLogFile(string path)
    {
        try
        {
            File.Delete(path);
            if (string.IsNullOrEmpty(DateSubDirectory))
                return;

            // Remove date sub-directories left empty, stopping short of the base directory.
            for (var directory = Path.GetDirectoryName(path);
                 directory is not null && directory.Length > BaseDirectory.Length && !Directory.EnumerateFileSystemEntries(directory).Any();
                 directory = Path.GetDirectoryName(directory))
                Directory.Delete(directory);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // A file still held by another reader is kept and retried when the next file opens.
        }
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Nothing to clean up if the partial file was never created.
        }
    }
}

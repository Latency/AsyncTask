// ****************************************************************************
// Project:  Logging
// File:     DailyTextWriter.cs
// Author:   Latency McLaughlin
// Date:     11/21/2026
// ****************************************************************************

using System.Text;

namespace Logging.RollingFile;

/// <summary>
///     A <see cref="TextWriter" /> that appends to <c>{directory}\{yyyyMMdd}\{name}.{stamp}{extension}</c> and moves to the
///     next day's folder when the date changes, even if the process runs across midnight. Each write is flushed before it
///     returns, and writes from different threads are serialized.
/// </summary>
/// <remarks>
///     The file opens on the first write. <paramref name="fileStamp" /> is the stamp in the file name; when it is
///     <see langword="null" /> or empty, the stamp is the time the file opens (<c>yyyyMMdd@HHmmss</c>).
/// </remarks>
public sealed class DailyTextWriter : TextWriter
{
    private readonly Writer _writer;
#if NET8_0_OR_GREATER
    private readonly TimeProvider _time;

    public DailyTextWriter(string directory, string fileName, string extension, string? fileStamp = null, TimeProvider? time = null)
    {
        _time = time ?? TimeProvider.System;
#else
    public DailyTextWriter(string directory, string fileName, string extension, string? fileStamp = null)
    {
#endif
        _writer = new(directory, fileName, extension)
        {
            FileStamp    = fileStamp,
            UseDailyFile = true
        };
    }

    public override Encoding Encoding => Encoding.UTF8;

    /// <summary>The path of the open file, or <see langword="null" /> before the first write.</summary>
    public string? FileName => _writer.FileName;

    public override void Write(char value) => Write(value.ToString());

    public override void Write(string? value)
    {
        if (!string.IsNullOrEmpty(value))
#if NET8_0_OR_GREATER
            _writer.Write(_time.GetLocalNow().DateTime, value);
#else
            _writer.Write(DateTime.Now, value!);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public override void Write(ReadOnlySpan<char> buffer) => Write(buffer.ToString());
#endif

    public override void Write(char[] buffer, int index, int count) => Write(new string(buffer, index, count));

    public override void Flush() => _writer.Flush();

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _writer.Dispose();

        base.Dispose(disposing);
    }

    private sealed class Writer : RollingFileWriter
    {
        public Writer(string directory, string fileName, string extension) : base(directory, "yyyyMMdd", fileName) => FileExtension = extension;
    }
}

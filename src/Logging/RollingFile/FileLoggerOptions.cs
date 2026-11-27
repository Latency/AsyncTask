// ****************************************************************************
// Project:  Logging
// File:     FileLoggerOptions.cs
// Author:   Latency McLaughlin
// Date:     06/24/2024
// ****************************************************************************

namespace Logging.RollingFile;

/// <summary>Options for file logging. Each record is written to disk before the log call returns.</summary>
public class FileLoggerOptions
{
    private string _fileName               = "diagnostics-";
    private int?   _fileSizeLimit          = 10485760;
    private int?   _retainedFileCountLimit;
    private int?   _retentionDays          = 7;
    private string _logDirectory           = "LogFiles";

    /// <summary>
    ///     Gets or sets a strictly positive value representing the maximum log size in bytes or null for no limit.
    ///     When a day's file reaches this size, further messages go to the next numbered file of that day.
    ///     Defaults to <c>10MB</c>.
    /// </summary>
    public int? FileSizeLimit
    {
        get => _fileSizeLimit;
        set
        {
            if ((value.GetValueOrDefault() <= 0) & value.HasValue)
                throw new ArgumentOutOfRangeException(nameof(value), "FileSizeLimit must be positive.");

            _fileSizeLimit = value;
        }
    }

    /// <summary>
    ///     Gets or sets a strictly positive value representing the maximum retained file count or null for no limit.
    ///     Defaults to <c>null</c>.
    /// </summary>
    public int? RetainedFileCountLimit
    {
        get => _retainedFileCountLimit;
        set
        {
            if ((value.GetValueOrDefault() <= 0) & value.HasValue)
                throw new ArgumentOutOfRangeException(nameof(value), "RetainedFileCountLimit must be positive.");

            _retainedFileCountLimit = value;
        }
    }

    /// <summary>
    ///     Gets or sets a strictly positive number of days a closed log file is kept, or null for no limit.
    ///     Older files are removed when a new file opens. Defaults to <c>7</c>.
    /// </summary>
    public int? RetentionDays
    {
        get => _retentionDays;
        set
        {
            if ((value.GetValueOrDefault() <= 0) & value.HasValue)
                throw new ArgumentOutOfRangeException(nameof(value), "RetentionDays must be positive.");

            _retentionDays = value;
        }
    }

    /// <summary>Use AAEC daily numbered files and retain whole daily groups instead of individual files.</summary>
    public bool UseAaecFileLayout { get; set; }

    /// <summary>Maximum numbered files per day in AAEC layout, or null for unlimited.</summary>
    public int? FilesPerDayLimit { get; set; }

    /// <summary>Propagate file operation failures to the caller rather than dropping the record.</summary>
    public bool ThrowOnWriteFailure { get; set; }

    /// <summary>Close the file after each record so other processes can append or open it.</summary>
    public bool CloseFileAfterWrite { get; set; }

    /// <summary>Serialize file selection, maintenance, and writes across cooperating processes on the same machine.</summary>
    /// <remarks>Requires CloseFileAfterWrite. Processes must use the same normalized local path and filename prefix.</remarks>
    public bool CoordinateProcesses { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether scopes are included in each message. Defaults to <c>false</c>.
    /// </summary>
    public bool IncludeScopes { get; set; }

    /// <summary>
    ///     Gets or sets a string representing the prefix of the file name used to store the logging information.
    ///     The current date, in the format YYYYMMDD will be added after the given value.
    ///     Defaults to <c>diagnostics-</c>.
    /// </summary>
    public string FileName
    {
        get => _fileName;
        set
        {
            #if !NET7_0_OR_GREATER
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(null, nameof(value));
            #else
            ArgumentException.ThrowIfNullOrEmpty(value);
            #endif

            _fileName = value;
        }
    }

    /// <summary>
    ///     Gets or sets the directory to place the log files within.
    ///     Defaults to <c>LogFiles</c>.
    /// </summary>
    public string LogDirectory
    {
        get => _logDirectory;
        set
        {
            #if !NET7_0_OR_GREATER
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(null, nameof(value));
            #else
            ArgumentException.ThrowIfNullOrEmpty(value);
            #endif

            _logDirectory = value;
        }
    }
}
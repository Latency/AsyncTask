// ****************************************************************************
// Project:  HSMS.Logging
// File:     LogWriter.cs
// Author:   Latency McLaughlin
// Date:     06/30/2021
// ****************************************************************************

using Logging.Extensions;
using Logging.RollingFile;
using Microsoft.Extensions.Logging;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Logging;

public class Program : IDisposable
{
    private readonly ILogger        _logger;
    private readonly ILoggerFactory  _factory;

    private static readonly Lazy<Program> SingletonInstance = new(() => new());
    public static Program Instance => SingletonInstance.Value;


    public static ILogger Logger => Instance._logger;


    /// <summary> 
    ///     Constructor 
    /// </summary>
    private Program()
    {
        _factory = LoggerFactory.Create(builder =>
        {
            builder
               .SetMinimumLevel(LogLevel.Trace)
               .AddFile(LoggerOptions)
               //.AddDebug()
               .AddSimpleConsole(options =>
                {
                   options.SingleLine      = true;
                   options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
               })
               ;
        });

        _logger = _factory.CreateLogger<Program>();
    }


    private void LoggerOptions(FileLoggerOptions options)
    {
        options.FileName      = "log-";           // The log file prefixes
        options.LogDirectory  = "LogFiles";       // The directory to write the logs
        options.FileSizeLimit = 20 * 1024 * 1024; // The maximum log file size (20MB here)
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _factory.Dispose();
    }
}
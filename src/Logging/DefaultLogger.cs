// ****************************************************************************
// Project:  AsyncTask
// File:     DefaultLogger.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Diagnostics.CodeAnalysis;
using AsyncTask.Interfaces;
using Microsoft.Extensions.Logging;

#if !NETCOREAPP1_1_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
using static System.Diagnostics.Trace;
#endif

namespace AsyncTask.Logging;

/// <summary>
///     SECS Connector Logger
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public partial class DefaultLogger
{
    #region Constructors
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="synchronizationContext"></param>
    public DefaultLogger(SynchronizationContext? synchronizationContext = null)
    {
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        SynchronizationContext = synchronizationContext;

        UiFactory = new(TaskScheduler.Current);

        _isEnabled = true;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger = Initialize(LogLevel.Trace);
        #else
        _listeners = Listeners;
        #endif
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructors


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public void Enable()
    {
        if (_isEnabled)
            return;

        _isEnabled = true;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger = Initialize(LogLevel.Trace);
        #else
        _listeners.AddRange(Listeners);
        #endif
    }


    public void Disable()
    {
        if (!_isEnabled)
            return;

        _isEnabled = false;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger = Initialize(LogLevel.None);
        #else
        _listeners.Clear();
        #endif
    }


    public void Trace(string msg)
    {
        if (!_isEnabled)
            return;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.LogTrace(msg);
        #endif

        OnTrace(msg);
    }


    public void Debug(string msg)
    {
        if (!_isEnabled)
            return;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.LogDebug(msg);
        #else
        WriteLine(msg);
        #endif

        OnDebug(msg);
    }


    public void Error(string msg, Exception? ex)
    {
        if (!_isEnabled)
            return;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.LogError(ex, msg);
        #else
        if (ex != null)
            msg += $"{Environment.NewLine}{ex}";
        TraceError(msg);
        #endif

        OnError(msg, ex);
    }


    public void Information(string msg)
    {
        if (!_isEnabled)
            return;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.LogInformation(msg);
        #else
        TraceInformation(msg);
        #endif

        OnInformation(msg);
    }


    public void Warning(string msg)
    {
        if (!_isEnabled)
            return;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.LogWarning(msg);
        #else
        TraceWarning(msg);
        #endif

        OnWarning(msg);
    }


    public void Critical(string msg)
    {
        if (!_isEnabled)
            return;

        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.LogCritical(msg);
        #endif

        OnCritical(msg);
    }


    public void None(string msg)
    {
        if (!_isEnabled)
            return;

        OnNone(msg);
    }


    public void Add(LogLevel enm, Action<IMessageEventArgs> del)
    {
        switch (enm)
        {
            case LogLevel.None:
                NoneHandler += del;
                break;
            case LogLevel.Debug:
                DebugHandler += del;
                break;
            case LogLevel.Error:
                ErrorHandler += del;
                break;
            case LogLevel.Critical:
                CriticalHandler += del;
                break;
            case LogLevel.Information:
                InformationHandler += del;
                break;
            case LogLevel.Trace:
                TraceHandler += del;
                break;
            case LogLevel.Warning:
                WarningHandler += del;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(enm), enm, null);
        }
    }


    public void Remove(LogLevel enm, Action<IMessageEventArgs> del)
    {
        switch (enm)
        {
            case LogLevel.None:
                NoneHandler -= del;
                break;
            case LogLevel.Debug:
                DebugHandler -= del;
                break;
            case LogLevel.Error:
                ErrorHandler -= del;
                break;
            case LogLevel.Critical:
                CriticalHandler -= del;
                break;
            case LogLevel.Information:
                InformationHandler -= del;
                break;
            case LogLevel.Trace:
                TraceHandler -= del;
                break;
            case LogLevel.Warning:
                WarningHandler -= del;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(enm), enm, null);
        }
    }


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        _logger.Log(logLevel, eventId, state, exception, formatter);
        #else
        throw new NotSupportedException();
        #endif
    }


    public bool IsEnabled(LogLevel logLevel)
    {
        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return _logger.IsEnabled(logLevel);
        #else
        throw new NotSupportedException();
        #endif
    }


    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return _logger.BeginScope(state);
        #else
        throw new NotSupportedException();
        #endif
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    { }


    #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private static Microsoft.Extensions.Logging.ILogger Initialize(LogLevel logLevel) => LoggerFactory.Create(builder => builder.SetMinimumLevel(logLevel)).CreateLogger(typeof(object));
    #endif
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}
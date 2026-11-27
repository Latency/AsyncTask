// ****************************************************************************
// Project:  AsyncTask
// File:     DefaultLogger.HSMS.cs
// Author:   Latency McLaughlin
// Date:     11/26/2026
// ****************************************************************************

using AsyncTask.Interfaces;
using AsyncTask.Models;
using Microsoft.Extensions.Logging;

namespace AsyncTask.Logging;

public sealed class DefaultLogger : global::AsyncTask.Interfaces.ILogger
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private bool _isEnabled = true;

    private event Action<IMessageEventArgs>? CriticalHandler;
    private event Action<IMessageEventArgs>? DebugHandler;
    private event Action<IMessageEventArgs>? ErrorHandler;
    private event Action<IMessageEventArgs>? InformationHandler;
    private event Action<IMessageEventArgs>? NoneHandler;
    private event Action<IMessageEventArgs>? TraceHandler;
    private event Action<IMessageEventArgs>? WarningHandler;

    public DefaultLogger(SynchronizationContext? synchronizationContext = null)
    {
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        SynchronizationContext = synchronizationContext;

#if NET11_0_OR_GREATER
        _logger = global::Logging.Program.Logger;
#else
        _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
#endif
    }

    public DefaultLogger(SynchronizationContext? synchronizationContext, Microsoft.Extensions.Logging.ILogger logger)
    {
        SynchronizationContext = synchronizationContext;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public SynchronizationContext? SynchronizationContext { get; }

    public void Enable() => _isEnabled = true;
    public void Disable() => _isEnabled = false;

    public void Trace(string msg) { if (!_isEnabled) return; _logger.Log(LogLevel.Trace, default, msg, null, (state, _) => state); TraceHandler?.Invoke(CreateMessage(msg)); }
    public void Debug(string msg) { if (!_isEnabled) return; _logger.Log(LogLevel.Debug, default, msg, null, (state, _) => state); DebugHandler?.Invoke(CreateMessage(msg)); }
    public void Information(string msg) { if (!_isEnabled) return; _logger.Log(LogLevel.Information, default, msg, null, (state, _) => state); InformationHandler?.Invoke(CreateMessage(msg)); }
    public void Warning(string msg) { if (!_isEnabled) return; _logger.Log(LogLevel.Warning, default, msg, null, (state, _) => state); WarningHandler?.Invoke(CreateMessage(msg)); }
    public void Error(string msg, Exception? ex = null) { if (!_isEnabled) return; _logger.Log(LogLevel.Error, default, msg, ex, (state, _) => state); ErrorHandler?.Invoke(CreateMessage(msg, ex)); }
    public void Critical(string msg) { if (!_isEnabled) return; _logger.Log(LogLevel.Critical, default, msg, null, (state, _) => state); CriticalHandler?.Invoke(CreateMessage(msg)); }
    public void None(string msg) { if (!_isEnabled) return; NoneHandler?.Invoke(CreateMessage(msg)); }

    public void Add(LogLevel enm, Action<IMessageEventArgs> del)
    {
        switch (enm)
        {
            case LogLevel.None: NoneHandler += del; break;
            case LogLevel.Debug: DebugHandler += del; break;
            case LogLevel.Error: ErrorHandler += del; break;
            case LogLevel.Critical: CriticalHandler += del; break;
            case LogLevel.Information: InformationHandler += del; break;
            case LogLevel.Trace: TraceHandler += del; break;
            case LogLevel.Warning: WarningHandler += del; break;
            default: throw new ArgumentOutOfRangeException(nameof(enm), enm, null);
        }
    }

    public void Remove(LogLevel enm, Action<IMessageEventArgs> del)
    {
        switch (enm)
        {
            case LogLevel.None: NoneHandler -= del; break;
            case LogLevel.Debug: DebugHandler -= del; break;
            case LogLevel.Error: ErrorHandler -= del; break;
            case LogLevel.Critical: CriticalHandler -= del; break;
            case LogLevel.Information: InformationHandler -= del; break;
            case LogLevel.Trace: TraceHandler -= del; break;
            case LogLevel.Warning: WarningHandler -= del; break;
            default: throw new ArgumentOutOfRangeException(nameof(enm), enm, null);
        }
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _logger.BeginScope(state);
    public bool IsEnabled(LogLevel logLevel) => _isEnabled && _logger.IsEnabled(logLevel);
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);

    public void Dispose() => GC.SuppressFinalize(this);

    private static IMessageEventArgs CreateMessage(string message, Exception? exception = null) => new MessageEventArgs
    {
        Message = $"{message}{Environment.NewLine}{(exception != null ? $"{exception.Message}{Environment.NewLine}" : string.Empty)}",
        Exception = exception
    };
}

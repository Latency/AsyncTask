// ****************************************************************************
// Project:  AsyncTask
// File:     ILogger.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AsyncTask.Interfaces;

public interface ILogger : Microsoft.Extensions.Logging.ILogger, IDisposable
{
    SynchronizationContext? SynchronizationContext { get; }

    void Enable();
    void Disable();
    void Trace(string       msg);
    void Debug(string       msg);
    void Information(string msg);
    void Warning(string     msg);
    void Error(string       msg, Exception? ex = null);
    void Critical(string    msg);
    void None(string        msg);


    void Add(LogLevel    enm, Action<IMessageEventArgs> del);
    void Remove(LogLevel enm, Action<IMessageEventArgs> del);
}
// ****************************************************************************
// Project:  AsyncTask
// File:     ILoggerHandlers.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace AsyncTask.Interfaces;

public interface ILoggerHandlers
{
    event Action<IMessageEventArgs>? CriticalHandler;
    event Action<IMessageEventArgs>? DebugHandler;
    event Action<IMessageEventArgs>? ErrorHandler;
    event Action<IMessageEventArgs>? InformationHandler;
    event Action<IMessageEventArgs>? NoneHandler;
    event Action<IMessageEventArgs>? TraceHandler;
    event Action<IMessageEventArgs>? WarningHandler;
}
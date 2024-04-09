// ****************************************************************************
// Project:  AsyncTask
// File:     LoggerHandlers.Schema.cs
// Author:   Latency McLaughlin
// Date:     04/13/2024
// ****************************************************************************

using System.Diagnostics;
using AsyncTask.Interfaces;

namespace AsyncTask.Logging;

/// <summary>
///     SECS Connector Logger
/// </summary>
public abstract partial class LoggerHandlers
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected TaskFactory UiFactory = new();
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Event Handlers
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected event Action<IMessageEventArgs>? CriticalHandler;
    protected event Action<IMessageEventArgs>? DebugHandler;
    protected event Action<IMessageEventArgs>? ErrorHandler;
    protected event Action<IMessageEventArgs>? InformationHandler;
    protected event Action<IMessageEventArgs>? NoneHandler;
    protected event Action<IMessageEventArgs>? TraceHandler;
    protected event Action<IMessageEventArgs>? WarningHandler;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Event Handlers
}
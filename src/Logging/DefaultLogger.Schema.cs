// ****************************************************************************
// Project:  AsyncTask
// File:     DefaultLogger.Schema.cs
// Author:   Latency McLaughlin
// Date:     04/13/2024
// ****************************************************************************

using System.Diagnostics;
using ILogger = AsyncTask.Interfaces.ILogger;

namespace AsyncTask.Logging;

/// <summary>
///     SECS Connector Logger
/// </summary>
public partial class DefaultLogger : LoggerHandlers, ILogger
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool _isEnabled;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    #if NETCOREAPP1_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private Microsoft.Extensions.Logging.ILogger _logger;
    #else
    private readonly TraceListenerCollection _listeners;
    #endif
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public SynchronizationContext? SynchronizationContext { get; }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}
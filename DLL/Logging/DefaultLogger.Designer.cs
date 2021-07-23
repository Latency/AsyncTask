// ****************************************************************************
// Project:  Medtronic.MFI.Secs4Net
// File:     DefaultLogger.Designer.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

#nullable enable

using System.Diagnostics;
using System.Threading;
using AsyncTask.Interfaces;

#if !NET5_0_OR_GREATER
using static System.Diagnostics.Trace;
#endif

namespace AsyncTask.Logging
{
    /// <summary>
    ///     SECS Connector Logger
    /// </summary>
    public partial class DefaultLogger : LoggerHandlers, ILogger
    {
        #region Fields
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool                             _isEnabled;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        #if NET5_0_OR_GREATER
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
}
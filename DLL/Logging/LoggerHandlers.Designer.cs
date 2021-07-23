// ****************************************************************************
// Project:  AsyncTask
// File:     LoggerHandlers.Designer.cs
// Author:   Latency McLaughlin
// Date:     07/22/2021
// ****************************************************************************

#nullable enable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncTask.Interfaces;

namespace AsyncTask.Logging
{
    /// <summary>
    ///     SECS Connector Logger
    /// </summary>
    public abstract partial class LoggerHandlers
    {
        #region Fields
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected TaskFactory uiFactory = new TaskFactory();
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
}
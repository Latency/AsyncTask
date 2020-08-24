// *****************************************************************************
// File:       SecsLogger.cs
// Solution:   ORM-Monitor
// Project:    GUI
// Date:       08/23/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Diagnostics;
using ORM_Monitor.Enums;
using ORM_Monitor.EventArgs;

namespace ORM_Monitor.Logging
{
    public sealed class Logger : DefaultLogger
    {
        #region Methods

        // ====================================================================

        [DebuggerStepThrough]
        public override void Info(string msg)
        {
            if (!IsEnabled)
                return;
            LoggingEvents[LogType.Info].Dispatch(this, new MessageEventArgs {Message = $"{msg}{Environment.NewLine}"});
        }

        [DebuggerStepThrough]
        public override void Warning(string msg)
        {
            if (!IsEnabled)
                return;
            LoggingEvents[LogType.Warning].Dispatch(this, new MessageEventArgs {Message = $"{msg}{Environment.NewLine}"});
        }

        [DebuggerStepThrough]
        public override void Error(string msg, Exception ex = null)
        {
            if (!IsEnabled)
                return;
            LoggingEvents[LogType.Error].Dispatch(this, new MessageEventArgs {Message = $"{msg}{Environment.NewLine}"});
            if (ex != null)
                LoggingEvents[LogType.Error].Dispatch(this, new MessageEventArgs {Message = $"{ex}{Environment.NewLine}"});
        }

        [DebuggerStepThrough]
        public override void Debug(string msg)
        {
            if (!IsEnabled)
                return;
            LoggingEvents[LogType.Debug].Dispatch(this, new MessageEventArgs {Message = $"{msg}{Environment.NewLine}"});
        }

        // ====================================================================

        #endregion Methods
    }
}
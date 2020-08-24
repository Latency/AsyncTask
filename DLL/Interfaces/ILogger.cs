// ****************************************************************************
// Project:  AsyncTask
// File:     ILogger.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections.Generic;
using AsyncTask.Enums;
using AsyncTask.EventArgs;

namespace AsyncTask.Interfaces
{
    public interface ILogger
    {
        bool IsEnabled { get; set; }

        Dictionary<LogType, EventElement<MessageEventArgs>> LoggingEvents { get; set; }

        void Debug(string msg);
        void Info(string msg);
        void Warning(string msg);
        void Error(string msg, Exception ex = null);
    }
}
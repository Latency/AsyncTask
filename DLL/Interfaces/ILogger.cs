// *****************************************************************************
// File:       ILogger.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Collections.Generic;
using ORM_Monitor.Enums;
using ORM_Monitor.EventArgs;

namespace ORM_Monitor.Interfaces
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
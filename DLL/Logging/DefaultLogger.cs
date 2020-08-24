// *****************************************************************************
// File:       DefaultLogger.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ORM_Monitor.Enums;
using ORM_Monitor.EventArgs;
using ORM_Monitor.Interfaces;
using static System.Diagnostics.Trace;

namespace ORM_Monitor.Logging
{
    /// <summary>
    ///     SECS Connector Logger
    /// </summary>
    public class DefaultLogger : ILogger
    {
        private readonly List<TraceListener> _listeners = new List<TraceListener>();
        private Dictionary<LogType, EventElement<MessageEventArgs>> _events;
        private bool _isEnabled;


        public DefaultLogger()
        {
            Listeners.Clear();
            _listeners.Add(new ConsoleTraceListener());
            IsEnabled = true;
        }


        /// <summary>
        ///     EventHandlers for all the connection states.
        /// </summary>
        public Dictionary<LogType, EventElement<MessageEventArgs>> LoggingEvents { get; set; } = new Dictionary<LogType, EventElement<MessageEventArgs>>
        {
            {LogType.Info, new EventElement<MessageEventArgs>()},
            {LogType.Warning, new EventElement<MessageEventArgs>()},
            {LogType.Error, new EventElement<MessageEventArgs>()},
            {LogType.Debug, new EventElement<MessageEventArgs>()}
        };

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;
                _isEnabled = value;
                if (!_isEnabled)
                {
                    _events = LoggingEvents;
                    LoggingEvents.Clear();
                    Listeners.Clear();
                } else
                {
                    if (LoggingEvents.Count == 0)
                        LoggingEvents = new Dictionary<LogType, EventElement<MessageEventArgs>>(_events);
                    Listeners.AddRange(_listeners.ToArray());
                }
            }
        }

        public virtual void Debug(string msg) => WriteLine(msg);

        public virtual void Info(string msg) => TraceInformation(msg);

        public virtual void Warning(string msg) => TraceWarning(msg);

        public virtual void Error(string msg, Exception ex = null)
        {
            if (ex != null)
                msg += $"{Environment.NewLine}{ex}";
            TraceError(msg);
        }
    }
}
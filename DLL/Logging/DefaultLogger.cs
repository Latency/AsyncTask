// ****************************************************************************
// Project:  AsyncTask
// File:     DefaultLogger.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using AsyncTask.Enums;
using AsyncTask.EventArgs;
using AsyncTask.Interfaces;
using static System.Diagnostics.Trace;

namespace AsyncTask.Logging
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
#if !NETSTANDARD2_1
            _listeners.Add(new ConsoleTraceListener());
#endif
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
                }
                else
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
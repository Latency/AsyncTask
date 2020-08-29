// ****************************************************************************
// Project:  AsyncTask
// File:     DefaultLogger.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Diagnostics;
using AsyncTask.Interfaces;
using static System.Diagnostics.Trace;

namespace AsyncTask.Logging
{
    /// <summary>
    ///     Logger
    /// </summary>
    public class DefaultLogger : ILogger
    {
        private readonly TraceListenerCollection _listeners;
        private bool _isEnabled;


        public DefaultLogger()
        {
            _listeners = Listeners;
            _isEnabled = true;
        }


        public virtual bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;
                _isEnabled = value;
                if (!_isEnabled)
                {
                    _listeners.Clear();
                }
                else
                {
                    _listeners.AddRange(Listeners);
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
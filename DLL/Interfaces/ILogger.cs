// ****************************************************************************
// Project:  AsyncTask
// File:     ILogger.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;

namespace AsyncTask.Interfaces
{
    public interface ILogger
    {
        bool IsEnabled { get; set; }

        void Debug(string msg);
        void Error(string msg, Exception ex = null);
        void Information(string msg);
        void Warning(string msg);
    }
}
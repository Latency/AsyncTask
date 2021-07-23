// ****************************************************************************
// Project:  AsyncTask
// File:     TaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.Interfaces;

namespace AsyncTask.DTO
{
    internal class TaskEventArgs : EventArgs, ITaskEventArgs
    {
        public Exception Exception { get; internal set; }
        public DateTime  StartTime { get; internal set; }
        public DateTime  EndTime   { get; internal set; }
        public TimeSpan  Duration  { get; internal set; }
    }
}
// ****************************************************************************
// Project:  AsyncTask
// File:     TaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using AsyncTask.Interfaces;

namespace AsyncTask.Models;

internal class TaskEventArgs : EventArgs, ITaskEventArgs
{
    public Exception? Exception { get; internal set; }
    public DateTime?  StartTime { get; internal set; }
    public DateTime?  EndTime   { get; internal set; }
    public TimeSpan?  Duration  { get; internal set; }
}
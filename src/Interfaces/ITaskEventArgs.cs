// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace AsyncTask.Interfaces;

public interface ITaskEventArgs
{
    Exception? Exception { get; }
    DateTime?  StartTime { get; }
    DateTime?  EndTime   { get; }
    TimeSpan?  Duration  { get; }
}
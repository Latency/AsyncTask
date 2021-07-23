// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     05/20/2021
// ****************************************************************************

using System;

namespace AsyncTask.Interfaces
{
    public interface ITaskEventArgs
    {
        Exception Exception { get; }
        DateTime  StartTime { get; }
        DateTime  EndTime   { get; }
        TimeSpan  Duration  { get; }
    }
}
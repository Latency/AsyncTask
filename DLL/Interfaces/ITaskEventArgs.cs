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
        Exception Exception { get; internal set; }
        DateTime  StartTime { get; internal set; }
        TimeSpan  Duration  { get; internal set; }
    }
}
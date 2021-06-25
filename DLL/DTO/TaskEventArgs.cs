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
        Exception ITaskEventArgs.Exception { get; set; }
        DateTime ITaskEventArgs. StartTime { get; set; }
        TimeSpan ITaskEventArgs. Duration  { get; set; }
    }
}
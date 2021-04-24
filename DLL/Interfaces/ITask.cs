// ****************************************************************************
// Project:  AsyncTask
// File:     ITask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading.Tasks;

namespace AsyncTask.Interfaces
{
    public interface ITask
    {
        TimeSpan  PollInterval { get; set; }
        TimeSpan? Timeout      { get; set; }
        Task      Task         { get; }
        ILogger   Logger       { get; set; }
        ITaskInfo TaskInfo     { get; set; }
        ITaskList TaskList     { get; set; }
        void      Start();
        void      Cancel(bool throwOnFirstException = false);
    }
}
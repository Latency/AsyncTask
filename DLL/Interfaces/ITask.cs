// ****************************************************************************
// Project:  AsyncTask
// File:     ITask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System.Collections.Concurrent;

namespace AsyncTask.Interfaces
{
    public interface ITask
    {
        ConcurrentDictionary<ITaskInfo, ITask> TaskList { get; set; }
        ITaskInfo TaskInfo { get; set; }
        ILogger Logger { get; set; }
        void Cancel(bool throwOnFirstException = false);
    }
}
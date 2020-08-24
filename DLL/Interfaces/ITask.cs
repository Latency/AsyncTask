// *****************************************************************************
// File:       ITask.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System.Collections.Concurrent;

namespace ORM_Monitor.Interfaces
{
    public interface ITask
    {
        ConcurrentDictionary<ITaskInfo, ITask> TaskList { get; set; }
        ITaskInfo TaskInfo { get; set; }
        ILogger Logger { get; set; }
        void Cancel(bool throwOnFirstException = false);
    }
}
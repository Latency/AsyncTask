// ****************************************************************************
// Project:  AsyncTask
// File:     TaskList.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System.Collections.Concurrent;
using AsyncTask.Interfaces;

namespace AsyncTask.DTO
{
    public class TaskList : ConcurrentDictionary<ITask, ITaskInfo>, ITaskList { }
}
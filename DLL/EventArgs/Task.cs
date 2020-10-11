// ****************************************************************************
// Project:  AsyncTask
// File:     TaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading.Tasks;
using AsyncTask.Extensions;
using AsyncTask.Interfaces;

namespace AsyncTask.EventArgs
{
    public class TaskEventArgs<TTaskInfo, TTaskList> : System.EventArgs where TTaskInfo : ITaskInfo where TTaskList : ITaskList
    {
        public TaskEventArgs(Task task)
        {
            Task = task;
        }

        public Task Task { get; }

        public TTaskInfo TaskInfo { get; set; }

        public TTaskList TaskList { get; set; }

        public ILogger Logger { get; set; }

        public Exception Exception { get; set; }

        public dynamic Result { get; set; }

        public string Duration => TimeExtensions.ToString(DateTime.Now.Subtract(TaskStartTime));

        public DateTime TaskStartTime { get; } = DateTime.Now;
    }
}
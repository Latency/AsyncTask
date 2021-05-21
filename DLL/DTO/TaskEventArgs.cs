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

namespace AsyncTask.DTO
{
    public class TaskEventArgs : EventArgs, ITaskEventArgs
    {
        public TaskEventArgs(Task task)
        {
            Task = task;
        }

        public Task      Task          { get; }
        public ITaskInfo TaskInfo      { get; set; }
        public ITaskList TaskList      { get; set; }
        public ILogger   Logger        { get; set; }
        public Exception Exception     { get; set; }
        public dynamic   Result        { get; set; }
        public string    Duration      => TimeExtensions.ToString(DateTime.Now.Subtract(TaskStartTime));
        public DateTime  TaskStartTime { get; } = DateTime.Now;
    }
}
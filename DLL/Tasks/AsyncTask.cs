// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.DTO;
using AsyncTask.EventArgs;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask : TaskBase<AsyncTask, TaskInfo, TaskList, Action<AsyncTask, TaskEventArgs<TaskInfo, TaskList>>>
    {
        public AsyncTask()
        {
            Parent = this;
        }

        protected override AsyncTask Parent { get; }
    }
}
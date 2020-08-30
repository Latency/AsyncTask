// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask_T.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.DTO;
using AsyncTask.EventArgs;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask<T> : TaskBase<AsyncTask<T>, TaskInfo, TaskList, Func<AsyncTask<T>, TaskEventArgs<TaskInfo, TaskList>, T>>
    {
        public AsyncTask()
        {
            Parent = this;
        }

        protected override AsyncTask<T> Parent { get; }
    }
}
// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask_T.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.DTO;
using AsyncTask.EventArgs;
using AsyncTask.Tasks;
using ORM_Monitor.Models;

namespace ORM_Monitor.Tasks
{
    public sealed class AsyncTask<T> : TaskBase<AsyncTask<T>, TaskRecordSet, TaskList, Func<AsyncTask<T>, TaskEventArgs<TaskRecordSet, TaskList>, T>>
    {
        public AsyncTask()
        {
            Parent = this;
        }

        protected override AsyncTask<T> Parent { get; }
    }
}
// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
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
    public sealed class AsyncTask : TaskBase<AsyncTask, TaskRecordSet, TaskList, Action<AsyncTask, TaskEventArgs<TaskRecordSet, TaskList>>>
    {
        public AsyncTask()
        {
            Parent = this;
        }

        protected override AsyncTask Parent { get; }
    }
}
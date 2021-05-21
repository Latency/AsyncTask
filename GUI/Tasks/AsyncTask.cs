// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.Interfaces;
using AsyncTask.Tasks;

namespace ORM_Monitor.Tasks
{
    public sealed class AsyncTask : TaskBase<AsyncTask, Action<AsyncTask, ITaskEventArgs>>
    {
        public AsyncTask()
        {
            Parent = this;
        }

        protected override AsyncTask Parent { get; }
    }
}
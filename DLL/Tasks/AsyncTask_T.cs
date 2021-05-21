// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask_T.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.Interfaces;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask<T> : TaskBase<AsyncTask<T>, Func<AsyncTask<T>, ITaskEventArgs, T>>
    {
        public AsyncTask()
        {
            Parent = this;
        }

        protected override AsyncTask<T> Parent { get; }
    }
}
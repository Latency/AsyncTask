// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask_T.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading.Tasks;
using AsyncTask.DTO;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask<T> : TaskBase<Func<AsyncTask<T>, T>, AsyncTask<T>, TaskInfo, TaskList, Task<T>>
    {
        public AsyncTask()
        {
            Parent = this;
            Wrap<T>();
        }
    }
}
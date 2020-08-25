// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading.Tasks;
using AsyncTask.DTO;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask : TaskBase<Action<AsyncTask>, AsyncTask, TaskInfo, TaskList, Task>
    {
        public AsyncTask()
        {
            Parent = this;
            Wrap();
        }
    }
}
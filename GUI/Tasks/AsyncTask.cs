// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading.Tasks;
using AsyncTask.DTO;
using AsyncTask.Tasks;
using ORM_Monitor.Models;

namespace ORM_Monitor.Tasks
{
    public sealed class AsyncTask : TaskBase<Action<AsyncTask>, AsyncTask, TaskRecordSet, TaskList, Task>
    {
        public AsyncTask()
        {
            Parent = this;
            Wrap();
        }
    }
}
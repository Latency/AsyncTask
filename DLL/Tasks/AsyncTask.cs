// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask : TaskBase<Action<CancellationToken>, Task>
    {
        public AsyncTask() => Wrap();
    }
}
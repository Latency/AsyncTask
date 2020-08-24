// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask_T.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTask.Tasks
{
    public sealed class AsyncTask<T> : TaskBase<Func<CancellationToken, T>, Task<T>>
    {
        public AsyncTask() => Wrap<T>();
    }
}
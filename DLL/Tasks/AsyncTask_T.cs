// *****************************************************************************
// File:       AsyncTask_T.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ORM_Monitor.Tasks
{
    public sealed class AsyncTask<T> : TaskBase<Func<CancellationToken, T>, Task<T>>
    {
        public AsyncTask() => Wrap<T>();
    }
}
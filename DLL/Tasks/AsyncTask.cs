// *****************************************************************************
// File:       AsyncTask.cs
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
    public sealed class AsyncTask : TaskBase<Action<CancellationToken>, Task>
    {
        public AsyncTask() => Wrap();
    }
}
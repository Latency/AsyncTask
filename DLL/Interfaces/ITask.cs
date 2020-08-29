// ****************************************************************************
// Project:  AsyncTask
// File:     ITask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading.Tasks;

namespace AsyncTask.Interfaces
{
    public interface ITask
    {
        TimeSpan? Timeout { get; set; }
        Task Task { get; }
        ILogger Logger { get; set; }
        void Start();
    }
}
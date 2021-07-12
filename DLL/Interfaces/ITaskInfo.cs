// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskInfo.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System.Threading;

namespace AsyncTask.Interfaces
{
    public interface ITaskInfo
    {
        string Name { get; set; }
        CancellationToken Token { get; }
    }
}
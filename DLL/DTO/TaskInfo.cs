// ****************************************************************************
// Project:  AsyncTask
// File:     TaskInfo.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System.Threading;
using AsyncTask.Interfaces;

namespace AsyncTask.DTO
{
    // ReSharper disable once UnusedMember.Global
    public class TaskInfo : ITaskInfo
    {
        public string Name { get; set; }

        CancellationToken ITaskInfo.Token => Token;

        public CancellationToken Token { get; internal set; }
    }
}
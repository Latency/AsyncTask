// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskInfo.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.ComponentModel;
using System.Threading;

namespace AsyncTask.Interfaces
{
    public interface ITaskInfo : INotifyPropertyChanged
    {
        string                 Name                   { get; set; }
        SynchronizationContext SynchronizationContext { get; set; }
        TimeSpan?              Timeout                { get; set; }
        CancellationToken      Token                  { get; }
        TimeSpan               PollInterval           { get; set; }
        ITaskList              TaskList               { get; set; }
        ILogger                Logger                 { get; set; }
    }
}
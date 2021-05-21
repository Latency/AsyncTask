// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     05/20/2021
// ****************************************************************************

using System;
using System.Threading.Tasks;

namespace AsyncTask.Interfaces
{
    public interface ITaskEventArgs
    {
        Task      Task          { get; }
        ITaskInfo TaskInfo      { get; set; }
        ITaskList TaskList      { get; set; }
        ILogger   Logger        { get; set; }
        Exception Exception     { get; set; }
        dynamic   Result        { get; set; }
        string    Duration      { get; }
        DateTime  TaskStartTime { get; }
    }
}
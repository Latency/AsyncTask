// ****************************************************************************
// Project:  AsyncTask
// File:     TaskInfo.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using AsyncTask.Interfaces;

namespace AsyncTask.DTO
{
    public class TaskInfo : ITaskInfo
    {
        public string Name { get; set; }
    }
}
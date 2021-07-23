// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskList.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System.Collections.Generic;

namespace AsyncTask.Interfaces
{
    public interface ITaskList : IDictionary<ITask, ITaskInfo>
    { }
}
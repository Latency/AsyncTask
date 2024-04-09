// ****************************************************************************
// Project:  AsyncTask
// File:     ITaskList.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace AsyncTask.Interfaces;

public interface ITaskList : IDictionary<ITask, ITaskInfo>
{ }
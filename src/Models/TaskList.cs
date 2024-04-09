// ****************************************************************************
// Project:  AsyncTask
// File:     TaskList.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Collections.Concurrent;
using AsyncTask.Interfaces;

namespace AsyncTask.Models;

public class TaskList : ConcurrentDictionary<ITask, ITaskInfo>, ITaskList
{ }
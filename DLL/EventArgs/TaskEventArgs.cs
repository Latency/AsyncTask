// *****************************************************************************
// File:       TaskEventArgs.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Collections.Concurrent;
using System.Threading;
using ORM_Monitor.Extensions;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.EventArgs
{
    public class TaskEventArgs<T> : System.EventArgs, ITask
    {
        protected DateTime TaskStartTime = DateTime.Now;


        public TaskEventArgs(CancellationTokenSource cts, bool isGeneric)
        {
            CancellationTokenSource = cts;
            IsGeneric = isGeneric;
        }


        private TaskEventArgs((TaskEventArgs<T> tea, DateTime startTime) tuple)
        {
            var (tea, startTime) = tuple;
            tea.TaskStartTime = startTime;
            Clone(tea);
        }


        private TaskEventArgs((TaskEventArgs<T> tea, T task) tuple)
        {
            var (tea, task) = tuple;
            tea.Task = task;
            Clone(tea);
        }


        public T Task { get; private set; }

        public bool IsGeneric { get; private set; }

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public string Duration => TimeExtensions.ToString(DateTime.Now.Subtract(TaskStartTime));

        public void Cancel(bool throwOnFirstException = false) => CancellationTokenSource.Cancel();

        public ConcurrentDictionary<ITaskInfo, ITask> TaskList { get; set; }

        public ITaskInfo TaskInfo { get; set; }

        public ILogger Logger { get; set; }


        private void Clone(TaskEventArgs<T> tea)
        {
            TaskStartTime = tea.TaskStartTime;
            Task = tea.Task;
            IsGeneric = tea.IsGeneric;
            CancellationTokenSource = tea.CancellationTokenSource;
            TaskList = tea.TaskList;
            TaskInfo = tea.TaskInfo;
            Logger = tea.Logger;
        }


        public static implicit operator TaskEventArgs<T>((TaskEventArgs<T> tea, DateTime startTime) tuple) => new TaskEventArgs<T>(tuple);

        public static implicit operator TaskEventArgs<T>((TaskEventArgs<T> tea, T task) tuple) => new TaskEventArgs<T>(tuple);
    }
}
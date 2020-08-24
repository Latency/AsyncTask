// ****************************************************************************
// Project:  AsyncTask
// File:     TaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections.Concurrent;
using System.Threading;
using AsyncTask.Extensions;
using AsyncTask.Interfaces;

namespace AsyncTask.EventArgs
{
    public class TaskEventArgs<T> : System.EventArgs, ITask
    {
        protected DateTime TaskStartTime = DateTime.Now;


        public TaskEventArgs(CancellationTokenSource cts, bool isGeneric)
        {
            CancellationTokenSource = cts;
            IsGeneric = isGeneric;
        }


#if NET45
        private TaskEventArgs(Tuple<TaskEventArgs<T>, DateTime> tuple)
        {
            tuple.Item1.TaskStartTime = tuple.Item2;
            Clone(tuple.Item1);
        }


        private TaskEventArgs(Tuple<TaskEventArgs<T>, T> tuple)
        {
            tuple.Item1.Task = tuple.Item2;
            Clone(tuple.Item1);
        }
#else
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
#endif


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


#if NET45
        public static implicit operator TaskEventArgs<T>(Tuple<TaskEventArgs<T>, DateTime> tuple) => new TaskEventArgs<T>(tuple);
        public static implicit operator TaskEventArgs<T>(Tuple<TaskEventArgs<T>, T> tuple) => new TaskEventArgs<T>(tuple);
#else
        public static implicit operator TaskEventArgs<T>((TaskEventArgs<T> tea, DateTime startTime) tuple) => new TaskEventArgs<T>(tuple);
        public static implicit operator TaskEventArgs<T>((TaskEventArgs<T> tea, T task) tuple) => new TaskEventArgs<T>(tuple);
#endif
    }
}
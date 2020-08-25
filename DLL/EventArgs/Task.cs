// ****************************************************************************
// Project:  AsyncTask
// File:     TaskEventArgs.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Threading;
using AsyncTask.Extensions;
using AsyncTask.Interfaces;
using AsyncTask.Logging;

namespace AsyncTask.EventArgs
{
    public class TaskEventArgs<TTaskInfo, TTaskList, TTask> : System.EventArgs where TTaskInfo : ITaskInfo where TTaskList : ITaskList
    {
        protected DateTime TaskStartTime = DateTime.Now;


        public TaskEventArgs(CancellationTokenSource cts, bool isGeneric)
        {
            CancellationTokenSource = cts;
            IsGeneric = isGeneric;
        }


#if NET45
        private TaskEventArgs(Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, DateTime> tuple)
        {
            tuple.Item1.TaskStartTime = tuple.Item2;
            Clone(tuple.Item1);
        }


        private TaskEventArgs(Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, TTask> tuple)
        {
            tuple.Item1.Task = tuple.Item2;
            Clone(tuple.Item1);
        }
#else
        private TaskEventArgs((TaskEventArgs<TTaskInfo, TTaskList,  TTask> tea, DateTime startTime) tuple)
        {
            var (tea, startTime) = tuple;
            tea.TaskStartTime = startTime;
            Clone(tea);
        }


        private TaskEventArgs((TaskEventArgs<TTaskInfo, TTaskList,  TTask> tea, TTask task) tuple)
        {
            var (tea, task) = tuple;
            tea.Task = task;
            Clone(tea);
        }
#endif


        public TTask Task { get; private set; }

        public bool IsGeneric { get; private set; }

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public string Duration => TimeExtensions.ToString(DateTime.Now.Subtract(TaskStartTime));

        public void Cancel(bool throwOnFirstException = false) => CancellationTokenSource.Cancel();

        public TTaskList TaskList { get; set; }

        public TTaskInfo TaskInfo { get; set; }

        public ILogger Logger { get; set; } = new DefaultLogger();


        private void Clone(TaskEventArgs<TTaskInfo, TTaskList, TTask> tea)
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
        public static implicit operator TaskEventArgs<TTaskInfo, TTaskList, TTask>(Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, DateTime> tuple) => new TaskEventArgs<TTaskInfo, TTaskList, TTask>(tuple);
        public static implicit operator TaskEventArgs<TTaskInfo, TTaskList, TTask>(Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, TTask> tuple) => new TaskEventArgs<TTaskInfo, TTaskList, TTask>(tuple);
#else
        public static implicit operator TaskEventArgs<TTaskInfo, TTaskList,  TTask>((TaskEventArgs<TTaskInfo, TTaskList,  TTask> tea, DateTime startTime) tuple) => new TaskEventArgs<TTaskInfo, TTaskList,  TTask>(tuple);
        public static implicit operator TaskEventArgs<TTaskInfo, TTaskList,  TTask>((TaskEventArgs<TTaskInfo, TTaskList,  TTask> tea, TTask task) tuple) => new TaskEventArgs<TTaskInfo, TTaskList,  TTask>(tuple);
#endif
    }
}
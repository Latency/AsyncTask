// ****************************************************************************
// Project:  AsyncTask
// File:     TaskBase.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask.EventArgs;
using AsyncTask.Interfaces;
using AsyncTask.Logging;

namespace AsyncTask.Tasks
{
    /// <summary>
    ///     AsyncTask
    /// </summary>
    /// <remarks>
    ///     CurrentId is a static property that is used to get the identifier of the currently executing task from the code that the task is executing.
    ///     It differs from the Id property, which returns the identifier of a particular Task instance.
    ///     If you attempt to retrieve the CurrentId value from outside the code that a task is executing, the property returns null.
    ///     Note that although collisions are very rare, task identifiers are not guaranteed to be unique.
    /// </remarks>
    public abstract class TaskBase<TParent, TTaskInfo, TTaskList, TDelegate> : CancellationTokenSource, ITask
        where TTaskInfo : class, ITaskInfo
        where TTaskList : class, ITaskList
        where TParent   : class, new()
    {
        private readonly TaskEventArgs<TTaskInfo, TTaskList> _eventArgs;
        private bool TimeOut => Timeout != null && DateTime.Now >= _eventArgs.TaskStartTime.Add((TimeSpan)Timeout);

        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnAdd      { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnRemove   { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnComplete { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnError    { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnCanceled { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnTick     { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnTimeout  { get; set; }
        public TDelegate                                            Delegate   { get; set; }


        /// <summary>
        ///     Constructor
        /// </summary>
        protected TaskBase()
        {
            _eventArgs = new TaskEventArgs<TTaskInfo, TTaskList>(Wrap());
        }


        /// <summary>
        ///     Parent
        /// </summary>
        protected abstract TParent Parent { get; }


        /// <summary>
        ///     Timeout
        /// </summary>
        public TimeSpan? Timeout { get; set; }



        /// <summary>
        ///     Poll Interval
        /// </summary>
        public TimeSpan PollInterval { get; set; } = new(0, 0, 1);


        /// <summary>
        ///     Task
        /// </summary>
        public Task Task => _eventArgs.Task;


        /// <summary>
        ///     TaskInfo
        /// </summary>
        public ITaskInfo TaskInfo
        {
            get => _eventArgs.TaskInfo;
            set => _eventArgs.TaskInfo = (TTaskInfo) value;
        }


        /// <summary>
        ///     TaskList
        /// </summary>
        public ITaskList TaskList
        {
            get => _eventArgs.TaskList;
            set => _eventArgs.TaskList = (TTaskList) value;
        }

        
        /// <summary>
        ///     Logger
        /// </summary>
        public ILogger Logger
        {
            get => _eventArgs.Logger;
            set => _eventArgs.Logger = value;
        }


        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _eventArgs.TaskInfo.Name;

        
        /// <summary>
        ///     Start
        /// </summary>
        public void Start()
        {
            var isCompleted = false;

            Task.Run(async () =>
            {
                while (!TimeOut)
                {
                    if (isCompleted)
                        return;

                    try
                    {
                        // Poll every 1 second.
                        await Task.Delay((int) PollInterval.TotalMilliseconds, Token);
                        OnTick?.Invoke(Parent, _eventArgs);
                    }
                    catch (TaskCanceledException)
                    {
                        OnCanceled?.Invoke(Parent, _eventArgs);
                        return;
                    }
                }
                OnTimeout?.Invoke(Parent, _eventArgs);
                Cancel();
            });



            Task.Run(() =>
            {
                try
                {
                    // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
                    // .Net v4.5 (C#7.3) does not support null Coalecing assignments.
                    if (Logger == null)
                        Logger = new DefaultLogger();

                    if (TaskList != null)
                    {
                        if (TaskList is not ConcurrentDictionary<ITaskInfo, ITask> taskList)
                            throw new NullReferenceException();

                        taskList.TryAdd(TaskInfo, this);
                    }

                    OnAdd?.Invoke(Parent, _eventArgs);

                    var po = new ParallelOptions
                    {
                        CancellationToken = Token,
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    };
                    Parallel.ForEach(new[] { _eventArgs.Task }, po, (task, _) =>
                    {
                        using (Token.Register(Thread.CurrentThread.Abort))
                        {
                            task.RunSynchronously();
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    // User canceled or timeout
                }
                catch (Exception ex)
                {
                    _eventArgs.Exception = ex;
                    OnError?.Invoke(Parent, _eventArgs);
                }
                finally
                {
                    isCompleted = true;
                }
            }).ContinueWith(t => // Cleanup
            {
                Dispose(); // Cancellation token
                _eventArgs.Task.Dispose();

                if (TaskList != null)
                {
                    if (TaskList is not ConcurrentDictionary<ITaskInfo, ITask> taskList)
                        throw new NullReferenceException();

                    taskList.TryRemove(TaskInfo, out _);
                }

                OnRemove?.Invoke(Parent, _eventArgs);
            });
        }


        /// <summary>
        ///     Wrap
        /// </summary>
        private Task Wrap() => new(() =>
        {
            try
            {
                var method = (Delegate) GetType().GetProperty("Delegate")?.GetValue(this, null);
                if (method == null)
                    throw new NullReferenceException();

                if (method.GetType().GenericTypeArguments[0].IsGenericType)
                    _eventArgs.Result = method.DynamicInvoke(Parent, _eventArgs);
                else
                    method.DynamicInvoke(Parent, _eventArgs);

                OnComplete?.Invoke(Parent, _eventArgs);
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }, Token);
    }
}
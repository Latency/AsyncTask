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
using AsyncTask.DTO;
using AsyncTask.Enums;
using AsyncTask.EventArgs;
using AsyncTask.Interfaces;

namespace AsyncTask.Tasks
{
    /// <summary>
    ///     AsyncTask
    /// </summary>
    /// <remarks>
    ///     CurrentId is a static property that is used to get the identifier of the currently executing task from the code
    ///     that the task is executing.
    ///     It differs from the Id property, which returns the identifier of a particular Task instance.
    ///     If you attempt to retrieve the CurrentId value from outside the code that a task is executing, the property returns
    ///     null.
    ///     Note that although collisions are very rare, task identifiers are not guaranteed to be unique.
    /// </remarks>
    public abstract class TaskBase<TParent, TTaskInfo, TTaskList, TDelegate> : CancellationTokenSource, ITask where TTaskInfo : TaskInfo where TTaskList : TaskList where TParent : new()
    {
        private TTaskInfo _taskInfo;
        private TTaskList _taskList;
        private ILogger _logger;
        private TaskEventArgs<TTaskInfo, TTaskList> _eventArgs;
        private bool TimeOut => Timeout != null && DateTime.Now >= _eventArgs.TaskStartTime.Add((TimeSpan)Timeout);
        private void DebugStatus(TaskType type, string msg) => Logger.Debug($"{type} '{TaskInfo.Name}':  {msg}");

        public TDelegate Delegate { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnAdd { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnRemove { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnComplete { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnError { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnCanceled { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList>> OnTimeout { get; set; }


        /// <summary>
        ///     Parent
        /// </summary>
        protected abstract TParent Parent { get; }


        /// <summary>
        ///     Timeout
        /// </summary>
        public TimeSpan? Timeout { get; set; }


        /// <summary>
        ///     Task
        /// </summary>
        public Task Task => _eventArgs.Task;


        /// <summary>
        ///     TaskInfo
        /// </summary>
        public TTaskInfo TaskInfo
        {
            get => _eventArgs.TaskInfo;
            set => _taskInfo = value;
        }


        /// <summary>
        ///     TaskList
        /// </summary>
        public TTaskList TaskList
        {
            get => _eventArgs.TaskList;
            set =>_taskList = value;
        }

        
        /// <summary>
        ///     Logger
        /// </summary>
        public ILogger Logger
        {
            get => _eventArgs.Logger;
            set => _logger = value;
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
            _eventArgs = new TaskEventArgs<TTaskInfo, TTaskList>(Wrap(), _taskInfo, _taskList, _logger);

            var isCompleted = false;

            Task.Run(async () =>
            {
#if DEBUG
                var i = -1;
#endif
                while (!TimeOut)
                {
                    if (isCompleted)
                        return;
#if DEBUG
                    if (++i > 0)
                        DebugStatus(TaskType.Monitor, $"Tick #{i}");
#endif
                    try
                    {
                        // Poll every 1 second.
                        await Task.Delay(TimeSpan.FromSeconds(1), Token);
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
                    DebugStatus(TaskType.Task, "Adding task");
                    var taskList = TaskList as ConcurrentDictionary<TTaskInfo, ITask>;
                    taskList?.TryAdd(TaskInfo, this);
                    OnAdd?.Invoke(Parent, _eventArgs);

                    var po = new ParallelOptions
                    {
                        CancellationToken = Token,
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    };
                    Parallel.ForEach(new[] { _eventArgs.Task }, po, (task, loopState) =>
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
                DebugStatus(TaskType.Task, "Removing task");
                var taskList = TaskList as ConcurrentDictionary<TTaskInfo, ITask>;
                taskList?.TryRemove(TaskInfo, out _);
                OnRemove?.Invoke(Parent, _eventArgs);
            });
        }


        /// <summary>
        ///     Wrap
        /// </summary>
        private Task Wrap() => new Task(() =>
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
                throw new TaskCanceledException();
            }
        }, Token);
    }
}
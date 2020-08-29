// ****************************************************************************
// Project:  AsyncTask
// File:     TaskBase.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public abstract class TaskBase<TDelegate, TParent, TTaskInfo, TTaskList, TTask> : CancellationTokenSource, ITask where TTaskInfo : ITaskInfo where TTaskList : ITaskList
    {
        protected TParent Parent;
        private TaskEventArgs<TTaskInfo, TTaskList, TTask> _eventArgs;
        private bool TimeOut => Timeout != null && DateTime.Now >= _eventArgs.TaskStartTime.Add((TimeSpan)Timeout);
        private void DebugStatus(TaskType type, string msg) => Logger.Debug($"{type} '{TaskInfo.Name}':  {msg}");

        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> Delegate { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnAdd { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnRemove { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnComplete { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnError { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnCanceled { get; set; }
        public Action<TParent, TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnTimeout { get; set; }


        /// <summary>
        ///     Constructor
        /// </summary>
        protected TaskBase() => _eventArgs = new TaskEventArgs<TTaskInfo, TTaskList, TTask>(this, typeof(TTask).GenericTypeArguments.Any());


        /// <summary>
        ///     Timeout
        /// </summary>
        public TimeSpan? Timeout { get; set; }


        /// <summary>
        ///     Task
        /// </summary>
        public Task Task => _eventArgs.Task as Task;


        /// <summary>
        ///     TaskList
        /// </summary>
        public TTaskList TaskList
        {
            get => _eventArgs.TaskList;
            set => _eventArgs.TaskList = value;
        }


        /// <summary>
        ///     TaskInfo
        /// </summary>
        public TTaskInfo TaskInfo
        {
            get => _eventArgs.TaskInfo;
            set => _eventArgs.TaskInfo = value;
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
            // Reset start time.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, DateTime>(_eventArgs, DateTime.Now);
#else
            _eventArgs = (_eventArgs, DateTime.Now);
#endif

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
                    Parallel.ForEach(new[] { _eventArgs.Task }, po, (item, loopState) =>
                    {
                        using (Token.Register(Thread.CurrentThread.Abort))
                        {
                            if (!(item is Task task))
                                throw new NullReferenceException();
                            task.RunSynchronously();
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    // User canceled
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
            }).ContinueWith(t =>
            {
                Dispose();
            });
        }


        private new void Dispose()
        {
            DebugStatus(TaskType.Task, "Removing task");
            OnRemove?.Invoke(Parent, _eventArgs);
            var taskList = TaskList as ConcurrentDictionary<TTaskInfo, ITask>;
            taskList?.TryRemove(TaskInfo, out _);
            base.Dispose(); // Cancellation token
            if (!(_eventArgs.Task is Task task))
                throw new NullReferenceException();
            task.Dispose();
        }
        

        protected void Wrap()
        {
            void WrappedDelegate()
            {
                try
                {
                    dynamic method = GetType().GetProperty("Delegate")?.GetValue(this, null);
                    method?.Invoke(Parent, _eventArgs);
                    OnComplete?.Invoke(Parent, _eventArgs);
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                    throw new TaskCanceledException();
                }
            }
            // Implicit assignment since Task won't have a setter.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, TTask>(_eventArgs, (TTask)Activator.CreateInstance(typeof(Task), (Action)WrappedDelegate, Token));
#else
            _eventArgs = (_eventArgs, (TTask) Activator.CreateInstance(typeof(Task), (Action) WrappedDelegate, Token));
#endif
        }


        protected void Wrap<T>()
        {
            T WrappedDelegate()
            {
                try
                {
                    dynamic method = GetType().GetProperty("Delegate")?.GetValue(this, null);
                    var result = method?.Invoke(Parent, _eventArgs);
                    OnComplete?.Invoke(Parent, _eventArgs);
                    return result;
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                    throw new TaskCanceledException();
                }
            }
            // Implicit assignment since Task won't have a setter.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, TTask>(_eventArgs, (TTask)Activator.CreateInstance(typeof(Task<>).MakeGenericType(typeof(T)), (Func<T>)WrappedDelegate, Token));
#else
            _eventArgs = (_eventArgs, (TTask) Activator.CreateInstance(typeof(Task<>).MakeGenericType(typeof(T)), (Func<T>) WrappedDelegate, Token));
#endif
        }
    }
}
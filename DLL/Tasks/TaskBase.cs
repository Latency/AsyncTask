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
        private TaskEventArgs<TTaskInfo, TTaskList, TTask> _eventArgs;
        protected TParent Parent;


        /// <summary>
        ///     Constructor
        /// </summary>
        protected TaskBase() => _eventArgs = new TaskEventArgs<TTaskInfo, TTaskList, TTask>(this, typeof(TTask).GenericTypeArguments.Any());


        public TDelegate Delegate { get; set; }
        public TimeSpan? Timeout { get; set; }

        public Action<TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnAdd { get; set; }
        public Action<TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnRemove { get; set; }
        public Action<TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnComplete { get; set; }
        public Action<TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnCanceled { get; set; }
        public Action<TaskEventArgs<TTaskInfo, TTaskList, TTask>> OnTimeout { get; set; }

        public Task Task => _eventArgs.Task as Task;

        public TTaskList TaskList
        {
            get => _eventArgs.TaskList;
            set => _eventArgs.TaskList = value;
        }

        public TTaskInfo TaskInfo
        {
            get => _eventArgs.TaskInfo;
            set => _eventArgs.TaskInfo = value;
        }

        public ILogger Logger
        {
            get => _eventArgs.Logger;
            set => _eventArgs.Logger = value;
        }

        public override string ToString() => _eventArgs.TaskInfo.Name;


        public async void Register()
        {
            Token.Register(() => OnCanceled?.Invoke(_eventArgs));

            Logger.Debug($"\tAdding task '{_eventArgs.TaskInfo.Name}'");
            var taskList = TaskList as ConcurrentDictionary<TTaskInfo, ITask>;
            taskList?.TryAdd(TaskInfo, this);

            OnAdd?.Invoke(_eventArgs);

            if (!(_eventArgs.Task is Task task))
                throw new NullReferenceException();
            task.Start();

            // Reset start time.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, DateTime>(_eventArgs, DateTime.Now);
#else
            _eventArgs = (_eventArgs, DateTime.Now);
#endif

            if (Timeout != null)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay((TimeSpan) Timeout, Token).ContinueWith(t =>
                    {
                        if (t.IsCompleted)
                            OnTimeout?.Invoke(_eventArgs);
                    });
                    Cancel();
                }, Token);
                Console.Write("");
            }
        }


        private void UnRegister()
        {
            OnRemove?.Invoke(_eventArgs);
            Logger.Debug($"\tRemoving task '{_eventArgs.TaskInfo.Name}'");
            var taskList = TaskList as ConcurrentDictionary<TTaskInfo, ITask>;
            taskList?.TryRemove(TaskInfo, out _);
            Dispose(); // Cancellation token
        }


        protected void Wrap()
        {
            void WrappedDelegate()
            {
                try
                {
                    dynamic method = GetType().GetProperty("Delegate")?.GetValue(this, null);
                    method?.Invoke(Parent);
                    if (Timeout != null)
                    {
                        if (Task.Wait((int) Timeout.Value.TotalMilliseconds, Token))
                            OnComplete?.Invoke(_eventArgs);
                        else
                            OnTimeout?.Invoke(_eventArgs);
                    }
                    else
                        Task.Wait(Token);
                    OnComplete?.Invoke(_eventArgs);
                }
                finally
                {
                    UnRegister();
                }
            }

            // Implicit assignment since Task doesn't have a setter.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, TTask>(_eventArgs, (TTask)Activator.CreateInstance(typeof(Task), (Action)WrappedDelegate));
#else
            _eventArgs = (_eventArgs, (TTask) Activator.CreateInstance(typeof(Task), (Action) WrappedDelegate));
#endif
        }


        protected void Wrap<T>()
        {
            T WrappedDelegate()
            {
                try
                {
                    dynamic method = GetType().GetProperty("Delegate")?.GetValue(this, null);
                    var result = method?.Invoke(Parent);
                    OnComplete?.Invoke(_eventArgs);
                    return result;
                }
                finally
                {
                    UnRegister();
                }
            }

            // Implicit assignment since Task doesn't have a setter.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTaskInfo, TTaskList, TTask>, TTask>(_eventArgs, (TTask)Activator.CreateInstance(typeof(Task<>).MakeGenericType(typeof(T)), (Func<T>)WrappedDelegate));
#else
            _eventArgs = (_eventArgs, (TTask) Activator.CreateInstance(typeof(Task<>).MakeGenericType(typeof(T)), (Func<T>) WrappedDelegate));
#endif
        }
    }
}
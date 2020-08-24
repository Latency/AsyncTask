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
    public abstract class TaskBase<TDelegate, TTask> : CancellationTokenSource, ITask
    {
        private TaskEventArgs<TTask> _eventArgs;


        /// <summary>
        ///     Constructor
        /// </summary>
        protected TaskBase() => _eventArgs = new TaskEventArgs<TTask>(this, typeof(TTask).GenericTypeArguments.Any());


        public TDelegate Delegate { get; set; }
        public TimeSpan? Timeout { get; set; }

        public Action<TaskEventArgs<TTask>> OnAdd { get; set; }
        public Action<TaskEventArgs<TTask>> OnRemove { get; set; }
        public Action<TaskEventArgs<TTask>> OnComplete { get; set; }
        public Action<TaskEventArgs<TTask>> OnCanceled { get; set; }
        public Action<TaskEventArgs<TTask>> OnTimeout { get; set; }

        public Task Task => _eventArgs.Task as Task;

        public ConcurrentDictionary<ITaskInfo, ITask> TaskList
        {
            get => _eventArgs.TaskList;
            set => _eventArgs.TaskList = value;
        }

        public ITaskInfo TaskInfo
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
            Token.Register(async () => await Task.Run(() => OnCanceled?.Invoke(_eventArgs)).ConfigureAwait(false));

            Logger.Debug($"\tAdding task '{_eventArgs.TaskInfo.Name}'");
            TaskList.TryAdd(TaskInfo, this);

            await Task.Run(() => OnAdd?.Invoke(_eventArgs)).ConfigureAwait(false);

            if (!(_eventArgs.Task is Task task))
                throw new NullReferenceException();
            task.Start();

            // Reset start time.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTask>, DateTime>(_eventArgs, DateTime.Now);
#else
            _eventArgs = (_eventArgs, DateTime.Now);
#endif

            if (Timeout != null)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay((TimeSpan) Timeout, Token).ContinueWith(async t =>
                    {
                        if (t.IsCompleted)
                            await Task.Run(() => OnTimeout?.Invoke(_eventArgs)).ConfigureAwait(false);
                    }).ConfigureAwait(false);
                    Cancel();
                }, Token);
                Console.Write("");
            }
        }


        private async void UnRegister()
        {
            await Task.Run(() => OnRemove?.Invoke(_eventArgs)).ConfigureAwait(false);
            Logger.Debug($"\tRemoving task '{_eventArgs.TaskInfo.Name}'");
            TaskList.TryRemove(TaskInfo, out _);
            Dispose(); // Cancellation token
        }


        protected void Wrap()
        {
            async void WrappedDelegate()
            {
                try
                {
                    await Task.Run(() =>
                    {
                        dynamic method = GetType().GetProperty("Delegate")?.GetValue(this, null);
                        method?.Invoke(Token);
                    }).ConfigureAwait(false);
                    await Task.Run(() => OnComplete?.Invoke(_eventArgs)).ConfigureAwait(false);
                }
                finally
                {
                    UnRegister();
                }
            }

            // Implicit assignment since Task doesn't have a setter.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTask>, TTask>(_eventArgs, (TTask)Activator.CreateInstance(typeof(Task), (Action)WrappedDelegate));
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
                    var result = Task.Run(() =>
                    {
                        dynamic method = GetType().GetProperty("Delegate")?.GetValue(this, null);
                        return method?.Invoke(Token);
                    }).ConfigureAwait(false).GetAwaiter().GetResult();
                    Task.Run(() => OnComplete?.Invoke(_eventArgs)).ConfigureAwait(false).GetAwaiter().GetResult();
                    return result;
                }
                finally
                {
                    UnRegister();
                }
            }

            // Implicit assignment since Task doesn't have a setter.
#if NET45
            _eventArgs = new Tuple<TaskEventArgs<TTask>, TTask>(_eventArgs, (TTask)Activator.CreateInstance(typeof(Task<>).MakeGenericType(typeof(T)), (Func<T>)WrappedDelegate));
#else
            _eventArgs = (_eventArgs, (TTask) Activator.CreateInstance(typeof(Task<>).MakeGenericType(typeof(T)), (Func<T>) WrappedDelegate));
#endif
        }
    }
}
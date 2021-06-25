// ****************************************************************************
// Project:  AsyncTask
// File:     TaskBase.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask.DTO;
using AsyncTask.Interfaces;
using AsyncTask.Logging;

namespace AsyncTask
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
    public class AsyncTask : Task, ITask
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ITaskEventArgs _eventArgs = new TaskEventArgs();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CancellationTokenSource[] _cts = { new(), new() };
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<CancellationToken> _delegate;


        public Action<ITask, ITaskEventArgs> OnAdd      { private get; set; }
        public Action<ITask, ITaskEventArgs> OnRemove   { private get; set; }
        public Action<ITask, ITaskEventArgs> OnComplete { private get; set; }
        public Action<ITask, ITaskEventArgs> OnError    { private get; set; }
        public Action<ITask, ITaskEventArgs> OnCanceled { private get; set; }
        public Action<ITask, ITaskEventArgs> OnTick     { private get; set; }
        public Action<ITask, ITaskEventArgs> OnTimeout  { private get; set; }


        /// <summary>
        ///     Timeout
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        

        /// <summary>
        ///     Poll Interval
        /// </summary>
        public TimeSpan PollInterval { get; set; } = new(0, 0, 1);


        /// <summary>
        ///     TaskInfo
        /// </summary>
        public ITaskInfo TaskInfo { get; set; }


        /// <summary>
        ///     TaskList
        /// </summary>
        public ITaskList TaskList { get; set; }

        
        /// <summary>
        ///     Logger
        /// </summary>
        public ILogger Logger { get; set; }


        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => TaskInfo?.Name ?? nameof(AsyncTask);


        /// <summary>
        ///     Start
        /// </summary>
        public new async void Start()
        {
            Logger ??= new DefaultLogger();

            if (TaskList is not null && TaskInfo is not null)
            {
                if (TaskList is not IDictionary taskList)
                    throw new NullReferenceException();
                taskList.Add(TaskInfo, this);
            }

            OnAdd?.Invoke(this, _eventArgs);

            var monitor = new Task(async () =>
            {
                DateTime? startTime = DateTime.Now;

                while (true)
                {
                    if (Timeout is not null && DateTime.Now >= startTime.Value.Add((TimeSpan)Timeout))
                        break;

                    try
                    {
                        // Poll every 1 second.
                        await Delay((int)(PollInterval).TotalMilliseconds, _cts[0].Token);

                        _eventArgs.Duration = DateTime.Now.Subtract(startTime.Value);

                        OnTick?.Invoke(this, _eventArgs);
                    }
                    catch (TaskCanceledException)
                    {
                        _cts[1].Cancel();
                        return;
                    }
                }

                OnTimeout?.Invoke(this, _eventArgs);

                _cts[1].Cancel();
            });
            monitor.Start();


            try
            {
                _eventArgs.StartTime = DateTime.Now;

                await Run(async () =>
                {
                    try
                    {
                        _delegate.DynamicInvoke(_cts[1].Token);
                    }
                    catch (Exception ex)
                    {
                        _cts[0].Cancel();

                        switch (ex)
                        {
                            case OperationCanceledException:
                                break;
                            default:
                                await Delay(TimeSpan.FromMilliseconds(100));
                                _eventArgs.Exception = ex;
                                OnError?.Invoke(this, _eventArgs);
                                break;
                        }
                    }
                }).ContinueWith(async _ =>
                {
                    if (!_cts[1].IsCancellationRequested)
                    {
                        await Delay(TimeSpan.FromMilliseconds(100));
                        _cts[0].Cancel();
                        monitor.Wait();
                        OnComplete?.Invoke(this, _eventArgs);
                    }
                });
            }
            finally
            {
                if (TaskList is not null && TaskInfo is not null)
                {
                    if (TaskList is not IDictionary taskList)
                        throw new NullReferenceException();
                    taskList.Remove(TaskInfo);
                }

                await Delay(TimeSpan.FromMilliseconds(100));
                OnRemove?.Invoke(this, _eventArgs);
            }
        }


        /// <summary>
        ///     Send cancellation request for token.
        /// </summary>
        /// <param name="throwOnFirstException"></param>
        public void Cancel(bool throwOnFirstException = false)
        {
            _cts[0].Cancel(throwOnFirstException);
            OnCanceled?.Invoke(this, _eventArgs);
        }


        #region Constructors
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public AsyncTask(Action<CancellationToken> action) : this(action, new CancellationToken()) { }
        public AsyncTask(Action<CancellationToken> action, CancellationToken   cancellationToken) : this(action, cancellationToken, TaskCreationOptions.None) { }
        public AsyncTask(Action<CancellationToken> action, TaskCreationOptions creationOptions) : this(action, new CancellationToken(), creationOptions) { }
        public AsyncTask(Action<CancellationToken> action, CancellationToken   cancellationToken, TaskCreationOptions creationOptions) : base(() => action(cancellationToken), cancellationToken, creationOptions) => _delegate = action;
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        #endregion Constructors
    }
}
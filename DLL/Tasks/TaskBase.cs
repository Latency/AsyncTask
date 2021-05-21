// ****************************************************************************
// Project:  AsyncTask
// File:     TaskBase.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask.DTO;
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
    public abstract class TaskBase<TParent, TDelegate> : CancellationTokenSource, ITask
        where TParent   : class, new()
    {
        private readonly ITaskEventArgs _eventArgs;
        private bool TimeOut => Timeout != null && DateTime.Now >= _eventArgs.TaskStartTime.Add((TimeSpan)Timeout);

        public Action<TParent, ITaskEventArgs> OnAdd      { get; set; }
        public Action<TParent, ITaskEventArgs> OnRemove   { get; set; }
        public Action<TParent, ITaskEventArgs> OnComplete { get; set; }
        public Action<TParent, ITaskEventArgs> OnError    { get; set; }
        public Action<TParent, ITaskEventArgs> OnCanceled { get; set; }
        public Action<TParent, ITaskEventArgs> OnTick     { get; set; }
        public Action<TParent, ITaskEventArgs> OnTimeout  { get; set; }
        public TDelegate                       Delegate   { get; set; }


        /// <summary>
        ///     Constructor
        /// </summary>
        protected TaskBase()
        {
            _eventArgs = new TaskEventArgs(Wrap());
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
            set => _eventArgs.TaskInfo = value;
        }


        /// <summary>
        ///     TaskList
        /// </summary>
        public ITaskList TaskList
        {
            get => _eventArgs.TaskList;
            set => _eventArgs.TaskList = value;
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
                        if (TaskList is not IDictionary taskList)
                            throw new NullReferenceException();

                        taskList.Add(TaskInfo, this);
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
            }).ContinueWith(_ => // Cleanup
            {
                Dispose(); // Cancellation token
                _eventArgs.Task.Dispose();

                if (TaskList != null)
                {
                    if (TaskList is not IDictionary taskList)
                        throw new NullReferenceException();

                    taskList.Remove(TaskInfo);
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
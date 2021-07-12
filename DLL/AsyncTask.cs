// ****************************************************************************
// Project:  AsyncTask
// File:     TaskBase.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Diagnostics;
using System.Reflection;
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
        private readonly CancellationTokenSource[] _cts;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _canceled;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<ITask, ITaskEventArgs> _delegate;

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
        /// <remarks>
        ///     Poll every 1 second by default.
        /// </remarks>
        public TimeSpan PollInterval { get; set; } = new(0, 0, 1);


        /// <summary>
        ///     TaskInfo
        /// </summary>
        public ITaskInfo TaskInfo { get; set; } = null;


        /// <summary>
        ///     TaskList
        /// </summary>
        public ITaskList TaskList { get; set; } = null;

        
        /// <summary>
        ///     Logger
        /// </summary>
        public ILogger Logger { get; set; }


        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => TaskInfo?.Name ?? nameof(AsyncTask);


        public new TaskStatus Status { get; private set; }


        /// <summary>
        ///     Start
        /// </summary>
        public new async void Start()
        {
            Status    =   TaskStatus.Running;

            Logger    ??= new DefaultLogger();
            _canceled =   false;

            var ctx = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

            if (TaskInfo is TaskInfo taskInfo)
                taskInfo.Token = _cts[1].Token;
            else
                taskInfo = null;

            if (TaskList is not null && taskInfo is not null)
                TaskList.Add(taskInfo, this);

            OnAdd?.Invoke(this, _eventArgs);

            #region Monitor
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            var monitor = new Task(() =>
            {
                Thread.CurrentThread.Name = $"{taskInfo?.Name ?? nameof(AsyncTask)}:  Monitor";
                var startTime = DateTime.Now;
                var endTime   = ((DateTime?)startTime).Value.Add(Timeout ?? new TimeSpan(0));

                try
                {
                    while (!_cts[0].IsCancellationRequested)
                    {
                        try
                        {
                            Delay((int) (PollInterval).TotalMilliseconds).ContinueWith(t =>
                            {
                                if (t.IsCanceled)
                                    return;
                                 
                                var time = DateTime.Now;

                                _eventArgs.Duration = time.Subtract(startTime);
                                ctx.StartNew(() => OnTick?.Invoke(this, _eventArgs));

                                if (Timeout is null || time < endTime)
                                    return;

                                ctx.StartNew(() => OnTimeout?.Invoke(this, _eventArgs));
                                _cts[0].Cancel();
                            }).ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    _cts[1].Cancel();
                }
            });
            monitor.Start();
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            #endregion Monitor


            #region Main Delegate
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            var are = new AutoResetEvent(false);
            try
            {

                _eventArgs.StartTime = DateTime.Now;

                await Factory.StartNew(() =>
                {
                    Thread.CurrentThread.Name = $"{taskInfo?.Name ?? nameof(AsyncTask)}:  Main Delegate";
                    _delegate.Invoke(this, _eventArgs);
                }, _cts[1].Token).ContinueWith(_ =>
                {
                    Synchronize();
                    Status = TaskStatus.RanToCompletion;
                    ctx.StartNew(() => OnComplete?.Invoke(this, _eventArgs));
                    are.Set();
                }, _cts[1].Token);
            }
            catch (Exception ex)
            {                
                Synchronize();
                switch (ex)
                {
                    case OperationCanceledException:
                        if (_canceled)
                        {
                            Status = TaskStatus.Canceled;
                            OnCanceled?.Invoke(this, _eventArgs);
                        }
                        break;
                    default:
                        Status = TaskStatus.Faulted;
                        _eventArgs.Exception = ex;
                        OnError?.Invoke(this, _eventArgs);
                        break;
                }
                are.Set();
            }
            finally
            {
                are.WaitOne();

                if (TaskList is not null && TaskInfo is not null)
                    TaskList.Remove(TaskInfo);

                OnRemove?.Invoke(this, _eventArgs);
            }
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            #endregion Main Delegate

            // Local method
            void Synchronize()
            {
                if (!_cts[0].IsCancellationRequested)
                    _cts[0].Cancel();

                monitor.Wait();
            }
        }


        /// <summary>
        ///     Send cancellation request for token.
        /// </summary>
        /// <param name="throwOnFirstException"></param>
        public void Cancel(bool throwOnFirstException = false)
        {
            _canceled = true;
            _cts[0].Cancel(throwOnFirstException);
        }


        #region Constructors
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public AsyncTask(Action<ITask, ITaskEventArgs> action) : base(() => {}, new CancellationToken())
        {
            _delegate = action;
            _cts      = new CancellationTokenSource[] { new(), new() };

            #region Reflection
            var type = GetType().BaseType;
            if (type == null)
                throw new NullReferenceException();

            var field = type.GetField("m_action", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is null)
                throw new NullReferenceException();
            
            // Set the 1st argument of the base ctor via reflection since we require late binding here.
            field.SetValue(this, new Action(() => _delegate?.Invoke(this, _eventArgs)));
            
            var method = type.GetMethod("AssignCancellationToken", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method is null)
                throw new NullReferenceException();

            method.Invoke(this, new object[] { _cts[1].Token, null, null });
            #endregion Reflection
        }
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        #endregion Constructors
    }
}
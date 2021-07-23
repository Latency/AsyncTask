// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     07/27/2021
// ****************************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask.DTO;
using AsyncTask.Extensions;
using AsyncTask.Interfaces;

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
    public class AsyncTask : Task<bool>, ITask
    {
        #region Fields
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CancellationTokenSource[] _cts;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ITaskEventArgs _eventArgs = new TaskEventArgs();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<int> _setTaskStatus;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Exception _exception;
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Fields


        #region Constructor
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public AsyncTask(Action<ITask, ITaskEventArgs> func) : base(() => true, new CancellationToken())
        {
            if (func.Method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null)
                throw new InvalidOperationException("Delegate may not be asynchronous.");

            _cts = new CancellationTokenSource[]
            {
                new(),
                new()
            };

            TaskInfo.Name = nameof(AsyncTask);

            #region Reflection
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            var type = GetType().BaseType;
            if (type == null)
                throw new NullReferenceException();

            var field = type.GetField("m_action", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is null)
                throw new NullReferenceException();

            // Set the 1st argument of the base ctor via reflection since we require late binding here.
            field.SetValue(this, new Func<bool>(() =>
            {
                try
                {
                    func.Invoke(this, _eventArgs);
                    return true;
                }
                catch (OperationCanceledException)
                {
                    // Handled
                }
                catch (Exception ex)
                {
                    _setTaskStatus(0x200000);
                    _exception = ex;
                }

                return false;
            }));

            var method = type.BaseType?.GetMethod("AssignCancellationToken", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method is null)
                throw new NullReferenceException();

            method.Invoke(this, new object[]
            {
                _cts[1].Token,
                null,
                null
            });

            field = type.GetField("m_stateFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is null)
                throw new NullReferenceException();

            _setTaskStatus = value => field.SetValue(this, value, BindingFlags.Instance | BindingFlags.NonPublic, null, CultureInfo.CurrentCulture);
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            #endregion Reflection
        }
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Constructor

        public Action<ITask, ITaskEventArgs> OnAdd      { private get; set; }
        public Action<ITask, ITaskEventArgs> OnRemove   { private get; set; }
        public Action<ITask, ITaskEventArgs> OnComplete { private get; set; }
        public Action<ITask, ITaskEventArgs> OnError    { private get; set; }
        public Action<ITask, ITaskEventArgs> OnCanceled { private get; set; }
        public Action<ITask, ITaskEventArgs> OnTick     { private get; set; }
        public Action<ITask, ITaskEventArgs> OnTimeout  { private get; set; }


        /// <summary>
        ///     TaskInfo
        /// </summary>
        public ITaskInfo TaskInfo { get; set; } = new TaskInfo();


        /// <summary>
        ///     Start
        /// </summary>
        public new async void Start()
        {
            ((TaskInfo) TaskInfo).Token = _cts[1].Token;

            TaskInfo.TaskList ??= new TaskList();
            TaskInfo.TaskList.Add(this, TaskInfo);

            UiDispatcher.Initialize(TaskInfo.SynchronizationContext);

            await InvokeAsync(nameof(OnAdd), OnAdd);

            #region Monitor
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            var monitor = Run(async () =>
            {
                Thread.CurrentThread.Name = $"{TaskInfo}:  Monitor";

                var lastTime = ((TaskEventArgs) _eventArgs).StartTime = DateTime.Now;
                ((TaskEventArgs) _eventArgs).EndTime = ((DateTime?) _eventArgs.StartTime).Value.Add(TaskInfo.Timeout ?? new TimeSpan(0));

                try
                {
                    while (!_cts[0].IsCancellationRequested)
                    {
                        var thisTime = DateTime.Now;
                        if (TaskInfo.Timeout is not null && thisTime >= _eventArgs.EndTime)
                        {
                            _setTaskStatus(0x400000);
                            await InvokeAsync(nameof(OnTimeout), OnTimeout);
                            break;
                        }

                        var pulse = thisTime.Subtract(lastTime).Add(TaskInfo.PollInterval);
                        await Delay(pulse, _cts[0].Token).ContinueWith(async task =>
                        {
                            if (task.Status == TaskStatus.Canceled)
                                return;

                            ((TaskEventArgs) _eventArgs).Duration = thisTime.Subtract(_eventArgs.StartTime).Add(pulse);

                            await InvokeAsync(nameof(OnTick), OnTick);
                        });

                        lastTime = DateTime.Now;
                    }
                }
                finally
                {
                    _cts[1].Cancel();
                }
            }, _cts[0].Token);

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            #endregion Monitor


            #region Main Delegate
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            try
            {
                await Run(() =>
                {
                    Thread.CurrentThread.Name = "Main Delegate";
                    try
                    {
                        RunSynchronously();
                    }
                    catch (Exception)
                    {
                        // Handled
                    }
                }, _cts[1].Token);
            }
            finally
            {
                _cts[0].Cancel(); 
                
                monitor.Wait();

                _cts[0].Dispose();
                _cts[1].Dispose();
            }

            switch (Status)
            {
                case TaskStatus.Faulted:
                    ((TaskEventArgs)_eventArgs).Exception = _exception;
                    await InvokeAsync(nameof(OnError), OnError);
                    break;
                case TaskStatus.Canceled:
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    break;
                case TaskStatus.RanToCompletion:
                    await InvokeAsync(nameof(OnComplete), OnComplete);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await InvokeAsync(nameof(OnRemove), OnRemove);

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            #endregion Main Delegate
        }


        /// <summary>
        ///     Send cancellation request for token.
        /// </summary>
        /// <param name="throwOnFirstException"></param>
        public async void Cancel(bool throwOnFirstException = false)
        {
            _cts[0].Cancel(throwOnFirstException);
            _setTaskStatus(0x400000);
            await InvokeAsync(nameof(OnCanceled), OnCanceled);
        }
        

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => TaskInfo.Name;


        #region Local Methods
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        private Task InvokeAsync(string name, Action<ITask, ITaskEventArgs> method) => UiDispatcher.InvokeAsync(() =>
        {
            try
            {
                method?.Invoke(this, _eventArgs);
            }
            catch (Exception ex)
            {
                _cts[0].Cancel();
                _setTaskStatus(0x200000);
                _exception = Activator.CreateInstance(ex.GetType(), $"{name} -> {ex.Message}", ex) as Exception;
            }
        });
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Local Methods
    }
}
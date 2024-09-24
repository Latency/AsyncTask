// ****************************************************************************
// Project:  AsyncTask
// File:     AsyncTask.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using AsyncTask.Interfaces;
using AsyncTask.Models;

namespace AsyncTask;

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
public class AsyncTask : Task<bool>, ITask
{
    #region Constructor
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public AsyncTask(Action<ITask, ITaskEventArgs> func) : base(() => true, new CancellationToken())
    {
        if (func.Method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null)
            throw new InvalidOperationException("Delegate may not be asynchronous.");

        _cts =
        [
            new(),
            new()
        ];

        TaskInfo.Name = nameof(AsyncTask);

        #region Reflection
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        var type = GetType().BaseType;
        if (type == null)
            throw new NullReferenceException();

        var stateFlagsField = type.GetField("m_stateFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        if (stateFlagsField is null)
            throw new NullReferenceException("m_stateFlags");

        _setTaskStatus = value => stateFlagsField.SetValue(this, value, BindingFlags.Instance | BindingFlags.NonPublic, null, CultureInfo.CurrentCulture);

        var field = type.GetField("m_action", BindingFlags.Instance | BindingFlags.NonPublic);
        if (field is null)
            throw new NullReferenceException("m_action");

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
           catch (Exception? ex)
           {
               _setTaskStatus(TASK_STATE_FAULTED);
               _exception = ex;
           }

           return false;
        }));

        var method = type.BaseType?.GetMethod("AssignCancellationToken", BindingFlags.Instance | BindingFlags.NonPublic);
        if (method is null)
            throw new NullReferenceException("AssignCancellationToken");

        method.Invoke(this, [
                _cts[1].Token,
                null,
                null
            ]
        );

        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Reflection
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructor

    public Action<ITask, ITaskEventArgs>? OnAdd      { private get; set; }
    public Action<ITask, ITaskEventArgs>? OnRemove   { private get; set; }
    public Action<ITask, ITaskEventArgs>? OnComplete { private get; set; }
    public Action<ITask, ITaskEventArgs>? OnError    { private get; set; }
    public Action<ITask, ITaskEventArgs>? OnCanceled { private get; set; }
    public Action<ITask, ITaskEventArgs>? OnTick     { private get; set; }
    public Action<ITask, ITaskEventArgs>? OnTimeout  { private get; set; }


    /// <summary>
    ///     TaskInfo
    /// </summary>
    public ITaskInfo TaskInfo { get; set; } = new TaskInfo();


    /// <summary>
    ///     Start
    /// </summary>
    public new async void Start()
    {
        ((TaskInfo)TaskInfo).Token = _cts[1].Token;

        TaskInfo.TaskList ??= new TaskList();
        TaskInfo.TaskList.Add(this, TaskInfo);

        Dispatch(nameof(OnAdd), OnAdd);

        #region Monitor
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        var monitor = Run(async () =>
        {
            Thread.CurrentThread.Name = $"{TaskInfo}:  Monitor";
            var lastTime = ((TaskEventArgs)_eventArgs).StartTime = DateTime.Now;
            ((TaskEventArgs)_eventArgs).EndTime  = _eventArgs.StartTime?.Add(TaskInfo.Timeout ?? new TimeSpan(0));
            ((TaskEventArgs)_eventArgs).Duration = TimeSpan.Zero;

            try
            {
                while (!_cts[0].IsCancellationRequested)
                {
                    var thisTime = DateTime.Now;
                    if (TaskInfo.Timeout is not null && thisTime >= _eventArgs.EndTime)
                    {
                        _setTaskStatus(TASK_STATE_CANCELED);
                        Dispatch(nameof(OnTimeout), OnTimeout);
                        break;
                    }

                    var pulse = thisTime.Subtract((DateTime)lastTime).Add(TaskInfo.PollInterval);
                    await Delay(pulse, _cts[0].Token).ContinueWith(task =>
                    {
                        if (task.Status == TaskStatus.Canceled)
                            return;

                        ((TaskEventArgs)_eventArgs).Duration = thisTime.Subtract((DateTime)_eventArgs.StartTime!).Add(pulse);

                        Dispatch(nameof(OnTick), OnTick);
                    });

                    lastTime = DateTime.Now;
                }
            }
            finally
            {
                #if NET8_0_OR_GREATER
                await _cts[1].CancelAsync();
                #else
                _cts[1].Cancel();
                #endif
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
            #if NET8_0_OR_GREATER
            await _cts[0].CancelAsync();
            #else
            _cts[0].Cancel();
            #endif

            monitor.Wait();

            _cts[0].Dispose();
            _cts[1].Dispose();
        }

        switch (Status)
        {
            case TaskStatus.Faulted:
                ((TaskEventArgs)_eventArgs).Exception = _exception;
                Dispatch(nameof(OnError), OnError);
                break;
            case TaskStatus.Canceled:
            case TaskStatus.Created:
            case TaskStatus.WaitingForActivation:
            case TaskStatus.WaitingToRun:
            case TaskStatus.Running:
            case TaskStatus.WaitingForChildrenToComplete:
                break;
            case TaskStatus.RanToCompletion:
                Dispatch(nameof(OnComplete), OnComplete);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Dispatch(nameof(OnRemove), OnRemove);

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Main Delegate
    }


    /// <summary>
    ///     Send cancellation request for token.
    /// </summary>
    /// <param name="throwOnFirstException"></param>
    public void Cancel(bool throwOnFirstException = false)
    {
        _cts[0].Cancel(throwOnFirstException);
        _setTaskStatus(TASK_STATE_CANCELED);
        Dispatch(nameof(OnCanceled), OnCanceled);
    }


    /// <summary>
    ///     Executes an action on the UI thread. If this method is called from the UI thread, the action is executed
    ///     immendiately.
    ///     If the method is called from another thread, the action will be enqueued on the UI thread's dispatcher and executed
    ///     asynchronously.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="method">The action that will be executed on the UI thread.</param>
    public void Dispatch(string name, Action<ITask, ITaskEventArgs>? method)
    {
        if (TaskInfo.SynchronizationContext == null)
            throw new NullReferenceException("SynchronizationContext is null.");

        if (TaskInfo.SynchronizationContext == SynchronizationContext.Current)
            try
            {
                method?.Invoke(this, _eventArgs);
            }
            catch (Exception ex)
            {
                _cts[0].Cancel();
                _setTaskStatus(TASK_STATE_FAULTED);
                _exception = Activator.CreateInstance(ex.GetType(), $"{name} -> {ex.Message}", ex) as Exception;
            }
        else
            TaskInfo.SynchronizationContext.Send(_ =>
            {
                try
                {
                    method?.Invoke(this, _eventArgs);
                }
                catch (Exception ex)
                {
                    _cts[0].Cancel();
                    _setTaskStatus(TASK_STATE_FAULTED);
                    _exception = Activator.CreateInstance(ex.GetType(), $"{name} -> {ex.Message}", ex) as Exception;
                }
            }, null);
    }


    /// <summary>
    ///     ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString() => TaskInfo.Name;

    // ReSharper disable InconsistentNaming
    private const int TASK_STATE_FAULTED  = 0x200000;
    private const int TASK_STATE_CANCELED = 0x400000;
    // ReSharper restore InconsistentNaming


    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CancellationTokenSource[] _cts;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ITaskEventArgs _eventArgs = new TaskEventArgs();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<int> _setTaskStatus;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Exception? _exception;

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields
}
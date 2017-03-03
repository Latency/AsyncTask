//  *****************************************************************************
//  File:       TaskEvent.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ORM_Monitor.Events;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor {
  /// <summary>
  ///   TaskEvent
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class TaskEvent<T> : IDisposable {
    public void Dispose() {
      Task.Dispose();
      Task = null;
      GC.Collect();
    }

    #region Driver

    // -----------------------------------------------------------------------

    /// <summary>
    ///   Asyncronous delegate monitor.
    /// </summary>
    /// <returns>TaskName</returns>
    public Task AsyncMonitor() {
      var name = MethodBase.GetCurrentMethod().Name;

      Task = Task.Run(() => {
        Thread.CurrentThread.Name = name;

        // Awaits for blocking main actions.
        try {
          var thread = new Thread(() => {
            // ReSharper disable once InvertIf
            if (_onRunning != null && _onRunning.IsSubscribed) {
              try {
                _onRunning.Invoke();
                if (_onCompleted != null && _onCompleted.IsSubscribed)
                  _onCompleted.Invoke();
              } catch (ThreadAbortException) {
                Thread.ResetAbort();
              } catch (Exception ex) {
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
              }
            }
          }) {
            Name = $"{name}->Worker",
            IsBackground = true
          };

          thread.SetApartmentState(ApartmentState.STA);
          thread.Start();

          var endTime = DateTime.Now.Add(Duration);

          // Poll for cancellation request or timeout.
          while (thread.IsAlive) {
            if (TokenSource.IsCancellationRequested) {
              if (_onCanceled != null && _onCanceled.IsSubscribed)
                _onCanceled.Invoke();
              thread.Abort(TaskStatus.Canceled);
              break;
            }
            if (Duration >= TimeSpan.Zero && DateTime.Now >= endTime) {
              if (_onTimeout != null && _onTimeout.IsSubscribed)
                _onTimeout.Invoke();
              thread.Abort(TaskStatus.Faulted);
              break;
            }

            _onProgressChanged.Invoke();

            // Pulse 1/2 sec.
            Thread.Sleep(500);
          }

          thread.Join();
        } catch (ThreadInterruptedException ex) {
          Debug.WriteLine(ex.Message);
        } catch (OperationCanceledException ex) {
          Debug.WriteLine(ex.Message);
        } catch (Exception ex) {
          throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
        }
      }, TokenSource.Token);

      Task.ContinueWith(t => _onExit.Invoke());

      return Task;
    }

    // -----------------------------------------------------------------------

    #endregion Driver

    #region Expression Bodies

    // -----------------------------------------------------------------------

    /// <summary>
    ///   IsDisposed
    /// </summary>
    public bool IsDisposed => (bool) TokenSource.GetType().GetProperty("IsDisposed", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty).GetValue(TokenSource);

    // -----------------------------------------------------------------------

    #endregion Expression Bodies

    #region Constructors

    // -----------------------------------------------------------------------

    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="source"></param>
    /// <param name="timeSpan"></param>
    /// <param name="expression"></param>
    public TaskEvent(TaskEventArgs<T>.Expression expression, T source, TimeSpan timeSpan) {
      Name = string.Empty;
      Status = TaskStatus.Created;
      Duration = timeSpan;
      TokenSource = new CancellationTokenSource();

      _onCanceled = new CanceledEvent<T>(this, expression, source);
      _onCompleted = new CompletedEvent<T>(this, expression, source);
      _onProgressChanged = new ProgressChangedEvent<T>(this, expression, source);
      _onRunning = new RunningEvent<T>(this, expression, source);
      _onTimeout = new TimeoutEvent<T>(this, expression, source);
      _onExit = new ExitEvent<T>(this, expression, source);
    }


    /// <summary>
    ///   Constructor
    /// </summary>
    public TaskEvent(TaskEventArgs<T>.Expression expression, T source, double timeout = -1) : this(expression, source, TimeSpan.FromMilliseconds(timeout)) {
    }

    // -----------------------------------------------------------------------

    #endregion Constructors

    #region Events

    // -----------------------------------------------------------------------

    /// <summary>
    ///   CanceledAction - Event Handler for CanceledEvent<c>T</c>
    /// </summary>
    public void OnCanceled(EventHandler<TaskEventArgs<T>> action) {
      _onCanceled.OnCanceled += action;
    }

    /// <summary>
    ///   CompletedAction - Event Handler for CompletedEvent<c>T</c>
    /// </summary>
    public void OnCompleted(EventHandler<TaskEventArgs<T>> action) {
      _onCompleted.OnCompleted += action;
    }
    
    /// <summary>
    ///   ProgressChangedAction - Event Handler for ProgressChangedEvent<c>T</c>
    /// </summary>
    public void OnProgressChanged(EventHandler<TaskEventArgs<T>> action) {
      _onProgressChanged.OnProgressChanged += action;
    }
    
    /// <summary>
    ///   RunningAction - Event Handler for RunningEvent<c>T</c>
    /// </summary>
    public void OnRunning(EventHandler<TaskEventArgs<T>> action) {
      _onRunning.OnRunning += action;
    }

    /// <summary>
    ///   TimeoutAction - Event Handler for TimeoutEvent<c>T</c>
    /// </summary>
    public void OnTimeout(EventHandler<TaskEventArgs<T>> action) {
      _onTimeout.OnTimeout += action;
    }

    /// <summary>
    ///   TimeoutAction - Event Handler for TimeoutEvent<c>T</c>
    /// </summary>
    public void OnExit(EventHandler<TaskEventArgs<T>> action) {
      _onExit.OnExit += action;
    }

    // -----------------------------------------------------------------------

    #endregion Events

    #region Backing Fields

    // -----------------------------------------------------------------------
    // ReSharper disable InconsistentNaming

    /// <summary>
    ///   CanceledAction - Event Handler for CanceledEvent<c>T</c>
    /// </summary>
    private readonly ICanceledEvent<T> _onCanceled;

    /// <summary>
    ///   CompletedAction - Event Handler for CompletedEvent<c>T</c>
    /// </summary>
    private readonly ICompletedEvent<T> _onCompleted;

    /// <summary>
    ///   ProgressChangedAction - Event Handler for ProgressChangedEvent<c>T</c>
    /// </summary>
    private readonly IProgressChangedEvent<T> _onProgressChanged;

    /// <summary>
    ///   RunningAction - Event Handler for RunningEvent<c>T</c>
    /// </summary>
    private readonly IRunningEvent<T> _onRunning;

    /// <summary>
    ///   TimeoutAction - Event Handler for TimeoutEvent<c>T</c>
    /// </summary>
    private readonly ITimeoutEvent<T> _onTimeout;

    /// <summary>
    ///   ExitAction - Event Handler for TimeoutEvent<c>T</c>
    /// </summary>
    private readonly IExitEvent<T> _onExit;

    // ReSharper restore InconsistentNaming
    // -----------------------------------------------------------------------

    #endregion Backing Fields

    #region Properties

    // -----------------------------------------------------------------------

    /// <summary>
    ///   Task
    /// </summary>
    public Task Task { get; private set; }

    /// <summary>
    ///   TokenSource
    /// </summary>
    public CancellationTokenSource TokenSource { get; }

    /// <summary>
    ///   Duration
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    ///   Status
    /// </summary>
    public TaskStatus Status { get; }

    /// <summary>
    ///   Name
    /// </summary>
    public string Name { get; set; }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
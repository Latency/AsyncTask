// ****************************************************************************
// File:       TaskEvent.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       01/01/2018
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2018
// ****************************************************************************

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ORM_Monitor.Events;

namespace ORM_Monitor {
  using Evt = EventHandler<TaskEventArgs>;


  /// <summary>
  ///   Task_Event
  /// </summary>
  public sealed partial class TaskEvent : TaskEventArgs, IDisposable {
    #region Constructors
    // -----------------------------------------------------------------------

    /// <inheritdoc />
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="duration"></param>
    public TaskEvent(TimeSpan duration) {
      TokenSource = new CancellationTokenSource();
      Duration = duration;
      Status = TaskStatus.Created;
      Name = string.Empty;

      canceledEvent = new CanceledEvent(this);
      completedEvent = new CompletedEvent(this);
      progressChangedEvent = new ProgressChangedEvent(this);
      runningEvent = new RunningEvent(this);
      timedoutEvent = new TimedoutEvent(this);
      exitedEvent = new ExitedEvent(this);
    }


    /// <inheritdoc />
    /// <summary>
    ///   Default Constructor
    /// </summary>
    /// <param name="timeout"></param>
    public TaskEvent(double timeout = -1) : this(TimeSpan.FromMilliseconds(timeout)) { }

    // -----------------------------------------------------------------------
    #endregion Constructors


    #region Backing Fields
    // -----------------------------------------------------------------------

    private Evt _onCanceled;
    private Evt _onCompleted;
    private Evt _onProgressChanged;
    private Evt _onRunning;
    private Evt _onTimedout;
    private Evt _onExited;

    // -----------------------------------------------------------------------
    #endregion Backing Fields


    #region Properties
    // -----------------------------------------------------------------------

    /// <summary>
    ///   OnCanceled
    /// </summary>
    public Evt OnCanceled {
      get => _onCanceled;
      set {
        if (canceledEvent.IsSubscribed)
          canceledEvent.Handler -= _onCanceled;
        _onCanceled = value;
        canceledEvent.Handler += _onCanceled;
      }
    }

    /// <summary>
    ///   OnCompleted
    /// </summary>
    public Evt OnCompleted {
      get => _onCompleted;
      set {
        if (completedEvent.IsSubscribed)
          canceledEvent.Handler -= _onCompleted;
        _onCompleted = value;
        completedEvent.Handler += _onCompleted;
      }
    }

    /// <summary>
    ///   OnProgressChanged
    /// </summary>
    public Evt OnProgressChanged {
      get => _onProgressChanged;
      set {
        if (progressChangedEvent.IsSubscribed)
          progressChangedEvent.Handler -= _onProgressChanged;
        _onProgressChanged = value;
        progressChangedEvent.Handler += _onProgressChanged;
      }
    }

    /// <summary>
    ///   OnRunning
    /// </summary>
    public Evt OnRunning {
      get => _onRunning;
      set {
        if (runningEvent.IsSubscribed)
          runningEvent.Handler -= _onRunning;
        _onRunning = value;
        runningEvent.Handler += _onRunning;
      }
    }

    /// <summary>
    ///   OnTimedout
    /// </summary>
    public Evt OnTimedout {
      get => _onTimedout;
      set {
        if (timedoutEvent.IsSubscribed)
          timedoutEvent.Handler -= _onTimedout;
        _onTimedout = value;
        timedoutEvent.Handler += _onTimedout;
      }
    }

    /// <summary>
    ///   OnExited
    /// </summary>
    public Evt OnExited {
      get => _onExited;
      set {
        if (exitedEvent.IsSubscribed)
          exitedEvent.Handler -= _onExited;
        _onExited = value;
        exitedEvent.Handler += _onExited;
      }
    }

    // -----------------------------------------------------------------------
    #endregion Properties


    #region Event Handlers
    // -----------------------------------------------------------------------
    // ReSharper disable InconsistentNaming

    /// <summary>
    ///   CanceledEvent
    /// </summary>
    private readonly CanceledEvent canceledEvent;

    /// <summary>
    ///   CompletedEvent
    /// </summary>
    private readonly CompletedEvent completedEvent;

    /// <summary>
    ///   ProgressChangedEvent
    /// </summary>
    private readonly ProgressChangedEvent progressChangedEvent;

    /// <summary>
    ///   RunningEvent
    /// </summary>
    private readonly RunningEvent runningEvent;

    /// <summary>
    ///   TimedoutEvent
    /// </summary>
    private readonly TimedoutEvent timedoutEvent;

    /// <summary>
    ///   ExitedEvent
    /// </summary>
    private readonly ExitedEvent exitedEvent;

    // ReSharper restore InconsistentNaming
    // -----------------------------------------------------------------------
    #endregion Event Handlers


    #region Expression Bodies
    // -----------------------------------------------------------------------

    /// <summary>
    ///   IsDisposed
    /// </summary>
    public bool IsDisposed {
      get {
        var pi = TokenSource.GetType().GetProperty("IsDisposed", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
        return pi != null && (bool)pi.GetValue(TokenSource);
      }
    }


    /// <inheritdoc />
    /// <summary>
    ///   Dispose
    /// </summary>
    public void Dispose() {
      Task?.Dispose();
      Task = null;

      TokenSource?.Dispose();
      TokenSource = null;

      canceledEvent?.Dispose();
      completedEvent?.Dispose();
      progressChangedEvent?.Dispose();
      runningEvent?.Dispose();
      timedoutEvent?.Dispose();
      exitedEvent?.Dispose();

      GC.Collect();
    }

    // -----------------------------------------------------------------------
    #endregion Expression Bodies
  }
}
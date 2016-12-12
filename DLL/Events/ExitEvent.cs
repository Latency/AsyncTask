//  *****************************************************************************
//  File:       ExitEvent.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Events {
  /// <summary>
  ///   RunningEvent - Event handler renamed as custom type definition by composition.
  /// </summary>
  public sealed class ExitEvent<T> : TaskHandler<T>, IExitEvent<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="expression"></param>
    public ExitEvent(CancellationTokenSource ctx, TaskEventArgs.Expression expression) : base(ctx, expression) {
    }

    #region Event Invocator

    // -----------------------------------------------------------------------

    /// <summary>
    ///   OnExit
    /// </summary>
    public event EventHandler<TaskEventArgs> OnExit {
      add {
        lock (Mutex) {
          Handler += value;
        }
      }
      remove {
        lock (Mutex) {
          Handler -= value;
        }
      }
    }

    // -----------------------------------------------------------------------

    #endregion Event Invocator

    #region Methods

    // -----------------------------------------------------------------------
    /// <summary>
    ///   Invoke
    /// </summary>
    public void Invoke() {
      Handler?.Invoke(this, new TaskEventArgs(Cts, Expression));
    }

    // -----------------------------------------------------------------------

    #endregion Methods

    #region Properties

    // -----------------------------------------------------------------------

    /// <summary>
    ///   IsSubscribed
    /// </summary>
    public override bool IsSubscribed => Handler?.GetInvocationList().Length > 0;

    // -----------------------------------------------------------------------

    #endregion Properties

    #region Fields

    // -----------------------------------------------------------------------

    /// <summary>
    ///   Handler
    /// </summary>
    protected override event EventHandler<TaskEventArgs> Handler;

    // -----------------------------------------------------------------------

    #endregion Fields
  }
}
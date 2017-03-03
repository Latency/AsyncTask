//  *****************************************************************************
//  File:       ProgressChangedEvent.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/09/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Events {
  /// <summary>
  ///   RunningEvent - Event handler renamed as custom type definition by composition.
  /// </summary>
  public sealed class ProgressChangedEvent<T> : TaskHandler<T>, IProgressChangedEvent<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="taskEvent"></param>
    /// <param name="expression"></param>
    /// <param name="source"></param>
    public ProgressChangedEvent(TaskEvent<T> taskEvent, TaskEventArgs<T>.Expression expression, T source) : base(taskEvent, expression, source) {
    }

    #region Event Invocator
    // -----------------------------------------------------------------------

    /// <summary>
    ///   OnProgressChanged
    /// </summary>
    public event EventHandler<TaskEventArgs<T>> OnProgressChanged {
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
  }
}
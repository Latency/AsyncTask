//  *****************************************************************************
//  File:       CompletedEvent.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Events {
  /// <summary>
  ///   CompletedEvent - Event handler renamed as custom type definition by composition.
  /// </summary>
  public sealed class CompletedEvent<T> : TaskHandler<T>, ICompletedEvent<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="taskEvent"></param>
    /// <param name="expression"></param>
    /// <param name="source"></param>
    public CompletedEvent(TaskEvent<T> taskEvent, TaskEventArgs<T>.Expression expression, T source) : base(taskEvent, expression, source) {
    }

    #region Event Invocator
    // -----------------------------------------------------------------------

    /// <summary>
    ///   OnCompleted
    /// </summary>
    public event EventHandler<TaskEventArgs<T>> OnCompleted {
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
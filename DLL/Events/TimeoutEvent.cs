//  *****************************************************************************
//  File:       TimeoutEvent.cs
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
  ///   TimeoutEvent - Event handler renamed as custom type definition by composition.
  /// </summary>
  public sealed class TimeoutEvent<T> : TaskHandler<T>, ITimeoutEvent<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="taskEvent"></param>
    /// <param name="expression"></param>
    /// <param name="service"></param>
    public TimeoutEvent(TaskEvent<T> taskEvent, TaskEventArgs<T>.Expression expression, object service) : base(taskEvent, expression, service) {
    }

    #region Event Invocator
    // -----------------------------------------------------------------------

    /// <summary>
    ///   OnTimeout
    /// </summary>
    public event EventHandler<TaskEventArgs<T>> OnTimeout {
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
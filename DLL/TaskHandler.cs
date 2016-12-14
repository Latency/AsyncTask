//  *****************************************************************************
//  File:       TaskHandler.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor {
  /// <summary>
  ///   TaskHandler
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class TaskHandler<T> : ITaskHandler {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="taskEvent"></param>
    /// <param name="expression"></param>
    /// <param name="service"></param>
    protected TaskHandler(TaskEvent<T> taskEvent, TaskEventArgs<T>.Expression expression, object service) {
      _taskEvent = taskEvent;
      _expression = expression;
      _service = service;
    }


    // -----------------------------------------------------------------------
    #region Properties

    /// <summary>
    ///   IsSubscribed
    /// </summary>
    public bool IsSubscribed => Handler?.GetInvocationList().Length > 0;

    // -----------------------------------------------------------------------
    #endregion Properties

    #region Methods

    // -----------------------------------------------------------------------
    /// <summary>
    ///   Invoke
    /// </summary>
    public void Invoke() {
      Handler?.Invoke(this, new TaskEventArgs<T>(_taskEvent, _expression, _service));
    }

    // -----------------------------------------------------------------------
    #endregion Methods

    #region Fields
    // -----------------------------------------------------------------------

    private readonly TaskEvent<T> _taskEvent;
    private readonly TaskEventArgs<T>.Expression _expression;
    private readonly object _service; 

    protected event EventHandler<TaskEventArgs<T>> Handler;
    protected readonly object Mutex = new object();

    // -----------------------------------------------------------------------
    #endregion Fields
  }
}
//  *****************************************************************************
//  File:       TaskEventArgs.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;

namespace ORM_Monitor {
  /// <summary>
  ///   TaskEventArgs
  /// </summary>
  public class TaskEventArgs<T> : EventArgs {
    /// <summary>
    ///   Expression
    /// </summary>
    public delegate T Expression(params dynamic[] args);

    #region Fields
    // -----------------------------------------------------------------------

    private readonly Expression _expression;

    // -----------------------------------------------------------------------
    #endregion Fields

    #region Constructor
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Copy Constructor (overload + 1)
    /// </summary>
    /// <param name="taskEvent"></param>
    /// <param name="expression"></param>
    /// <param name="source"></param>
    public TaskEventArgs(TaskEvent<T> taskEvent, Expression expression, T source) {
      Event = taskEvent;
      _expression = expression;
      Source = source;
    }
    
    // -----------------------------------------------------------------------
    #endregion Constructor

    #region Methods
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Expression
    /// </summary>
    public T Invoke(params dynamic[] args) {
      return _expression.Invoke(args);
    }

    // -----------------------------------------------------------------------
    #endregion Methods

    #region Properties
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Event
    /// </summary>
    public TaskEvent<T> Event { get; }

    /// <summary>
    ///   Source
    /// </summary>
    public T Source { get; }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
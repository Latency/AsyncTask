//  *****************************************************************************
//  File:       TaskEventArgs.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading;

namespace ORM_Monitor {
  /// <summary>
  ///   TaskEventArgs
  /// </summary>
  public class TaskEventArgs : EventArgs {
    /// <summary>
    ///   Expression
    /// </summary>
    public delegate void Expression(params dynamic[] args);

    private readonly Expression _expression;
    private readonly CancellationTokenSource _taskStatus;

    /// <summary>
    ///   Copy Constructor (overload + 1)
    /// </summary>
    /// <param name="cts"></param>
    /// <param name="expression"></param>
    public TaskEventArgs(CancellationTokenSource cts, Expression expression) {
      _taskStatus = cts;
      _expression = expression;
    }


    // -----------------------------------------------------------------------

    #region Properties

    /// <summary>
    ///   Token
    /// </summary>
    public CancellationToken? Token => _taskStatus?.Token;


    /// <summary>
    ///   Expression
    /// </summary>
    protected void Invoke(params dynamic[] args) {
      _expression.Invoke(args);
    }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
//  *****************************************************************************
//  File:      TaskEventArgs.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading;

namespace ORM_Monitor.Models {
  /// <summary>
  ///   TaskEventArgs
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class TaskEventArgs<T> : EventArgs {
    private readonly CancellationTokenSource _taskStatus;


    /// <summary>
    ///   Static Constructor
    /// </summary>
    static TaskEventArgs() {
      if (!typeof (T).IsSubclassOf(typeof (Delegate)))
        throw new InvalidOperationException(typeof (T).Name + " is not a delegate type.");
    }


    /// <summary>
    ///   Copy Constructor (overload + 1)
    /// </summary>
    /// <param name="cts"></param>
    /// <param name="expression"></param>
    public TaskEventArgs(CancellationTokenSource cts, T expression) {
      _taskStatus = cts;
      Expression = expression;
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
    public T Expression { get; private set; }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
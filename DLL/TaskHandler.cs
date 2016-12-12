//  *****************************************************************************
//  File:       TaskHandler.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor {
  /// <summary>
  ///   TaskHandler
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class TaskHandler<T> : ITaskHandler<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="cts"></param>
    /// <param name="expression"></param>
    protected TaskHandler(CancellationTokenSource cts, TaskEventArgs.Expression expression) {
      Cts = cts;
      Expression = expression;
    }


    // -----------------------------------------------------------------------
    #region Properties

    /// <summary>
    ///   IsSubscribed
    /// </summary>
    public abstract bool IsSubscribed { get; }

    // -----------------------------------------------------------------------
    #endregion Properties

    #region Fields
    // -----------------------------------------------------------------------

    protected readonly CancellationTokenSource Cts;
    public readonly TaskEventArgs.Expression Expression;

    protected abstract event EventHandler<TaskEventArgs> Handler;
    protected readonly object Mutex = new object();

    // -----------------------------------------------------------------------
    #endregion Fields
  }
}
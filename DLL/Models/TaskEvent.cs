//  *****************************************************************************
//  File:      TaskEvent.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading;
using ORM_Monitor.Controllers;

namespace ORM_Monitor.Models {
  /// <summary>
  ///   TaskEvent
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class TaskEvent<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    public TaskEvent(int timeout = -1) {
      StartTime = DateTime.Now;
      EndTime = StartTime.Add(TimeSpan.FromMilliseconds(timeout));

      TokenSource = new CancellationTokenSource();
      CanceledAction = new CanceledEvent<T>(TokenSource);
      RunningAction = new RunningEvent<T>(TokenSource);
      CompletedAction = new CompletedEvent<T>(TokenSource);
      TimeoutAction = new TimeoutEvent<T>(TokenSource);
    }

    #region Properties

    // -----------------------------------------------------------------------

    /// <summary>
    ///   CanceledAction - Event Handler for CanceledEvent<c>T</c>
    /// </summary>
    public CanceledEvent<T> CanceledAction { get; }

    /// <summary>
    ///   RunningAction - Event Handler for RunningEvent<c>T</c>
    /// </summary>
    public RunningEvent<T> RunningAction { get; }

    /// <summary>
    ///   CompletedAction - Event Handler for CompletedEvent<c>T</c>
    /// </summary>
    public CompletedEvent<T> CompletedAction { get; }

    /// <summary>
    ///   TimeoutAction - Event Handler for TimeoutEvent<c>T</c>
    /// </summary>
    public TimeoutEvent<T> TimeoutAction { get; }

    /// <summary>
    ///   TokenSource
    /// </summary>
    public CancellationTokenSource TokenSource { get; }

    /// <summary>
    ///   StartTime
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    ///   EndTime
    /// </summary>
    public DateTime EndTime { get; }

    /// <summary>
    ///   TimedOut
    /// </summary>
    public bool TimedOut { get; set; }

    /// <summary>
    ///   Name
    /// </summary>
    public string Name { get; set; }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
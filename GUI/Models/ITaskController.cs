//  *****************************************************************************
//  File:       ITaskController.cs
//  Solution:   ORM-Monitor
//  Project:    ~GUI2
//  Date:       02/21/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

namespace ORM_Monitor.Models {
  public interface ITaskController {
    /// <summary>
    ///  Runs the task, returning true for success, false for failure.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="source"></param>
    /// <returns>true or false.</returns>
    TaskEvent<T> Run<T>(TaskEventArgs<T>.Expression expression, T source);
  }
}
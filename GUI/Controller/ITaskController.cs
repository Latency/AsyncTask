//  *****************************************************************************
//  File:       ITaskController.cs
//  Solution:   ORM-Monitor
//  Project:    ~GUI2
//  Date:       02/21/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

namespace ORM_Monitor.Controller {
  public interface ITaskController {
    /// <summary>
    ///  Runs the task, returning true for success, false for failure.
    /// </summary>
    /// <returns>true or false.</returns>
    TaskEvent Run();
  }
}
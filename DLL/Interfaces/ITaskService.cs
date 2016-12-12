//  *****************************************************************************
//  File:       ITaskService.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading.Tasks;

namespace ORM_Monitor.Interfaces {
  /// <summary>
  ///   IServiceTask
  /// </summary>
  public interface ITaskService {
    #region Properties

    // ------------------------------------------------------------------------

    /// <summary>
    ///   Date
    /// </summary>
    DateTime? Date { get; set; }

    /// <summary>
    ///   ImageName
    /// </summary>
    string ImageName { get; set; }

    /// <summary>
    ///   TaskName
    /// </summary>
    string TaskName { get; set; }

    /// <summary>
    ///   Task
    /// </summary>
    Task Task { get; set; }

    /// <summary>
    ///   Event
    /// </summary>
    TaskEvent<dynamic> Event { get; set; }

    /// <summary>
    ///   Description
    /// </summary>
    string Description { get; set; }

    /// <summary>
    ///   Priority
    /// </summary>
    int Priority { get; set; }

    /// <summary>
    ///   Progress
    /// </summary>
    int Progress { get; set; }

    // ------------------------------------------------------------------------

    #endregion Properties
  }
}
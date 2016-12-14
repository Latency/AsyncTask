//  *****************************************************************************
//  File:       ITaskService.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       12/13/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading.Tasks;
using GUI.Views;
using ORM_Monitor;

namespace GUI.Models {
  /// <summary>
  ///   IServiceTask
  /// </summary>
  public interface ITaskService {
    #region Properties

    // ------------------------------------------------------------------------

    /// <summary>
    ///   View
    /// </summary>
    ITaskView View { get; set; }

    /// <summary>
    ///   Index
    /// </summary>
    int Index { get; set; }

    /// <summary>
    ///   Date
    /// </summary>
    DateTime? Date { get; set; }

    /// <summary>
    ///   TaskImageName
    /// </summary>
    string TaskImageName { get; set; }

    /// <summary>
    ///   TaskImageName
    /// </summary>
    string ButtonImageName { get; set; }

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
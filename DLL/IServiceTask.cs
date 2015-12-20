//  *****************************************************************************
//  File:      IServiceTask.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading.Tasks;

namespace ORM_Monitor {
  /// <summary>
  ///   IServiceTask
  /// </summary>
  public interface IServiceTask {
    #region Properties

    // ------------------------------------------------------------------------

    /// <summary>
    ///   Status
    /// </summary>
    TaskStatus Status { get; set; }

    /// <summary>
    ///   Date
    /// </summary>
    DateTime? Date { get; set; }

    /// <summary>
    ///   ImageName
    /// </summary>
    string ImageName { get; set; }

    /// <summary>
    ///   Task
    /// </summary>
    string Task { get; set; }

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
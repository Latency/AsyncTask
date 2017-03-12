//  *****************************************************************************
//  File:       ITaskRecordSet.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       12/13/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ORM_Monitor.Models {
  /// <summary>
  ///   IServiceTask
  /// </summary>
  public interface ITaskRecordSet {
    #region Properties
    // ------------------------------------------------------------------------

    /// <summary>
    ///   TaskName
    /// </summary>
    string TaskName { get; set; }

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
    ushort Progress { get; set; }

    /// <summary>
    ///   Status
    /// </summary>
    TaskStatus Status { get; set; }

    /// <summary>
    ///   Action
    /// </summary>
    Button Action { get; set; }

    /// <summary>
    ///   Date
    /// </summary>
    DateTime? Date { get; set; }

    /// <summary>
    ///   Tag
    /// </summary>
    object Tag { get; set; }
    // ------------------------------------------------------------------------

      #endregion Properties
    }
}
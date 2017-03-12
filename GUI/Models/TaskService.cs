//  *****************************************************************************
//  File:       TaskService.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       12/13/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System.Windows;
using System.Windows.Controls;

namespace ORM_Monitor.Models {
  /// <summary>
  ///   ServiceTask
  /// </summary>
  public class TaskService {
    #region Constructor
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="parent"></param>
    /// <param name="ctrlr"></param>
    /// <param name="dgRow"></param>
    public TaskService(Window owner, ITaskRecordSet parent, ITaskController ctrlr, DataGridRow dgRow) {
      Owner = owner;
      Parent = parent;
      Controller = ctrlr;
      GridRow = dgRow;
    }

    // -----------------------------------------------------------------------
    #endregion Constructor


    #region Properties
    // -----------------------------------------------------------------------
    /// <summary>
    ///   Owner
    /// </summary>
    public Window Owner { get; private set; }

    /// <summary>
    ///   Controller
    /// </summary>
    public ITaskController Controller { get; private set; }

    /// <summary>
    ///   RecordSet
    /// </summary>
    public ITaskRecordSet Parent { get; private set; }

    /// <summary>
    ///   View
    /// </summary>
    public DataGridRow GridRow { get; private set; }

    /// <summary>
    ///   Event
    /// </summary>
    public TaskEvent<dynamic> Event { get; set; }

    // -----------------------------------------------------------------------
    #endregion Properties
  }
}
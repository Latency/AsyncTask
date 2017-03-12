//  *****************************************************************************
//  File:      TaskController.Designer.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using ORM_Monitor.Models;

namespace ORM_Monitor.Controller {
  public sealed partial class TaskController : ITaskController, IDisposable {
    #region Properties
    // -----------------------------------------------------------------------

    /// <summary>
    ///  Concurrent llist container.
    /// </summary>
    public volatile Dictionary<string, TaskEvent<dynamic>> RunningTasks = new Dictionary<string, TaskEvent<dynamic>>();

    // -----------------------------------------------------------------------
    #endregion Properties


    /// <summary>
    ///  Dispose
    /// </summary>
    public void Dispose() {
      foreach (var t in RunningTasks.Where(t => !t.Value.IsDisposed))
        t.Value.TokenSource.Cancel();
    }
  }
}

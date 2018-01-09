// **************************************************************************
// File:      TaskEventArgs.cs
// Solution:  ORM-Monitor
// Project:   ORM-Monitor
// Date:      01/02/2018
// Author:    Latency McLaughlin
// Copywrite: Bio-Hazard Industries - 1998-2018
// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ORM_Monitor {
  /// <inheritdoc />
  /// <summary>
  ///   TaskEventArgs
  /// </summary>
  public abstract class TaskEventArgs : EventArgs {
    #region Properties
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Task
    /// </summary>
    public Task Task { get; protected set; }

    /// <summary>
    ///   TokenSource
    /// </summary>
    public CancellationTokenSource TokenSource { get; protected set; }

    /// <summary>
    ///   Duration
    /// </summary>
    public TimeSpan Duration { get; protected set; }

    /// <summary>
    ///   Status
    /// </summary>
    public TaskEventStatus Status { get; protected set; }

    /// <summary>
    ///   Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Expression
    /// </summary>
    public List<Delegate> Expression { get; set; }

    /// <summary>
    ///   Tag
    /// </summary>
    public object Tag { get; set; }

    // -----------------------------------------------------------------------
      #endregion Properties
    }
}
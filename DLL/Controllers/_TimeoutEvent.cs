//  *****************************************************************************
//  File:      _TimeoutEvent.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System.Threading;
using ORM_Monitor.Models;

namespace ORM_Monitor.Controllers {
  /// <summary>
  ///   TimeoutEvent - Event handler renamed as custom type definition by composition.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class TimeoutEvent<T> : TaskHandler<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="ctx"></param>
    public TimeoutEvent(CancellationTokenSource ctx) : base(ctx) {}


    /// <summary>
    ///   OnTimeout
    /// </summary>
    public event TaskEventHandler OnTimeout {
      add { MyHandler += value; }
      remove { MyHandler -= value; }
    }
  }
}
//  *****************************************************************************
//  File:      _CompletedEvent.cs
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
  ///   CompletedEvent - Event handler renamed as custom type definition by composition.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class CompletedEvent<T> : TaskHandler<T> {
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="ctx"></param>
    public CompletedEvent(CancellationTokenSource ctx) : base(ctx) {}


    /// <summary>
    ///   OnCompleted
    /// </summary>
    public event TaskEventHandler OnCompleted {
      add { MyHandler += value; }
      remove { MyHandler -= value; }
    }
  }
}
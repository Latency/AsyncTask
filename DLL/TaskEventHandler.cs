//  *****************************************************************************
//  File:       TaskEventHandler.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Runtime.CompilerServices;

namespace ORM_Monitor {
  using Evt = EventHandler<TaskEventArgs>;


  /// <summary>
  ///   TaskEventHandler
  /// </summary>
  public abstract class TaskEventHandler : IDisposable {
    #region Constructor
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="taskEvent"></param>
    protected TaskEventHandler(TaskEvent taskEvent) {
      TaskEvent = taskEvent;
    }

    // -----------------------------------------------------------------------
    #endregion Constructor


    #region Fields
    // -----------------------------------------------------------------------

    /// <summary>
    ///   TaskEvent
    /// </summary>
    protected readonly TaskEvent TaskEvent;

    /// <summary>
    ///   Backing Field for Handler
    /// </summary>
    private Evt _myEvent;

    /// <summary>
    ///   Handler
    /// </summary>
    protected internal event Evt Handler {
      [MethodImpl(MethodImplOptions.Synchronized)]
      add {
        _myEvent = Delegate.Combine(_myEvent, value) as Evt;
      }
      [MethodImpl(MethodImplOptions.Synchronized)]
      remove {
        _myEvent = Delegate.Remove(_myEvent, value) as Evt;
      }
    }

    // -----------------------------------------------------------------------
    #endregion Fields


    #region Properties
    // -----------------------------------------------------------------------

    /// <summary>
    ///   IsSubscribed
    /// </summary>
    public bool IsSubscribed => _myEvent?.GetInvocationList().Length > 0;

    // -----------------------------------------------------------------------
    #endregion Properties


    #region Methods
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Invoke
    /// </summary>
    /// <param name="tea"></param>
    public abstract void Invoke(TaskEventArgs tea);

    /// <inheritdoc />
    /// <summary>
    ///   Dispose
    /// </summary>
    public void Dispose() {
      if (_myEvent == null)
        return;

      foreach (var d in _myEvent.GetInvocationList())
        // ReSharper disable once DelegateSubtraction
        Handler -= d as Evt;
    }

    // -----------------------------------------------------------------------
    #endregion Methods
  }
}
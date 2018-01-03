//  *****************************************************************************
//  File:       CanceledEvent.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       1/1/2018
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

namespace ORM_Monitor.Events {
  /// <inheritdoc />
  /// <summary>
  ///   CanceledEvent - Handler handler renamed as custom type definition by composition.
  /// </summary>
  public class CanceledEvent : TaskEventHandler {
    #region Constructor
    // -----------------------------------------------------------------------

    /// <inheritdoc />
    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="taskEvent"></param>
    public CanceledEvent(TaskEvent taskEvent) : base(taskEvent) { }

    // -----------------------------------------------------------------------
    #endregion Constructor


    #region Handler Method
    // -----------------------------------------------------------------------

    /// <inheritdoc />
    /// <summary>
    ///   OnCanceledEvent
    /// </summary>
    /// <param name="tea"></param>
    public override void Invoke(TaskEventArgs tea) => TaskEvent.OnCanceled?.Invoke(this, tea);

    // -----------------------------------------------------------------------
    #endregion Handler Method
  }
}
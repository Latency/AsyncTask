//  *****************************************************************************
//  File:      ITaskScheduler.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System.ComponentModel;

namespace ORM_Monitor {
  /// <summary>
  ///   ITaskScheduler
  /// </summary>
  public interface ITaskScheduler {
    /// <summary>
    ///   DoWork
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void DoWork(object sender, DoWorkEventArgs args);

    /// <summary>
    ///   RunWorkerCompleted
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args);

    /// <summary>
    ///   ProgressChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void ProgressChanged(object sender, ProgressChangedEventArgs args);
  }
}
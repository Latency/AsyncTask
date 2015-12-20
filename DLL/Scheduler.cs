//  *****************************************************************************
//  File:      Scheduler.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ReflectSoftware.Insight.Common;

namespace ORM_Monitor {
  /// <summary>
  ///   Scheduler
  /// </summary>
  public class Scheduler {
    private readonly List<BackgroundWorker> _normalTasks;


    /// <summary>
    ///   Default constructor
    /// </summary>
    public Scheduler() {
      _normalTasks = new List<BackgroundWorker>();
    }


    /// <summary>
    ///   RunTask
    /// </summary>
    /// <param name="t"></param>
    public void RunTask(ITaskScheduler t) {
      var task = t as BackgroundWorker;
      if (task == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("task"));

      task.WorkerReportsProgress = true;
      task.WorkerSupportsCancellation = true;

      task.Disposed += (sender, args) => {
        var sendingWorker = sender as BackgroundWorker;
        if (sendingWorker == null)
          return;

        _normalTasks.Remove(sendingWorker);
      };

      task.DoWork += (sender, args) => {
        var sendingWorker = sender as BackgroundWorker; // Capture the BackgroundWorker that fired the event
        if (sendingWorker == null)
          return;

        var t1 = args.Argument as ITaskScheduler;
        if (t1 != null && !sendingWorker.CancellationPending) {
          try {
            t1.DoWork(sendingWorker, args);
          }
          catch (Exception ex) {
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, ex);
          }
        }
        else {
          args.Cancel = true; // If a cancellation request is pending, assign this flag a value of true
          sendingWorker.Dispose();
        }
      };

      task.RunWorkerCompleted += (sender, args) => {
        var sendingWorker = sender as BackgroundWorker; // Capture the BackgroundWorker that fired the event
        if (sendingWorker == null)
          return;

        try {
          t.RunWorkerCompleted(sendingWorker, args);
        }
        catch (Exception ex) {
          throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, ex);
        }
        sendingWorker.Dispose();
      };

      task.ProgressChanged += (sender, args) => {
        var sendingWorker = sender as BackgroundWorker; // Capture the BackgroundWorker that fired the event
        if (sendingWorker == null)
          return;

        if (sendingWorker.WorkerReportsProgress) {
          try {
            t.ProgressChanged(sendingWorker,
              new ProgressChangedEventArgs(args.ProgressPercentage,
                sendingWorker.CancellationPending ? TaskStatus.Canceled : TaskStatus.Running));
          }
          catch (Exception ex) {
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, ex);
          }
        }
      };

      _normalTasks.Add(task);

      task.RunWorkerAsync(task);
    }


    /// <summary>
    ///   Cancel
    /// </summary>
    public bool Cancel() {
      var success = false;
      foreach (var t in _normalTasks.Where(b => b.IsBusy && !b.CancellationPending)) {
        t.CancelAsync();
        success = true;
      }
      return success;
    }
  }
}
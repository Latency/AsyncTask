//  *****************************************************************************
//  File:      V_StatusBar.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BrightIdeasSoftware;
using ORM_Monitor;
using ORM_Monitor.Models;
using ReflectSoftware.Insight.Common;

namespace GUI.Views {
  // ReSharper disable once InconsistentNaming
  internal sealed class V_StatusBar : BackgroundWorker, ITaskScheduler {
    /// <summary>
    ///   Default constructor
    /// </summary>
    /// <param name="olv"></param>
    /// <param name="tag"></param>
    public V_StatusBar(ObjectListView olv, object tag = null) {
      Controller = olv;
      Tag = tag;
    }

    #region Override Methods

    // -----------------------------------------------------------------------

    /// <summary>
    ///   OnDoWork
    /// </summary>
    void ITaskScheduler.DoWork(object sender, DoWorkEventArgs e) {
      //var task = sender as T_StatusBar;
      //var st = task?.Tag as ServiceTask;
      //if (st == null)
      //  throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("st"));

      // TODO
    }


    /// <summary>
    ///   OnRunWorkerCompleted
    /// </summary>
    void ITaskScheduler.RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
      //var task = sender as T_StatusBar;
      //var st = task?.Tag as ServiceTask;
      //if (st == null)
      //  throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("st"));

      // TODO
    }


    /// <summary>
    ///   FetchElement
    /// </summary>
    /// <param name="serviceTask"></param>
    /// <returns></returns>
    private static dynamic FetchElement(ServiceTask serviceTask) {
      var task = serviceTask.View as V_StatusBar;
      if (task == null)
        throw new NullReferenceException(MethodBase.GetCurrentMethod().Name);
      var olv = task.Controller;
      return olv.Items != null && olv.Items.Count > 0 && serviceTask.Index < olv.Items.Count
        ? olv.Items[serviceTask.Index]
        : null;
    }


    /// <summary>
    ///   OnProgressChanged
    /// </summary>
    void ITaskScheduler.ProgressChanged(object sender, ProgressChangedEventArgs e) {
      var task = sender as V_StatusBar;
      var st = task?.Tag as ServiceTask;
      if (st == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("item"));

      st.Progress = e.ProgressPercentage;

      var item = FetchElement(st);
      if (item != null) {
        item.SubItems[2].Text = $"{st.Progress}%";
        // item.SubItems[3].Text = st.Status.ToString();
      }

      // TODO
    }


    /// <summary>
    ///   OnDoWork
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDoWork(DoWorkEventArgs e) {
      base.OnDoWork(e);

      var task = e.Argument as V_StatusBar;
      var st = task?.Tag as ServiceTask;
      var targetTime = DateTime.Now.AddSeconds(3);
      var currentTime = DateTime.Now;
      var diffTime = targetTime.Subtract(currentTime).TotalMilliseconds;
      double current;
      while ((current = targetTime.Subtract(currentTime).TotalMilliseconds) > 0) {
        if (CancellationPending) {
          e.Cancel = true;
          if (st != null)
            st.Status = TaskStatus.Canceled;
          return;
        }

        Thread.Sleep(250);
        var val = 1 - current/diffTime;
        if (WorkerReportsProgress)
          ReportProgress((int) (val*100), TaskStatus.Running);
        currentTime = DateTime.Now;
      }

      if (st != null)
        st.Status = TaskStatus.RanToCompletion;
      if (WorkerReportsProgress)
        ReportProgress(100, TaskStatus.RanToCompletion);
    }


    /// <summary>
    ///   Dispose
    /// </summary>
    public new void Dispose() {
      base.Dispose();
      Tag = null;
    }

    // -----------------------------------------------------------------------

    #endregion Override Methods

    #region Properties

    // -----------------------------------------------------------------------

    public ObjectListView Controller { get; }
    public object Tag { get; set; }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
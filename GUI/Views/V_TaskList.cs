//  *****************************************************************************
//  File:      V_TaskList.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using GUI.Models;
using GUI.Properties;
using GUI.WinForms;
using ORM_Monitor;
using ORM_Monitor.Events;
using ReflectSoftware.Insight;
using ReflectSoftware.Insight.Common;

namespace GUI.Views {
  // ReSharper disable once InconsistentNaming
  internal sealed partial class V_TaskList : ITaskView {
    /// <summary>
    ///   Default constructor
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="olv"></param>
    /// <param name="ts"></param>
    public V_TaskList(RadForm1 ctx, ObjectListView olv, ITaskService ts = null) {
      Context = ctx;
      Controller = olv;
      Service = ts;
    }

    #region Override Methods

    // -----------------------------------------------------------------------

    /// <summary>
    ///   FetchElement
    /// </summary>
    /// <param name="serviceTask"></param>
    /// <returns></returns>
    private ListViewItem FetchElement(ITaskService serviceTask) {
      var task = serviceTask.View as V_TaskList;
      if (task == null)
        throw new NullReferenceException(MethodBase.GetCurrentMethod().Name);

      Func<ObjectListView, ITaskService, dynamic> action = (o, s) => o.Items != null && o.Items.Count > 0 && s.Index < o.Items.Count ? o.Items[s.Index] : null;

      var olv = task.Controller;
      return olv.InvokeRequired ? Context.Invoke(action, olv, serviceTask) as ListViewItem : action.Invoke(olv, serviceTask);
    }


    /// <summary>
    ///  Runs the task, returning true for success, false for failure.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>true or false.</returns>
    public TaskEvent<T> Run<T>(TaskEventArgs<T>.Expression expression) {
      var @t = new TaskEvent<T>(expression, Service, TimeSpan.FromSeconds(Settings.Default.TTL)) {
        Name = GetType().Name.Substring(2)
      };

      Action<ListViewItem, TaskService> action = (viewItem, service) => {
        viewItem.SubItems[2].Text = $"{service.Progress}%";
        viewItem.SubItems[3].Text = service.Task.Status.ToString();
      };

      // ---------------------------------
      @t.OnRunning(
        (obj, tea) => {
          var th = obj as RunningEvent<T>;
          var ts = tea.Service as TaskService;
          if (th == null || ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(@t.OnRunning)));

          double current;
          var targetTime = DateTime.Now.AddSeconds(3);
          var currentTime = DateTime.Now;
          var diffTime = targetTime.Subtract(currentTime).TotalMilliseconds;
          var item = FetchElement(ts);

          while ((current = targetTime.Subtract(currentTime).TotalMilliseconds) > 0) {
            if (tea.Event.TokenSource == null || tea.Event.TokenSource.Token.IsCancellationRequested)
              return;
            
            var val = 1 - current / diffTime;
            ts.Progress = (int)(val * 100);

            if (item != null && Controller.InvokeRequired)
              Controller.Invoke(action, item, ts);

            // Pulse 10x per second.
            Task.Delay(100).Wait();

            currentTime = DateTime.Now;
          }

          
          ts.Progress = 100;
          Controller.Invoke(action, item, ts);
        }
      );

      // ---------------------------------
      @t.OnCompleted(
        (obj, tea) => {
          var ts = tea.Service as TaskService;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(@t.OnCompleted)));
          var item = FetchElement(ts);

          ts.Progress = 100;

          if (item != null && Controller.InvokeRequired)
            Controller.Invoke(action, item, ts);
        }
      );

      // ---------------------------------
      @t.OnTimeout(
        (obj, tea) => {
          var ts = tea.Service as TaskService;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(@t.OnTimeout)));
          var item = FetchElement(ts);

          if (item != null && Controller.InvokeRequired)
            Controller.Invoke(action, item, ts);
        }
      );

      // ---------------------------------
      @t.OnProgressChanged(
        (obj, tea) => {
          // TODO
        }
      );

      // ---------------------------------
      @t.OnExit(
        (obj, tea) => {
          RILogManager.Default.SendInformation($"Disposing {GetType().Name} token source.");
          var ts = tea.Service as TaskService;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(@t.OnExit)));

          var item = FetchElement(ts);
          if (item != null && Controller.InvokeRequired)
            Controller.Invoke(action, item, ts);

          Context.RunningTasks.Remove(ts.TaskName);
          
          t.TokenSource.Dispose();
        }
      );

      // ---------------------------------
      @t.OnCanceled(
        (obj, tea) => {
          var ts = tea.Service as TaskService;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(@t.OnCanceled)));

          var item = FetchElement(ts);
          if (item != null && Controller.InvokeRequired) {
            Controller.Invoke(action, item, ts);
          }
        }
      );

      return @t;
    }


    // -----------------------------------------------------------------------

    #endregion Override Methods
  }
}
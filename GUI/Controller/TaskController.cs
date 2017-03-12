//  *****************************************************************************
//  File:       TaskController.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       02/22/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Reflection;
using System.Threading.Tasks;
using ORM_Monitor.Events;
using ORM_Monitor.Models;
using ORM_Monitor.Properties;
using ReflectSoftware.Insight;
using ReflectSoftware.Insight.Common;

namespace ORM_Monitor.Controller {
  public sealed partial class TaskController {
    #region Override Methods
    // -----------------------------------------------------------------------

    /// <summary>
    ///  Runs the task, returning true for success, false for failure.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="source"></param>
    /// <returns>true or false.</returns>
    TaskEvent<T> ITaskController.Run<T>(TaskEventArgs<T>.Expression expression, T source) {
      var @t = new TaskEvent<T>(expression, source, TimeSpan.FromSeconds(Settings.Default.TTL)) {
        Name = GetType().Name.Substring(2)
      };

      // ---------------------------------
      @t.OnRunning(
        (obj, tea) => {
          var th = obj as RunningEvent<dynamic>;
          var ts = tea.Source as TaskRecordSet;
          if (th == null || ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnRunning"));

          double current;
          var targetTime = DateTime.Now.AddSeconds(3);
          var currentTime = DateTime.Now;
          var diffTime = targetTime.Subtract(currentTime).TotalMilliseconds;
          var service = ts.Tag as TaskService;

          service?.Owner.Dispatcher.Invoke(() => {
            ts.Action.Content = "Stop";
            ts.Status = TaskStatus.Running;
          });

          while ((current = targetTime.Subtract(currentTime).TotalMilliseconds) > 0) {
            if (tea.Event.TokenSource == null || tea.Event.TokenSource.Token.IsCancellationRequested)
              return;

            var val = 1 - current / diffTime;

            service?.Owner.Dispatcher.Invoke(() => {
              ts.Progress = (ushort) (val*100);
            });

            // Pulse 10x per second.
            Task.Delay(100);

            currentTime = DateTime.Now;
          }

          service?.Owner.Dispatcher.Invoke(() => {
            ts.Progress = 100;
          });
        }
      );

      // ---------------------------------
      @t.OnCompleted(
        (obj, tea) => {
          var ts = tea.Source as TaskRecordSet;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnCompleted"));

          var service = ts.Tag as TaskService;
          service?.Owner.Dispatcher.Invoke(() => {
            ts.Action.Content = "Remove";
            ts.Progress = 100;
            ts.Status = TaskStatus.RanToCompletion;
          });
        }
      );

      // ---------------------------------
      @t.OnTimeout(
        (obj, tea) => {
          var ts = tea.Source as TaskRecordSet;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnTimeout"));

          var service = ts.Tag as TaskService;
          service?.Owner.Dispatcher.Invoke(() => {
            ts.Action.IsEnabled = true;
            ts.Action.Content = "Remove";
            ts.Status = TaskStatus.Faulted;
          });
        }
      );

      // ---------------------------------
      @t.OnProgressChanged(
        (obj, tea) => {
          var ts = tea.Source as TaskRecordSet;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnProgressChanged"));
        }
      );

      // ---------------------------------
      @t.OnExit(
        (obj, tea) => {
          var ts = tea.Source as TaskRecordSet;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnExit"));

          RunningTasks.Remove(ts.TaskName);

          RILogManager.Default.SendInformation($"Disposing {GetType().Name} token source.");
          t.TokenSource.Dispose();
        }
      );

      // ---------------------------------
      @t.OnCanceled(
        (obj, tea) => {
          var ts = tea.Source as TaskRecordSet;
          if (ts == null)
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnCanceled"));

          var service = ts.Tag as TaskService;
          service?.Owner.Dispatcher.Invoke(() => {
            ts.Status = TaskStatus.Canceled;
            ts.Action.IsEnabled = true;
            ts.Action.Content = "Remove";
          });
        }
      );

      return @t;
    }


    // -----------------------------------------------------------------------
    #endregion Override Methods
  }
}
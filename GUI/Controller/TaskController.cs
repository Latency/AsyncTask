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
using ORM_Monitor.Models;
using ORM_Monitor.Properties;
using ReflectSoftware.Insight;
using ReflectSoftware.Insight.Common;

namespace ORM_Monitor.Controller {
  public sealed partial class TaskController {
    /// <inheritdoc />
    /// <summary>
    ///  Runs the task, returning true for success, false for failure.
    /// </summary>
    /// <returns>true or false.</returns>
    TaskEvent ITaskController.Run() {
      var t = new TaskEvent(TimeSpan.FromSeconds(Settings.Default.TTL)) {
        Name = GetType().Name.Substring(2),
        OnRunning = (obj, tea) => {
          if (!(tea.Tag is TaskRecordSet ts))
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
            if (tea.TokenSource == null || tea.TokenSource.Token.IsCancellationRequested)
              return;

            var val = 1 - current / diffTime;

            service?.Owner.Dispatcher.Invoke(() => {
              ts.Progress = (ushort)(val * 100);
            });

            // Pulse 10x per second.
            Task.Delay(100);

            currentTime = DateTime.Now;
          }

          service?.Owner.Dispatcher.Invoke(() => {
            ts.Progress = 100;
          });
        },
        OnCompleted = (obj, tea) => {
          if (!(tea.Tag is TaskRecordSet ts))
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnCompleted"));

          var service = ts.Tag as TaskService;
          service?.Owner.Dispatcher.Invoke(() => {
            ts.Action.Content = "Remove";
            ts.Progress = 100;
            ts.Status = TaskStatus.RanToCompletion;
          });
        },
        OnTimedout = (obj, tea) => {
          if (!(tea.Tag is TaskRecordSet ts))
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnTimedout"));

          var service = ts.Tag as TaskService;
          service?.Owner.Dispatcher.Invoke(() => {
            ts.Action.IsEnabled = true;
            ts.Action.Content = "Remove";
            ts.Status = TaskStatus.Faulted;
          });
        },
        OnProgressChanged = (obj, tea) => {
          if (!(tea.Tag is TaskRecordSet))
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnProgressChanged"));
        },
        OnExited = (obj, tea) => {
          if (!(tea.Tag is TaskRecordSet ts))
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnExited"));

          RunningTasks.Remove(ts.TaskName);

          RILogManager.Default.SendInformation($"Disposing {GetType().Name} token source.");
        },
        OnCanceled = (obj, tea) => {
          if (!(tea.Tag is TaskRecordSet ts))
            throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("OnCanceled"));

          var service = ts.Tag as TaskService;
          service?.Owner.Dispatcher.Invoke(() => {
            ts.Status = TaskStatus.Canceled;
            ts.Action.IsEnabled = true;
            ts.Action.Content = "Remove";
          });
        }
      };

      return t;
    }
  }
}
// ****************************************************************************
// File:       AsyncMonitor.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       01/01/2018
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2018
// ****************************************************************************

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ORM_Monitor {
  /// <inheritdoc cref="TaskEventArgs" />
  /// <summary>
  ///   Task_Event
  /// </summary>
  public sealed partial class TaskEvent {
    #region Methods
    // -----------------------------------------------------------------------

    /// <summary>
    ///   Asyncronous delegate monitor.
    /// </summary>
    /// <returns>Task</returns>
    public Task AsyncMonitor() {
      var name = MethodBase.GetCurrentMethod().Name;

      Task = Task.Run(() => {
        Thread.CurrentThread.Name = name;

        // Awaits for blocking main actions.
        try {
          var thread = new Thread(() => {
            // ReSharper disable once InvertIf
            if (runningEvent != null && runningEvent.IsSubscribed) {
              try {
                Status = TaskEventStatus.Running;
                runningEvent.Invoke(this);
                if (completedEvent != null && completedEvent.IsSubscribed) {
                  Status = TaskEventStatus.RanToCompletion;
                  completedEvent.Invoke(this);
                }
              } catch (ThreadAbortException) {
                Status = TaskEventStatus.Aborted;
                Thread.ResetAbort();
              } catch (OperationCanceledException) {
                Status = TaskEventStatus.Canceled;
              } catch (Exception ex) {
                Status = TaskEventStatus.Faulted;
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(AggregateException)) {
                  if (ex.InnerException.InnerException != null && ex.InnerException.InnerException.GetType() == typeof(OperationCanceledException))
                    return;
                }
                throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
              }
            }
          }) {
            Name = $"{name}->Worker",
            IsBackground = true
          };

          thread.SetApartmentState(ApartmentState.STA);
          thread.Start();

          var endTime = DateTime.Now.Add(Duration);

          // Poll for cancellation request or timeout.
          while (thread.IsAlive) {
            if (TokenSource.IsCancellationRequested) {
              thread.Abort(TaskStatus.Canceled);
              if (canceledEvent != null && canceledEvent.IsSubscribed) {
                Status = TaskEventStatus.Canceled;
                canceledEvent.Invoke(this);
              }
              break;
            }
            if (Duration >= TimeSpan.Zero && DateTime.Now >= endTime) {
              thread.Abort(TaskStatus.Faulted);
              if (timedoutEvent != null && timedoutEvent.IsSubscribed) {
                Status = TaskEventStatus.TimedOut;
                timedoutEvent.Invoke(this);
              }
              break;
            }

            progressChangedEvent.Invoke(this);

            // Pulse 1/2 sec with a logical delay without blocking the current thread.
            Task.Delay(500, TokenSource.Token);
          }

          thread.Join();
        } catch (Exception ex) {
          throw new Exception(MethodBase.GetCurrentMethod().Name, ex);
        }

        Task.GetAwaiter().OnCompleted(() => {
          exitedEvent.Invoke(this);
          Dispose();
        });

      }, TokenSource.Token);

      return Task;
    }

    // -----------------------------------------------------------------------
    #endregion Methods
  }
}
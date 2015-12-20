//  *****************************************************************************
//  File:      Async-IO.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ORM_Monitor.Models;
using ReflectSoftware.Insight;

namespace ORM_Monitor.Controllers {
  // ReSharper disable once InconsistentNaming
  /// <summary>
  ///   Asynchronous Input/Output
  /// </summary>
  public static class Async_IO {
    /// <summary>
    ///   RunningAction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="te"></param>
    /// <param name="expression"></param>
    private static void MainAction<T>(this TaskEvent<T> te, T expression) {
      try {
        te.RunningAction.Invoke(expression);
      }
      catch (ThreadAbortException) {}
      catch (Exception e) {
        RILogManager.Default.SendException(MethodBase.GetCurrentMethod().Name, e);
        Environment.Exit(0);
      }
    }


    /// <summary>
    ///   CompletedAction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="te"></param>
    /// <param name="expression"></param>
    private static void CompletedAction<T>(this TaskEvent<T> te, T expression) {
      if (te.CompletedAction.IsSubscribed) {
        try {
          if (te.TokenSource.Token.IsCancellationRequested)
            te.CanceledAction.Invoke(expression);
          else if (!te.TimedOut)
            te.CompletedAction.Invoke(expression);
        }
        catch (Exception e) {
          RILogManager.Default.SendException(MethodBase.GetCurrentMethod().Name, e);
          throw;
        }
      }
    }


    /// <summary>
    ///   CanceledAction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="te"></param>
    /// <param name="expression"></param>
    private static void CanceledAction<T>(this TaskEvent<T> te, T expression) {
      if (te.CanceledAction.IsSubscribed) {
        try {
          te.CanceledAction.Invoke(expression);
        }
        catch (Exception e) {
          RILogManager.Default.SendException(MethodBase.GetCurrentMethod().Name, e);
          throw;
        }
      }
    }


    /// <summary>
    ///   TimeoutAction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="te"></param>
    /// <param name="expression"></param>
    private static void TimeoutAction<T>(this TaskEvent<T> te, T expression) {
      if (te.TimeoutAction.IsSubscribed) {
        try {
          te.TimeoutAction.Invoke(expression);
        }
        catch (Exception e) {
          RILogManager.Default.SendException(MethodBase.GetCurrentMethod().Name, e);
          throw;
        }
      }
    }


    /// <summary>
    ///   Asyncronous delegate monitor.
    ///   AsyncMonitor (+3 Overload)
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="t"></param>
    /// <returns>Task</returns>
    public static async Task AsyncMonitor<T>(this T expression, TaskEvent<T> t) where T : class {
      var asyncTask = Task.Factory.StartNew(() => {
        var a = new Thread(() => {
          try {
            t.MainAction(expression);
          }
          catch (ThreadAbortException) {}
        });
        a.Start();
        while (a.IsAlive) {
          t.TokenSource.Token.ThrowIfCancellationRequested();

          if (DateTime.Now >= t.EndTime) {
            t.TimedOut = true;
            t.TimeoutAction(expression);
            a.Abort();
            break;
          }
          Thread.Sleep(250);
        }

        a.Join();
      }, t.TokenSource.Token, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Current);

      await asyncTask.ContinueWith(task => {
        // Check task status.
        switch (task.Status) {
          case TaskStatus.RanToCompletion:
            t.CompletedAction(expression);
            break;
          case TaskStatus.Faulted:
            if (task.Exception != null)
              throw task.Exception;
            break;
          case TaskStatus.Canceled:
            t.CanceledAction(expression);
            break;
        }
      });
    }
  }
}
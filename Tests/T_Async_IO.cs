//  *****************************************************************************
//  File:      T_Async_IO.cs
//  Solution:  ORM-Monitor
//  Project:   Tests
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ORM_Monitor.Controllers;
using ORM_Monitor.Models;
using MyType = System.Action<object, string>;
using Timer = System.Timers.Timer;

namespace Tests {
  [TestFixture]
  // ReSharper disable once InconsistentNaming
  public class T_Async_IO {
    /// <summary>
    ///   SetUp
    /// </summary>
    [SetUp]
    public void Setup() {
      _aTimer = new Timer {
        Interval = 1000,
        Enabled = true
      };

      _aTimer.Elapsed += (sender, args) => {
        _tick++;
        Debug.WriteLine("Tick #{0}", _tick);
      };
    }


    /// <summary>
    ///   TearDown
    /// </summary>
    [TearDown]
    public void TearDown() {
      _aTimer.Stop();
    }

    private Timer _aTimer;
    private int _tick;


    /// <summary>
    ///   Test
    /// </summary>
    [Test]
    public void Monitor_Test() {
      // Construct started tasks
      var tasks = new Task[3];
      // ReSharper disable once InconsistentNaming
      const int TTL = 5000;
      const string completed = "Completed successfully.";
      const string timeout = "Time out failure!";
      Func<int?, string> canceled = id => $"Task ID({id}) has canceled.";

      try {
        MyType action = (obj, str) => {
          var testNo = obj != null ? $"{((TaskEvent<MyType>) obj).Name}: " : string.Empty;
          Debug.WriteLine(testNo + str);
        };

        // #############################################################
        Debug.WriteLine("Processing **** NORMAL    **** test");
        //

        var @t0 = new TaskEvent<MyType>(TTL) {
          Name = "t0"
        };

        @t0.RunningAction.OnRunning += (th, tea) => { SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 2)); };

        @t0.CompletedAction.OnCompleted += (th, tea) => { tea.Expression(@t0, completed); };

        @t0.TimeoutAction.OnTimeout += (th, tea) => {
          tea.Expression(@t0, timeout);
          Assert.Fail("timeout @t0");
        };

        @t0.CanceledAction.OnCanceled += (th, tea) => {
          tea.Expression(@t0, canceled(Task.CurrentId));
          Assert.Fail("canceled @t0");
        };

        tasks[0] = action.AsyncMonitor(@t0);

        // #############################################################
        Debug.WriteLine("Processing **** CANCELED  **** test");
        //

        var @t1 = new TaskEvent<MyType>(TTL) {
          Name = "t1"
        };

        @t1.RunningAction.OnRunning += (th, tea) => {
          // Poll longer than Timeout threshold
          SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10));
        };

        @t1.CompletedAction.OnCompleted += (th, tea) => {
          tea.Expression(@t1, completed);
          Assert.Fail("completed @t1");
        };

        @t1.TimeoutAction.OnTimeout += (th, tea) => {
          tea.Expression(@t1, timeout);
          Assert.Fail("timeout @t1");
        };

        @t1.CanceledAction.OnCanceled += (th, tea) => { tea.Expression(@t1, canceled(Task.CurrentId)); };

        var foo = new Action<object>(task => {
          var te = task as TaskEvent<MyType>;
          if (te == null)
            throw new NullReferenceException(nameof(te));

          Thread.Sleep(3000);
          te.TokenSource.Cancel();
        });

        new Thread(() => foo(@t1)).Start();

        tasks[1] = action.AsyncMonitor(@t1);

        // #############################################################
        Debug.WriteLine("Processing **** TIMED OUT **** test");
        //

        var @t2 = new TaskEvent<MyType>(TTL) {
          Name = "t2"
        };

        @t2.RunningAction.OnRunning += (th, tea) => {
          // Poll longer than Timeout threshold
          SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 6));
        };

        @t2.CompletedAction.OnCompleted += (th, tea) => {
          tea.Expression(@t2, completed);
          Assert.Fail("completed @t2");
        };

        @t2.TimeoutAction.OnTimeout += (th, tea) => { tea.Expression(@t2, timeout); };

        @t2.CanceledAction.OnCanceled += (th, tea) => {
          tea.Expression(@t2, canceled(Task.CurrentId));
          Assert.Fail("canceled @t2");
        };

        tasks[2] = action.AsyncMonitor(@t2);

        // Wait for all the tasks to finish.
        Task.WaitAll(tasks);
        Debug.WriteLine("All Tasks Completed");
      }
      catch (OperationCanceledException) {}
      catch (SuccessException) {
        // Records a message in the test result.
      }
      catch (Exception ex) {
        _aTimer.Stop();
        Debug.Print(ex.Message);
        Environment.Exit(1);
      }

      // Run test for 10 seconds to ensure all threads completed.
      while (_tick < 10) {}
    }
  }
}
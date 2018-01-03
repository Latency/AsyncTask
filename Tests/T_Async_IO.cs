// ****************************************************************************
// File:       T_Async_IO.cs
// Solution:   ORM-Monitor
// Project:    Tests
// Date:       01/01/2018
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2018
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ORM_Monitor;
using Tests.Properties;
using Timer = System.Timers.Timer;

namespace Tests {
  [TestFixture]
  // ReSharper disable once InconsistentNaming
  public class T_Async_IO {
    private Timer _aTimer;
    private int _tick;


    /// <summary>
    ///   SetUp
    /// </summary>
    [SetUp]
    public void Setup() {
      _aTimer = new Timer {Interval = 1000, Enabled = true};
      _aTimer.Elapsed += (sender, args) => { Debug.WriteLine("Tick #{0}", ++_tick); };
    }


    /// <summary>
    ///   TearDown
    /// </summary>
    [TearDown]
    public void TearDown() {
      _aTimer.Stop();
    }


    /// <summary>
    ///   Expr
    /// </summary>
    /// <param name="args"></param>
    private delegate void Expr(params string[] args);


    /// <summary>
    ///   Test
    /// </summary>
    [Test]
    public void Monitor_Test() {
      try {
        string[] test = { "NORMAL", "CANCELED", "TIMED OUT" };
        var tasks = new List<Task>();
        var timeout = Convert.ToDouble(Settings.TTL);
        var actionCollection = new List<Delegate>(
          new Delegate[] {
            (Action<string,string>)  ((name, status) => Debug.WriteLine($"@{name}: {status}")                 ),
            (Expr)                   (arg => Debug.WriteLine($"@{arg[0]}: Task ID({arg[1]}) has canceled.")   )
          }
        );

        // #############################################################
        Debug.WriteLine($"Executing **** {test[0],10} **** test.  Running for {2,2} seconds.  Timeout at {timeout / 1000} seconds.");

        var t1 = new TaskEvent(timeout) {
          Name = "t1",
          Expression = actionCollection,
          OnRunning = (obj, tea) => {
            SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 2));
          },
          OnCompleted = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Completed);
          },
          OnTimedout = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Timeout);
            Assert.Fail($"timedout @{tea.Name}");
          },
          OnCanceled = (th, tea) => {
            tea.Expression[1].DynamicInvoke(new object[] { new[] { tea.Name, tea.Task.Id.ToString()}});
            Assert.Fail($"canceled @{tea.Name}");
          },
          OnExited = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Exited);
            tasks.Remove(tea.Task);
          }
        };

        tasks.Add(t1.AsyncMonitor());


        // #############################################################
        Debug.WriteLine($"Executing **** {test[1],10} **** test.  Running for {10,2} seconds.  Timeout at {timeout / 1000} seconds.  Interrupt at 4 seconds.");

        var t2 = new TaskEvent(timeout) {
          Name = "t2",
          Expression = actionCollection,
          OnRunning = (obj, tea) => {
            // Poll longer than Timeout threshold
            SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10));
          },
          OnCompleted = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Completed);
            Assert.Fail($"completed @{tea.Name}");
          },
          OnTimedout = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Timeout);
            Assert.Fail($"timedout @{tea.Name}");
          },
          OnCanceled = (th, tea) => {
            tea.Expression[1].DynamicInvoke(new object[] { new[] { tea.Name, tea.Task.Id.ToString() } });
          },
          OnExited = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Exited);
            tasks.Remove(tea.Task);
          }
        };
        
        #region Triger
        // ------------------------------------
        Task.Run(() => {
          Task.Delay(TimeSpan.FromSeconds(4)).Wait();
          t2.TokenSource.Cancel();
        });
        // ------------------------------------
        #endregion Trigger

        tasks.Add(t2.AsyncMonitor());


        // #############################################################
        Debug.WriteLine($"Executing **** {test[2],10} **** test.  Running for {10,2} seconds.  Timeout at {timeout / 1000} seconds.");

        var t3 = new TaskEvent(timeout) {
          Name = "t3",
          Expression = actionCollection,
          OnRunning = (obj, tea) => {
            // Poll longer than Timeout threshold
            SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10));
          },
          OnCompleted = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Completed);
            Assert.Fail($"completed @{tea.Name}");
          },
          OnTimedout = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Timeout);
          },
          OnCanceled = (th, tea) => {
            tea.Expression[1].DynamicInvoke(new object[] { new[] { tea.Name, tea.Task.Id.ToString() } });
            Assert.Fail($"canceled @{tea.Name}");
          },
          OnExited = (th, tea) => {
            tea.Expression[0].DynamicInvoke(tea.Name, Messages.Exited);
            tasks.Remove(tea.Task);
          }
        };

        tasks.Add(t3.AsyncMonitor());


        // Wait for all the tasks to finish.
        Task.WaitAll(tasks.ToArray());

        Debug.WriteLine(@"All Tasks Completed");
      } catch (OperationCanceledException) {
      } catch (Exception ex) {
        _aTimer.Stop();
        Debug.Print(ex.Message);
        Environment.Exit(1);
      }
    }
  }
}
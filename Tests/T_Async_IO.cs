//  *****************************************************************************
//  File:       T_Async_IO.cs
//  Solution:   ORM-Monitor
//  Project:    Tests
//  Date:       11/06/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ORM_Monitor;
using ReflectSoftware.Insight;
using Tests.Properties;
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
      _log.SendStart(GetType().Name);

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
      _log.SendStop(GetType().Name);
    }

    private readonly ReflectInsight _log = RILogManager.Default;
    private Timer _aTimer;
    private int _tick;

    /// <summary>
    ///   Test
    /// </summary>
    [Test]
    public void Monitor_Test() {
      TaskEventArgs<MyType>.Expression expression = args => {
        var obj = args[0];
        var str = args[1] as string;
        var testNo = obj != null ? $"{((TaskEvent<dynamic>) obj).Name}: " : string.Empty;
        _log.SendInformation(testNo + str);
        return default(MyType);
      };

      // Construct started r
      var tasks = new Task[3];
      Func<int?, string> canceled = id => $"Task ID({id}) has canceled.";

      try {
        // #############################################################
        Debug.WriteLine("Processing **** NORMAL    **** test");
        //

        var @t0 = new TaskEvent<MyType>(expression, TimeSpan.FromSeconds(Settings.Default.TTL)) {
          Name = "t0"
        };

        @t0.OnRunning(
          (obj, tea) => {
            SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 2));
          }
        );

        @t0.OnCompleted(
          (th, tea) => {
            tea?.Invoke(@t0, Messages.Completed);
          }
        );

        @t0.OnTimeout(
          (th, tea) => {
            tea?.Invoke(@t0, Messages.Timeout);
            Assert.Fail("timeout @t0");
          }
        );

        @t0.OnCanceled(
          (th, tea) => {
            tea?.Invoke(@t0, canceled(Task.CurrentId));
            Assert.Fail("canceled @t0");
          }
        );

        tasks[0] = @t0.AsyncMonitor();

        // #############################################################
        Debug.WriteLine("Processing **** CANCELED  **** test");
        //

        var @t1 = new TaskEvent<MyType>(expression, TimeSpan.FromSeconds(Settings.Default.TTL)) {
          Name = "t1"
        };

        @t1.OnRunning(
          (obj, tea) => {
            // Poll longer than Timeout threshold
            SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10));
          }
        );

        @t1.OnCompleted(
          (th, tea) => {
            tea?.Invoke(@t1, Messages.Completed);
            Assert.Fail("completed @t1");
          }
        );

        @t1.OnTimeout(
          (th, tea) => {
            tea?.Invoke(@t1, Messages.Timeout);
            Assert.Fail("timeout @t1");
          }
        );

        @t1.OnCanceled(
          (th, tea) => {
           tea?.Invoke(@t1, canceled(Task.CurrentId));
          }
        );

        var foo = new Action<object>(task => {
          var te = task as TaskEvent<MyType>;
          if (te == null)
            throw new NullReferenceException(nameof(te));

          Task.Delay(TimeSpan.FromSeconds(3)).Wait();
          te.TokenSource.Cancel();
        });

        new Thread(() => foo(@t1)).Start();

        tasks[1] = @t1.AsyncMonitor();

        // #############################################################
        Debug.WriteLine("Processing **** TIMED OUT **** test");
        //

        var @t2 = new TaskEvent<MyType>(expression, TimeSpan.FromSeconds(Settings.Default.TTL)) {
          Name = "t2"
        };

        @t2.OnRunning(
          (th, tea) => {
            // Poll longer than Timeout threshold
            SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 6));
          }
        );

        @t2.OnCompleted(
          (th, tea) => {
            tea?.Invoke(@t2, Messages.Completed);
            Assert.Fail("completed @t2");
          }
        );

        @t2.OnTimeout(
          (th, tea) => {
            tea?.Invoke(@t2, Messages.Timeout);
          }
        );

        @t2.OnCanceled(
          (th, tea) => {
            tea?.Invoke(@t2, canceled(Task.CurrentId));
            Assert.Fail("canceled @t2");
          }
        );

        tasks[2] = @t2.AsyncMonitor();

        // Wait for all the tasks to finish.
        Task.WaitAll(tasks);
        Debug.WriteLine("All Tasks Completed");
      } catch (OperationCanceledException) {
      } catch (SuccessException) {
        // Records a message in the test result.
      } catch (Exception ex) {
        _aTimer.Stop();
        Debug.Print(ex.Message);
        Environment.Exit(1);
      }

      // Run test for 10 seconds to ensure all threads completed.
      while (_tick < 10) {
      }
    }
  }
}
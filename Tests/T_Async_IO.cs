// *****************************************************************************
// File:       T_Async_IO.cs
// Solution:   ORM-Monitor
// Project:    Tests
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask.DTO;
using AsyncTask.Logging;
using NUnit.Framework;
using Timer = System.Timers.Timer;

namespace Tests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class T_Async_IO
    {
        private Timer _aTimer;
        private int _tick;


        /// <summary>
        ///     SetUp
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _aTimer = new Timer {Interval = 1000, Enabled = true};
            _aTimer.Elapsed += (_, _) => TestContext.Progress.WriteLine($"Tick #{++_tick}");
        }


        /// <summary>
        ///     TearDown
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _aTimer.Stop();
        }


        /// <summary>
        ///     Test
        /// </summary>
        [Test]
        public void Monitor_Test()
        {
            try
            {
                var taskList = new TaskList();
                const int timeout = 5;
                string[] test = {"NORMAL", "CANCELED", "TIMED OUT"};
                Action<string, string> expr = (name, msg) => TestContext.Progress.WriteLine($"@{name}:  {msg}");

                // #############################################################
                TestContext.Progress.WriteLine($"Executing **** {test[0]} **** test.  Running for 2 seconds.  Timeout at {timeout} seconds.");

                var t1 = new AsyncTask.AsyncTask(_ => Thread.Sleep(TimeSpan.FromSeconds(2)))
                {
                    TaskInfo = new TaskInfo
                    {
                        Name = "t1"
                    },
                    Timeout    = TimeSpan.FromSeconds(timeout),
                    Logger     = new DefaultLogger(),
                    TaskList   = taskList,
                    OnAdd      = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, "Adding task"),
                    OnRemove   = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, "Removing task"),
                    OnComplete = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Completing task for '{asyncTask.TaskInfo?.Name}'."),
                    OnTimeout  = (asyncTask, _) =>
                    {
                        expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Timeout for '{asyncTask.TaskInfo?.Name}'.");
                        Assert.Fail($"OnTimeout @{asyncTask.TaskInfo?.Name}");
                    },
                    OnCanceled = (asyncTask, _) =>
                    {
                        expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Canceling '{asyncTask.TaskInfo?.Name}'.");
                        Assert.Fail($"OnCanceled @{asyncTask.TaskInfo?.Name}");
                    }
                };
                t1.Start();


                // #############################################################
                TestContext.Progress.WriteLine($"Executing **** {test[1]} **** test.  Running for 10 seconds.  Timeout at {timeout} seconds.  Interrupt at 4 seconds.");

                var t2 = new AsyncTask.AsyncTask(_ => Thread.Sleep(TimeSpan.FromSeconds(10)))
                {
                    TaskInfo = new TaskInfo
                    {
                        Name = "t2"
                    },
                    Timeout    = TimeSpan.FromSeconds(timeout),
                    Logger     = new DefaultLogger(),
                    TaskList   = taskList,
                    OnAdd      = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, "Adding task"),
                    OnRemove   = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, "Removing task"),
                    OnComplete = (asyncTask, _) =>
                    {
                        expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Completing task for '{asyncTask.TaskInfo?.Name}'.");
                        Assert.Fail($"OnComplete @{asyncTask.TaskInfo?.Name}");
                    },
                    OnTimeout  = (asyncTask, _) =>
                    {
                        expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Timeout for '{asyncTask.TaskInfo?.Name}'.");
                        Assert.Fail($"OnTimeout @{asyncTask.TaskInfo?.Name}");
                    },
                    OnCanceled = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Canceling '{asyncTask.TaskInfo?.Name}'.")
                };
                t2.Start();

                #region Triger

                // ------------------------------------
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(4));
                    t2.Cancel();
                });
                // ------------------------------------

                #endregion Trigger


                //// #############################################################
                TestContext.Progress.WriteLine($"Executing **** {test[2]} **** test.  Running for 10 seconds.  Timeout at {timeout} seconds.");

                var t3 = new AsyncTask.AsyncTask(_ => Thread.Sleep(TimeSpan.FromSeconds(10)))
                {
                    TaskInfo = new TaskInfo()
                    {
                        Name = "t3"
                    },
                    Timeout    = TimeSpan.FromSeconds(timeout),
                    Logger     = new DefaultLogger(),
                    TaskList   = taskList,
                    OnAdd      = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, "Adding task"),
                    OnRemove   = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, "Removing task"),
                    OnComplete = (asyncTask, _) =>
                    {
                        expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Completing task for '{asyncTask.TaskInfo?.Name}'.");
                        Assert.Fail($"OnComplete @{asyncTask.TaskInfo?.Name}");
                    },
                    OnTimeout  = (asyncTask, _) => expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Timeout for '{asyncTask.TaskInfo?.Name}'."),
                    OnCanceled = (asyncTask, _) =>
                    {
                        expr.DynamicInvoke(asyncTask.TaskInfo?.Name, $"Canceling '{asyncTask.TaskInfo?.Name}'.");
                        Assert.Fail($"OnCanceled @{asyncTask.TaskInfo?.Name}");
                    }
                };
                t3.Start();

                // Wait for all the tasks to finish.
                try
                {
                    Task.WaitAll(t1, t2, t3);
                }
                catch
                {
                    // ignored
                }

                TestContext.Progress.WriteLine(@"All Tasks Completed");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
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
            _aTimer.Elapsed += (sender, args) => TestContext.Progress.WriteLine($"Tick #{++_tick}");
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

                var t1 = new AsyncTask.Tasks.AsyncTask
                {
                    TaskInfo = new TaskInfo
                    {
                        Name = "t1"
                    },
                    Timeout = TimeSpan.FromSeconds(timeout),
                    Logger = new DefaultLogger(),
                    TaskList = taskList,
                    Delegate = (asyncTask, args) => Thread.Sleep(TimeSpan.FromSeconds(2)),
                    OnAdd = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, "Adding task"),
                    OnRemove = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, "Removing task"),
                    OnComplete = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, $"Completing task for '{args.TaskInfo.Name}'."),
                    OnTimeout = (asyncTask, args) =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, $"Timeout for '{args.TaskInfo.Name}'.");
                        Assert.Fail($"OnTimeout @{args.TaskInfo.Name}");
                    },
                    OnCanceled = (asyncTask, args) =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, $"Canceling '{args.TaskInfo.Name}'.");
                        Assert.Fail($"OnCanceled @{args.TaskInfo.Name}");
                    }
                };
                t1.Start();


                // #############################################################
                TestContext.Progress.WriteLine($"Executing **** {test[1]} **** test.  Running for 10 seconds.  Timeout at {timeout} seconds.  Interrupt at 4 seconds.");

                var t2 = new AsyncTask.Tasks.AsyncTask
                {
                    TaskInfo = new TaskInfo
                    {
                        Name = "t2"
                    },
                    Timeout = TimeSpan.FromSeconds(timeout),
                    Logger = new DefaultLogger(),
                    TaskList = taskList,
                    Delegate = (asyncTask, args) => Thread.Sleep(TimeSpan.FromSeconds(10)),
                    OnAdd = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, "Adding task"),
                    OnRemove = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, "Removing task"),
                    OnComplete = (asyncTask, args) =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, $"Completing task for '{args.TaskInfo.Name}'.");
                        Assert.Fail($"OnComplete @{args.TaskInfo.Name}");
                    },
                    OnTimeout = (asyncTask, args) =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, $"Timeout for '{args.TaskInfo.Name}'.");
                        Assert.Fail($"OnTimeout @{args.TaskInfo.Name}");
                    },
                    OnCanceled = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, $"Canceling '{args.TaskInfo.Name}'.")
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

                var t3 = new AsyncTask.Tasks.AsyncTask
                {
                    TaskInfo = new TaskInfo()
                    {
                        Name = "t3"
                    },
                    Timeout = TimeSpan.FromSeconds(timeout),
                    Logger = new DefaultLogger(),
                    TaskList = taskList,
                    Delegate = (asyncTask, args) => Thread.Sleep(TimeSpan.FromSeconds(10)),
                    OnAdd = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, "Adding task"),
                    OnRemove = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, "Removing task"),
                    OnComplete = (asyncTask, args) =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, $"Completing task for '{args.TaskInfo.Name}'.");
                        Assert.Fail($"OnComplete @{args.TaskInfo.Name}");
                    },
                    OnTimeout = (asyncTask, args) => expr.DynamicInvoke(args.TaskInfo.Name, $"Timeout for '{args.TaskInfo.Name}'."),
                    OnCanceled = (asyncTask, args) =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, $"Canceling '{args.TaskInfo.Name}'.");
                        Assert.Fail($"OnCanceled @{args.TaskInfo.Name}");
                    }
                };
                t3.Start();

                // Wait for all the tasks to finish.
                try
                {
                    Task.WaitAll(t1.Task, t2.Task, t3.Task);
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
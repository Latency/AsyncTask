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
using Tests.Properties;
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
                //TestContext.Progress.WriteLine($"Executing **** {test[0]} **** test.  Running for 2 seconds.  Timeout at {timeout} seconds.");

                //var t1 = new AsyncTask.Tasks.AsyncTask
                //{
                //    TaskInfo = new TaskInfo
                //    {
                //        Name = "t1"
                //    },
                //    Timeout = TimeSpan.FromSeconds(timeout),
                //    Logger = new DefaultLogger(),
                //    TaskList = taskList,
                //    Delegate = _ => SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(2)),
                //    OnAdd = args => TestContext.Progress.WriteLine($"Adding task for '{args.TaskInfo.Name}'"),
                //    OnRemove = args => TestContext.Progress.WriteLine($"Removing task for '{args.TaskInfo.Name}'."),
                //    OnComplete = args => expr.DynamicInvoke(args.TaskInfo.Name, Messages.Completed),
                //    OnTimeout = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Timeout);
                //        Assert.Fail($"OnTimeout @{args.TaskInfo.Name}");
                //    },
                //    OnCanceled = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Canceled);
                //        Assert.Fail($"OnCanceled @{args.TaskInfo.Name}");
                //    }
                //};
                //t1.Register();

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
                    Delegate = _ => SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(10)),
                    OnAdd = args => TestContext.Progress.WriteLine($"Adding task for '{args.TaskInfo.Name}'"),
                    OnRemove = args => TestContext.Progress.WriteLine($"Removing task for '{args.TaskInfo.Name}'."),
                    OnComplete = args => expr.DynamicInvoke(args.TaskInfo.Name, Messages.Completed),
                    OnTimeout = args =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Timeout);
                        Assert.Fail($"OnTimeout @{args.TaskInfo.Name}");
                    },
                    OnCanceled = args => expr.DynamicInvoke(args.TaskInfo.Name, Messages.Canceled)
                };
                t2.Register();

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
                //TestContext.Progress.WriteLine($"Executing **** {test[2]} **** test.  Running for 10 seconds.  Timeout at {timeout} seconds.");

                //var t3 = new AsyncTask.Tasks.AsyncTask
                //{
                //    TaskInfo = new TaskInfo()
                //    {
                //        Name = "t3"
                //    },
                //    Timeout = TimeSpan.FromSeconds(timeout),
                //    Logger = new DefaultLogger(),
                //    TaskList = taskList,
                //    Delegate = _ => SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(10)),
                //    OnAdd = args => TestContext.Progress.WriteLine($"Adding task for '{args.TaskInfo.Name}'"),
                //    OnRemove = args => TestContext.Progress.WriteLine($"Removing task for '{args.TaskInfo.Name}'."),
                //    OnComplete = args => expr.DynamicInvoke(args.TaskInfo.Name, Messages.Completed),
                //    OnTimeout = args => expr.DynamicInvoke(args.TaskInfo.Name, Messages.Timeout),
                //    OnCanceled = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Canceled);
                //        Assert.Fail($"OnCanceled @{args.TaskInfo.Name}");
                //    }
                //};
                //t3.Register();

                //// Wait for all the tasks to finish.
                //Task.WaitAll(t1.Task, t2.Task, t3.Task);

                Task.WaitAll(t2.Task);

                TestContext.Progress.WriteLine(@"All Tasks Completed");
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
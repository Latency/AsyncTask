// *****************************************************************************
// File:       T_Async_IO.cs
// Solution:   ORM-Monitor
// Project:    Tests
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ORM_Monitor.Interfaces;
using ORM_Monitor.Logging;
using ORM_Monitor.Tasks;
using Tests.Properties;
using Timer = System.Timers.Timer;

namespace Tests
{
    public class TaskType : ITaskInfo
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public override string ToString() => $"{Id}:  {Name}";
    }


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
            _aTimer.Elapsed += (sender, args) => { Debug.WriteLine("Tick #{0}", ++_tick); };
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
                const int timeout = 5;
                var taskList = new ConcurrentDictionary<ITaskInfo, ITask>();
                string[] test = {"NORMAL", "CANCELED", "TIMED OUT"};
                Action<string, string> expr = (name, msg) => Debug.WriteLine($"@{name}:  {msg}");

                // #############################################################

                var t1 = new AsyncTask
                {
                    TaskInfo = new TaskType
                    {
                        Name = "t1",
                        Id = 1
                    },
                    Timeout = new TimeSpan(0, 0, timeout),
                    Logger = new DefaultLogger(),
                    TaskList = taskList,
                    Delegate = async token => await Task.Delay(TimeSpan.FromSeconds(2), token),
                    OnAdd = args =>
                    {
                        Debug.WriteLine(
                            $"Adding task for '{args.TaskInfo.Name}'\n" +
                            $"\tIsGeneric: {args.IsGeneric}\n" +
                            $"\tDuration:  {args.Duration}\n" +
                            $"\tTaskType:  {args.TaskInfo}"
                        );
                    },
                    OnRemove = args =>
                    {
                        Debug.WriteLine($"Removing task for '{args.TaskInfo.Name}'.");
                    },
                    OnComplete = args =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Completed);
                    },
                    OnTimeout = args =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Timeout);
                        Assert.Fail($"OnTimeout @{args.TaskInfo.Name}");
                    },
                    OnCanceled = args =>
                    {
                        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Canceled);
                        Assert.Fail($"OnCanceled @{args.TaskInfo.Name}");
                    }
                };
                t1.Register();

                t1.Logger.Debug($"Executing **** {test[0]} **** test.  Running for 2 seconds.  Timeout at {timeout} seconds.");

                //// #############################################################
                //Debug.WriteLine($"Executing **** {test[1]} **** test.  Running for 10 seconds.  Timeout at {timeout} seconds.  Interrupt at 4 seconds.");

                //var t2 = new AsyncTask
                //{
                //    TaskInfo = new TaskType
                //    {
                //        Name = "t2",
                //        Id = 2
                //    },
                //    Timeout = new TimeSpan(0, 0, timeout),
                //    Logger = new DefaultLogger(),
                //    TaskList = taskList,
                //    Delegate = token => SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10)),
                //    OnAdd = args =>
                //    {
                //        Debug.WriteLine(
                //            $"Adding task for '{args.TaskInfo.Name}'\n" +
                //            $"\tIsGeneric: {args.IsGeneric}\n" +
                //            $"\tDuration:  {args.Duration}\n" +
                //            $"\tTaskType:  {args.TaskInfo}"
                //        );
                //    },
                //    OnRemove = args => Debug.WriteLine($"Removing task for '{args.TaskInfo.Name}'."),
                //    OnComplete = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Completed);
                //        Assert.Fail($"OnComplete @{args.TaskInfo.Name}");
                //    },
                //    OnTimeout = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Timeout);
                //        Assert.Fail($"OnTimeout @{args.TaskInfo.Name}");
                //    },
                //    OnCanceled = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Canceled);
                //    }
                //};
                //t2.Register();

                //#region Triger

                //// ------------------------------------
                //Task.Run(async () =>
                //{
                //    await Task.Delay(TimeSpan.FromSeconds(4));
                //    t2.Cancel();
                //});
                //// ------------------------------------

                //#endregion Trigger


                //// #############################################################
                //Debug.WriteLine($"Executing **** {test[2]} **** test.  Running for 10 seconds.  Timeout at {timeout} seconds.");

                //var t3 = new AsyncTask()
                //{
                //    TaskInfo = new TaskType
                //    {
                //        Name = "t3",
                //        Id = 3
                //    },
                //    Timeout = new TimeSpan(0, 0, timeout),
                //    Logger = new DefaultLogger(),
                //    TaskList = taskList,
                //    Delegate = token => SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10)),
                //    OnAdd = args =>
                //    {
                //        Debug.WriteLine(
                //            $"Adding task for '{args.TaskInfo.Name}'\n" +
                //            $"\tIsGeneric: {args.IsGeneric}\n" +
                //            $"\tDuration:  {args.Duration}\n" +
                //            $"\tTaskType:  {args.TaskInfo}"
                //        );
                //    },
                //    OnRemove = args => Debug.WriteLine($"Removing task for '{args.TaskInfo.Name}'."),
                //    OnComplete = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Completed);
                //        Assert.Fail($"OnComplete @{args.TaskInfo.Name}");
                //    },
                //    OnTimeout = args => expr.DynamicInvoke(args.TaskInfo.Name, Messages.Timeout),
                //    OnCanceled = args =>
                //    {
                //        expr.DynamicInvoke(args.TaskInfo.Name, Messages.Canceled);
                //        Assert.Fail($"OnCanceled @{args.TaskInfo.Name}");
                //    }
                //};
                //t3.Register();

                // Wait for all the tasks to finish.
                //Task.WaitAll(t1.Task, t2.Task, t3.Task);

                t1.Task.GetAwaiter();

                Debug.WriteLine(@"All Tasks Completed");
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _aTimer.Stop();
                Debug.Print(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
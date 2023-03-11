// *****************************************************************************
// File:       T_Async_IO.cs
// Solution:   ORM-Monitor
// Project:    Tests
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask.DTO;

namespace Tests
{
    public sealed class TestSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state) => d(state);
        public override void Send(SendOrPostCallback d, object state) => d(state);
    }


    // ReSharper disable once InconsistentNaming
    public partial class Tests
    {
        /// <summary>
        ///     Test
        /// </summary>
        [Fact]
        public async Task Monitor_Test()
        {
            try
            {
                var       taskList               = new TaskList();
                const int timeout                = 5;
                string[]  test                   = {"NORMAL", "CANCELED", "TIMED OUT"};

                void Expr(string name, string msg) => Console.WriteLine($"@{name}:  {msg}");

                // #############################################################
                var t1 = new AsyncTask.AsyncTask((task, args) =>
                {
                    do
                    {
                        task.TaskInfo.Token.ThrowIfCancellationRequested();
                        Task.Delay(250).GetAwaiter().GetResult();
                    }
                    while (args.Duration < TimeSpan.FromSeconds(2));
                })
                {
                    TaskInfo = new TaskInfo
                    {
                        Name = "t1",
                        Timeout = TimeSpan.FromSeconds(timeout),
                        TaskList = taskList,
                        PollInterval = new TimeSpan(0, 0, 1),
                        SynchronizationContext = new TestSynchronizationContext()
                    },
                    OnAdd = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Adding task {test[0]}.  Running for 2 seconds.  Timeout at {timeout} seconds."),
                    OnRemove = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Removing task {test[0]}."),
                    OnComplete = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Completing task for '{asyncTask.TaskInfo.Name}'."),
                    OnTick = (asyncTask, args) => Expr(asyncTask.TaskInfo.Name, $"Duration:  {args.Duration:hh\\:mm\\:ss}"),
                    OnTimeout = (asyncTask, _) =>
                   {
                       Expr(asyncTask.TaskInfo.Name, $"Timeout for '{asyncTask.TaskInfo.Name}'.");
                       Assert.Fail($"OnTimeout @{asyncTask.TaskInfo.Name}");
                   },
                    OnCanceled = (asyncTask, _) =>
                    {
                        Expr(asyncTask.TaskInfo.Name, $"Canceling '{asyncTask.TaskInfo.Name}'.");
                        Assert.Fail($"OnCanceled @{asyncTask.TaskInfo.Name}");
                    }
                };
                t1.Start();


                // #############################################################
                var t2 = new AsyncTask.AsyncTask((task, args) =>
                {
                    do
                    {
                        task.TaskInfo.Token.ThrowIfCancellationRequested();
                        Task.Delay(250).GetAwaiter().GetResult();
                    }
                    while (args.Duration < TimeSpan.FromSeconds(10));
                })
                {
                    TaskInfo = new TaskInfo
                    {
                        Name = "t2",
                        Timeout = TimeSpan.FromSeconds(timeout),
                        TaskList = taskList,
                        PollInterval = new TimeSpan(0, 0, 1),
                        SynchronizationContext = new TestSynchronizationContext()
                    },
                    OnAdd = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Adding task {test[1]}.  Running for 10 seconds.  Timeout at {timeout} seconds.  Interrupt at 4 seconds."),
                    OnRemove = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Removing task {test[1]}."),
                    OnComplete = (asyncTask, _) =>
                    {
                        Expr(asyncTask.TaskInfo.Name, $"Completing task for '{asyncTask.TaskInfo.Name}'.");
                        Assert.Fail($"OnComplete @{asyncTask.TaskInfo.Name}");
                    },
                    OnTick = (asyncTask, args) => Expr(asyncTask.TaskInfo.Name, $"Duration:  {args.Duration:hh\\:mm\\:ss}"),
                    OnTimeout = (asyncTask, _) =>
                   {
                       Expr(asyncTask.TaskInfo.Name, $"Timeout for '{asyncTask.TaskInfo.Name}'.");
                       Assert.Fail($"OnTimeout @{asyncTask.TaskInfo.Name}");
                   },
                    OnCanceled = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Canceling '{asyncTask.TaskInfo.Name}'.")
                };
                t2.Start();

                #region Triger
                // ------------------------------------
                #pragma warning disable 4014
                Task.Run(async () =>
                #pragma warning restore 4014
                {
                    await Task.Delay(TimeSpan.FromSeconds(4.4));
                    t2.Cancel();
                });
                // ------------------------------------
                #endregion Trigger


                // #############################################################
                var t3 = new AsyncTask.AsyncTask((task, args) =>
                {
                    do
                    {
                        task.TaskInfo.Token.ThrowIfCancellationRequested();
                        Task.Delay(250).GetAwaiter().GetResult();
                    }
                    while (args.Duration < TimeSpan.FromSeconds(10));
                })
                {
                    TaskInfo = new TaskInfo()
                    {
                        Name = "t3",
                        Timeout = TimeSpan.FromSeconds(timeout),
                        TaskList = taskList,
                        PollInterval = new TimeSpan(0, 0, 1),
                        SynchronizationContext = new TestSynchronizationContext()
                    },
                    OnAdd = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Adding task {test[2]}.  Running for 10 seconds.  Timeout at {timeout} seconds."),
                    OnRemove = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Removing task {test[2]}."),
                    OnComplete = (asyncTask, _) =>
                    {
                        Expr(asyncTask.TaskInfo.Name, $"Completing task for '{asyncTask.TaskInfo.Name}'.");
                        Assert.Fail($"OnComplete @{asyncTask.TaskInfo.Name}");
                    },
                    OnTick = (asyncTask, args) => Expr(asyncTask.TaskInfo.Name, $"Duration:  {args.Duration:hh\\:mm\\:ss}"),
                    OnTimeout = (asyncTask, _) => Expr(asyncTask.TaskInfo.Name, $"Timeout for '{asyncTask.TaskInfo.Name}'."),
                    OnCanceled = (asyncTask, _) =>
                    {
                        Expr(asyncTask.TaskInfo.Name, $"Canceling '{asyncTask.TaskInfo.Name}'.");
                        Assert.Fail($"OnCanceled @{asyncTask.TaskInfo.Name}");
                    }
                };
                t3.Start();

                // Wait for all the tasks to finish.
                try
                {
                    await Task.WhenAll(taskList.Keys.Cast<Task>()).ContinueWith(_ => Console.WriteLine(@"All Tasks Completed."));
                }
                catch
                {
                    // ignored
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
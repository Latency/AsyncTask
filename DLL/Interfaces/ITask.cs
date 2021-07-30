// ****************************************************************************
// Project:  AsyncTask
// File:     ITask.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTask.Interfaces
{
    public interface ITask : IAsyncResult, IDisposable
    {
        ITaskInfo               TaskInfo                { get; set; }
        int                     Id                      { get; }
        AggregateException?     Exception               { get; }
        TaskStatus              Status                  { get; }
        bool                    IsCanceled              { get; }
#if NETCOREAPP3_1_OR_GREATER
        bool                    IsCompletedSuccessfully { get; }
#endif
        TaskCreationOptions     CreationOptions         { get; }
        bool                    IsFaulted               { get; }
        void                    Start();
        void                    Start(TaskScheduler                                scheduler);
        void                    RunSynchronously();
        void                    RunSynchronously(TaskScheduler                     scheduler);
        TaskAwaiter             GetAwaiter();
        ConfiguredTaskAwaitable ConfigureAwait(bool                                continueOnCapturedContext);
        Task                    ContinueWith(Action<Task>                          continuationAction);
        Task                    ContinueWith(Action<Task>                          continuationAction, CancellationToken       cancellationToken);
        Task                    ContinueWith(Action<Task>                          continuationAction, TaskScheduler           scheduler);
        Task                    ContinueWith(Action<Task>                          continuationAction, TaskContinuationOptions continuationOptions);
        Task                    ContinueWith(Action<Task>                          continuationAction, CancellationToken       cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler);
        Task                    ContinueWith(Action<Task, object?>                 continuationAction, object?                 state);
        Task                    ContinueWith(Action<Task, object?>                 continuationAction, object?                 state, CancellationToken       cancellationToken);
        Task                    ContinueWith(Action<Task, object?>                 continuationAction, object?                 state, TaskScheduler           scheduler);
        Task                    ContinueWith(Action<Task, object?>                 continuationAction, object?                 state, TaskContinuationOptions continuationOptions);
        Task                    ContinueWith(Action<Task, object?>                 continuationAction, object?                 state, CancellationToken       cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler);
        Task<TResult>           ContinueWith<TResult>(Func<Task, TResult>          continuationFunction);
        Task<TResult>           ContinueWith<TResult>(Func<Task, TResult>          continuationFunction, CancellationToken       cancellationToken);
        Task<TResult>           ContinueWith<TResult>(Func<Task, TResult>          continuationFunction, TaskScheduler           scheduler);
        Task<TResult>           ContinueWith<TResult>(Func<Task, TResult>          continuationFunction, TaskContinuationOptions continuationOptions);
        Task<TResult>           ContinueWith<TResult>(Func<Task, TResult>          continuationFunction, CancellationToken       cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler);
        Task<TResult>           ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object?                 state);
        Task<TResult>           ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object?                 state, CancellationToken       cancellationToken);
        Task<TResult>           ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object?                 state, TaskScheduler           scheduler);
        Task<TResult>           ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object?                 state, TaskContinuationOptions continuationOptions);
        Task<TResult>           ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object?                 state, CancellationToken       cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler);
        void                    Wait();
        bool                    Wait(TimeSpan                                      timeout);
        void                    Wait(CancellationToken                             cancellationToken);
        bool                    Wait(int                                           millisecondsTimeout);
        bool                    Wait(int                                           millisecondsTimeout, CancellationToken cancellationToken);
#if NET6_0
        Task                    WaitAsync(CancellationToken                        cancellationToken);
        Task                    WaitAsync(TimeSpan                                 timeout);
        Task                    WaitAsync(TimeSpan                                 timeout, CancellationToken cancellationToken);
#endif
        void                    Cancel(bool                                        throwOnFirstException = false);
        void                    Dispatch(string                                    name, Action<ITask, ITaskEventArgs>           method);
    }
}
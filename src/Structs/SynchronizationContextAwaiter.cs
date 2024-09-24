// ****************************************************************************
// Project:  AsyncTask
// File:     SynchronizationContextAwaiter.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Runtime.CompilerServices;

namespace AsyncTask.Structs;

public readonly struct SynchronizationContextAwaiter(SynchronizationContext context) : INotifyCompletion
{
    private static readonly SendOrPostCallback PostCallback = static state => ((Action?)state)?.Invoke();

    /// <summary>
    ///     IsCompleted
    /// </summary>
    public bool IsCompleted => context == SynchronizationContext.Current;

    /// <summary>
    ///     OnCompleted
    /// </summary>
    /// <param name="continuation"></param>
    public void OnCompleted(Action continuation) => context.Post(PostCallback, continuation);
    
    /// <summary>
    ///     GetResult
    /// </summary>
    public void GetResult()
    { }
}
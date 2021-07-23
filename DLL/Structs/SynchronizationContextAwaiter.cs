// ****************************************************************************
// Project:  AsyncTask
// File:     SynchronizationContextAwaiter.cs
// Author:   Latency McLaughlin
// Date:     02/22/2021
// ****************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AsyncTask.Structs
{
    public readonly struct SynchronizationContextAwaiter : INotifyCompletion
    {
        private static readonly SendOrPostCallback PostCallback = state => ((Action) state)?.Invoke();

        private readonly SynchronizationContext _context;
        public SynchronizationContextAwaiter(SynchronizationContext context) => _context = context;

        public bool IsCompleted => _context == SynchronizationContext.Current;

        public void OnCompleted(Action continuation) => _context.Post(PostCallback, continuation);

        public void GetResult() { }
    }
}
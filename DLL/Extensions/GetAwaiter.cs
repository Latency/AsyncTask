// ****************************************************************************
// Project:  AsyncTask
// File:     GetAwaiter.cs
// Author:   Latency McLaughlin
// Date:     02/22/2021
// ****************************************************************************

using System.Threading;
using AsyncTask.Structs;

namespace AsyncTask.Extensions
{
    public static class Threading
    {
        public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context) => new(context);
    }
}
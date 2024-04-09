// ****************************************************************************
// Project:  AsyncTask
// File:     GetAwaiter.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using AsyncTask.Structs;

namespace AsyncTask.Extensions;

public static class Threading
{
    public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context) => new(context);
}
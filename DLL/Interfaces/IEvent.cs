// ****************************************************************************
// Project:  AsyncTask
// File:     IEvent.cs
// Author:   Latency McLaughlin
// Date:     09/17/2020
// ****************************************************************************

using System;
using System.Reflection;

namespace AsyncTask.Interfaces
{
    public interface IEvent : IDisposable
    {
        int Count { get; }
        void Register(object sender, EventInfo evt);
        void UnRegister(object sender, EventInfo evt);
    }
}
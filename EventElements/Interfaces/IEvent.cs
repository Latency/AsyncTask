// ****************************************************************************
// Project:  AsyncTask
// File:     IEvent.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Reflection;

namespace EventElements.Interfaces;

public interface IEvent : IDisposable
{
    int Count { get; }
    void Register(object sender, EventInfo evt);
    void UnRegister(object sender, EventInfo evt);
}
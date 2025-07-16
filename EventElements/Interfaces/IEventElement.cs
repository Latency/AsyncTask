// ****************************************************************************
// Project:  AsyncTask
// File:     IEventElement.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System;
using System.Collections.Generic;

namespace EventElements.Interfaces;

public interface IEventElement<T> : IEvent
{
    List<EventHandler<T>> Delegates { get; }
    EventHandler<T>? this[int index] { get; set; }
    event EventHandler<T> EventDelegate;
    void Dispatch(object sender, T message);
    void Add(EventHandler<T>? kDelegate);
    void Remove(EventHandler<T>? kDelegate);
}
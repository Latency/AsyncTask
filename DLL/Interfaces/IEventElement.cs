// ****************************************************************************
// Project:  AsyncTask
// File:     IEventElement.cs
// Author:   Latency McLaughlin
// Date:     09/10/2020
// ****************************************************************************

using System;
using System.Collections.Generic;

namespace AsyncTask.Interfaces
{
    public interface IEventElement<T> : IEvent
    {
        List<EventHandler<T>> Delegates { get; }
        event EventHandler<T> EventDelegate;
        EventHandler<T> this[int index] { get; set; }
        void Dispatch(object sender, T message);
        void Add(EventHandler<T> kDelegate);
        void Remove(EventHandler<T> kDelegate);
    }
}
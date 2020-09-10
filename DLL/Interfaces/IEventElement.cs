// ****************************************************************************
// Project:  AsyncTask
// File:     IEventElement.cs
// Author:   Latency McLaughlin
// Date:     09/10/2020
// ****************************************************************************

using System;

namespace AsyncTask.Interfaces
{
    public interface IEventElement<T> : IDisposable
    {
        event EventHandler<T> EventDelegate;
        int Count { get; }
        EventHandler<T> this[int index] { get; set; }
        void Dispatch(object sender, T message);
        void Add(EventHandler<T> kDelegate);
        void Remove(EventHandler<T> kDelegate);
    }
}
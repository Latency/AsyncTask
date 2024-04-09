// ****************************************************************************
// Project:  AsyncTask
// File:     IEventCustomElement.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace EventElements.Interfaces;

public interface IEventCustomElement<TSender, TEventArgs> : IEvent
{
    List<Action<TSender, TEventArgs>> Delegates { get; }
    Action<TSender, TEventArgs>? this[int index] { get; set; }
    event Action<TSender, TEventArgs> EventDelegate;
    void Dispatch(TSender sender, TEventArgs message);
    void Add(Action<TSender, TEventArgs>? kDelegate);
    void Remove(Action<TSender, TEventArgs>? kDelegate);
}
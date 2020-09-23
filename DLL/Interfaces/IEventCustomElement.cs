// ****************************************************************************
// Project:  Medtronic.MFI.AAEC.EC.Gem
// File:     ICustomEventElement.cs
// Author:   Latency McLaughlin
// Date:     09/10/2020
// ****************************************************************************

using System;
using System.Collections.Generic;

namespace AsyncTask.Interfaces
{
    public interface IEventCustomElement<TSender, TEventArgs> : IEvent
    {
        List<Action<TSender, TEventArgs>> Delegates { get; }
        event Action<TSender, TEventArgs> EventDelegate;
        Action<TSender, TEventArgs> this[int index] { get; set; }
        void Dispatch(TSender sender, TEventArgs message);
        void Add(Action<TSender, TEventArgs> kDelegate);
        void Remove(Action<TSender, TEventArgs> kDelegate);
    }
}
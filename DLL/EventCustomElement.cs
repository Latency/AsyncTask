// ****************************************************************************
// Project:  AsyncTask
// File:     EventElement.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AsyncTask.Interfaces;

namespace AsyncTask
{
    public class EventCustomElement<TSender, TEventArgs> : IEventCustomElement<TSender, TEventArgs>
    {
        public static implicit operator List<MethodInfo>(EventCustomElement<TSender, TEventArgs> eventElement) => eventElement.Delegates.Select(d => d.Method).ToList();


        /// <summary>
        ///     Constructor (default)
        /// </summary>
        public EventCustomElement()
        { }


        /// <summary>
        ///     Constructor (overload + 1)
        /// </summary>
        /// <param name="kDelegateCollection"></param>
        public EventCustomElement(params Action<TSender, TEventArgs>[] kDelegateCollection)
        {
            foreach (var kDelegate in kDelegateCollection)
                EventDelegate += kDelegate;
        }
        

        /// <summary>
        ///     EventDelegate
        /// </summary>
        protected event Action<TSender, TEventArgs> EventDelegate;
        event Action<TSender, TEventArgs> IEventCustomElement<TSender, TEventArgs>.EventDelegate
        {
            add => EventDelegate += value;
            remove => EventDelegate -= value;
        }


        /// <summary>
        ///     Delegates
        /// </summary>
        public List<Action<TSender, TEventArgs>> Delegates { get; } = new List<Action<TSender, TEventArgs>>();


        /// <summary>
        ///     Count
        /// </summary>
        public int Count => EventDelegate?.GetInvocationList().Length ?? 0;


        /// <summary>
        ///     Operator []
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Action<TSender, TEventArgs> this[int index]
        {
            get => (Action<TSender, TEventArgs>) EventDelegate?.GetInvocationList()[index];
            set
            {
                if (EventDelegate != null)
                    EventDelegate.GetInvocationList()[index] = value;
            }
        }


        /// <summary>
        ///     Dispose
        /// </summary>
        public void Dispose()
        {
            if (EventDelegate == null)
                return;
            foreach (var @delegate in EventDelegate.GetInvocationList())
                EventDelegate -= (Action<TSender, TEventArgs>) @delegate;
        }


        /// <summary>
        ///     Dispatch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void Dispatch(TSender sender, TEventArgs message)
        {
            if (EventDelegate == null)
                return;
            foreach (var @delegate in EventDelegate.GetInvocationList())
            {
                var kDelegate = @delegate as Action<TSender, TEventArgs>;
                kDelegate?.Invoke(sender, message);
            }
        }


        /// <summary>
        ///     Operator +
        /// </summary>
        /// <param name="kElement"></param>
        /// <param name="kDelegate"></param>
        /// <returns>kElement</returns>
        public static EventCustomElement<TSender, TEventArgs> operator +(EventCustomElement<TSender, TEventArgs> kElement, Action<TSender, TEventArgs> kDelegate)
        {
            kElement.EventDelegate += kDelegate;
            return kElement;
        }


        /// <summary>
        ///     Operator -
        /// </summary>
        /// <param name="kElement"></param>
        /// <param name="kDelegate"></param>
        /// <returns>kElement</returns>
        public static EventCustomElement<TSender, TEventArgs> operator -(EventCustomElement<TSender, TEventArgs> kElement, Action<TSender, TEventArgs> kDelegate)
        {
            kElement.EventDelegate -= kDelegate;
            return kElement;
        }


        /// <summary>
        ///     Register
        /// </summary>
        public void Register(object sender, EventInfo evt)
        {
            foreach (var @delegate in Delegates)
                evt.AddEventHandler(sender, @delegate);
        }


        /// <summary>
        ///     UnRegister
        /// </summary>
        public void UnRegister(object sender, EventInfo evt)
        {
            foreach (var @delegate in Delegates)
                evt.RemoveEventHandler(sender, @delegate);
        }


        /// <summary>
        ///     Add
        /// </summary>
        /// <param name="kDelegate"></param>
        public void Add(Action<TSender, TEventArgs> kDelegate) => EventDelegate += kDelegate;


        /// <summary>
        ///     Remove
        /// </summary>
        /// <param name="kDelegate"></param>
        public void Remove(Action<TSender, TEventArgs> kDelegate) => EventDelegate -= kDelegate;
    }
}
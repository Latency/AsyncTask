﻿// ****************************************************************************
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
    public sealed class EventElement<T> : IEventElement<T>
    {
        public static implicit operator List<MethodInfo>(EventElement<T> eventElement) => eventElement.Delegates.Select(d => d.Method).ToList();

 
        /// <summary>
        ///     Constructor (default)
        /// </summary>
        public EventElement()
        { }


        /// <summary>
        ///     Constructor (overload + 1)
        /// </summary>
        /// <param name="kDelegateCollection"></param>
        public EventElement(params EventHandler<T>[] kDelegateCollection)
        {
            foreach (var kDelegate in kDelegateCollection)
                Delegates.Add(kDelegate);
        }


        /// <summary>
        ///     EventDelegate
        /// </summary>
        public event EventHandler<T> EventDelegate;


        /// <summary>
        ///     Delegates
        /// </summary>
        public List<EventHandler<T>> Delegates { get; } = new();


        /// <summary>
        ///     Count
        /// </summary>
        public int Count => EventDelegate?.GetInvocationList().Length ?? 0;


        /// <summary>
        ///     Operator []
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EventHandler<T> this[int index]
        {
            get => (EventHandler<T>) EventDelegate?.GetInvocationList()[index];
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
            Delegates.Clear();
            foreach (var @delegate in EventDelegate.GetInvocationList())
                Remove((EventHandler<T>) @delegate);
        }


        /// <summary>
        ///     Dispatch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void Dispatch(object sender, T message)
        {
            if (EventDelegate == null)
                return;
            foreach (var @delegate in EventDelegate.GetInvocationList())
            {
                var kDelegate = @delegate as EventHandler<T>;
                kDelegate?.Invoke(sender, message);
            }
        }


        /// <summary>
        ///     Operator +
        /// </summary>
        /// <param name="kElement"></param>
        /// <param name="kDelegate"></param>
        /// <returns>kElement</returns>
        public static EventElement<T> operator +(EventElement<T> kElement, EventHandler<T> kDelegate)
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
        public static EventElement<T> operator -(EventElement<T> kElement, EventHandler<T> kDelegate)
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
        public void Add(EventHandler<T> kDelegate) => EventDelegate += kDelegate;


        /// <summary>
        ///     Remove
        /// </summary>
        /// <param name="kDelegate"></param>
        public void Remove(EventHandler<T> kDelegate) => EventDelegate -= kDelegate;
    }
}
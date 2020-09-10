// ****************************************************************************
// Project:  AsyncTask
// File:     EventElement.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.Interfaces;

namespace AsyncTask
{
    public sealed class EventElement<T> : IEventElement<T>
    {


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
                // ReSharper disable once VirtualMemberCallInConstructor
                EventDelegate += kDelegate;
        }


        /// <summary>
        ///     EventDelegate
        /// </summary>
        public event EventHandler<T> EventDelegate;


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
            foreach (var @delegate in EventDelegate.GetInvocationList())
                EventDelegate -= (EventHandler<T>) @delegate;
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
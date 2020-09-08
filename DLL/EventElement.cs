// ****************************************************************************
// Project:  AsyncTask
// File:     EventElement.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;

namespace AsyncTask
{
    public class EventElement<T> : IDisposable
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
                Eventdelegate += kDelegate;
        }


        /// <summary>
        ///     Count
        /// </summary>
        public int Count => Eventdelegate?.GetInvocationList().Length ?? 0;


        /// <summary>
        ///     Operator []
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual EventHandler<T> this[int index]
        {
            get => (EventHandler<T>) Eventdelegate?.GetInvocationList()[index];
            set
            {
                if (Eventdelegate != null)
                    Eventdelegate.GetInvocationList()[index] = value;
            }
        }


        /// <summary>
        ///     Dispose
        /// </summary>
        public virtual void Dispose()
        {
            if (Eventdelegate == null)
                return;
            foreach (var @delegate in Eventdelegate.GetInvocationList())
                Eventdelegate -= (EventHandler<T>) @delegate;
        }

        protected virtual event EventHandler<T> Eventdelegate;


        /// <summary>
        ///     Dispatch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public virtual void Dispatch(object sender, T message)
        {
            if (Eventdelegate == null)
                return;
            foreach (var @delegate in Eventdelegate.GetInvocationList())
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
            kElement.Eventdelegate += kDelegate;
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
            kElement.Eventdelegate -= kDelegate;
            return kElement;
        }
    }
}
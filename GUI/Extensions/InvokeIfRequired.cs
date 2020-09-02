// ****************************************************************************
// Project:  GUI
// File:     InvokeIfRequired.cs
// Author:   Latency McLaughlin
// Date:     08/31/2020
// ****************************************************************************

using System;
using System.Windows.Threading;

namespace ORM_Monitor.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        ///     Simple helper extension method to marshall to correct thread if its required
        /// </summary>
        /// <param name="control"></param>
        /// <param name="methodcall"></param>
        /// <param name="priorityForCall"></param>
        /// <param name="parameters"></param>
        public static T InvokeIfRequired<T>(this DispatcherObject control, Func<object[], T> methodcall, DispatcherPriority priorityForCall, params object[] parameters) =>
            // CheckAccess returns true if you're on the dispatcher thread.
            !control.Dispatcher.CheckAccess() ? (T) control.Dispatcher.Invoke(priorityForCall, methodcall, parameters) : methodcall(parameters);


        /// <summary>
        ///     Simple helper extension method to marshall to correct thread if its required
        /// </summary>
        /// <param name="control"></param>
        /// <param name="methodcall"></param>
        /// <param name="priorityForCall"></param>
        /// <param name="parameters"></param>
        public static void InvokeIfRequired(this DispatcherObject control, Action<object[]> methodcall, DispatcherPriority priorityForCall, params object[] parameters)
        {
            // CheckAccess returns true if you're on the dispatcher thread.
            if (!control.Dispatcher.CheckAccess())
                control.Dispatcher.Invoke(priorityForCall, methodcall, parameters);
            else
                methodcall(parameters);
        }
    }
}
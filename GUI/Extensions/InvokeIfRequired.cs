// ****************************************************************************
// Project:  GUI
// File:     InvokeIfRequired.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Windows.Threading;

namespace ORM_Monitor.Extensions;

public static partial class Extensions
{
    /// <param name="control"></param>
    extension(DispatcherObject control)
    {
        /// <summary>
        ///     Simple helper extension method to marshall to correct thread if its required
        /// </summary>
        /// <param name="methodcall"></param>
        /// <param name="priorityForCall"></param>
        /// <param name="parameters"></param>
        public T InvokeIfRequired<T>(Func<object[], T> methodcall, DispatcherPriority priorityForCall, params object[] parameters) =>
            // CheckAccess returns true if you're on the dispatcher thread.
            !control.Dispatcher.CheckAccess() ? (T)control.Dispatcher.Invoke(priorityForCall, methodcall, parameters) : methodcall(parameters);

        /// <summary>
        ///     Simple helper extension method to marshall to correct thread if its required
        /// </summary>
        /// <param name="methodcall"></param>
        /// <param name="priorityForCall"></param>
        /// <param name="parameters"></param>
        public void InvokeIfRequired(Action<object[]> methodcall, DispatcherPriority priorityForCall, params object[] parameters)
        {
            // CheckAccess returns true if you're on the dispatcher thread.
            if (!control.Dispatcher.CheckAccess())
                control.Dispatcher.Invoke(priorityForCall, methodcall, parameters);
            else
                methodcall(parameters);
        }
    }
}
// ****************************************************************************
// Project:  AsyncTask
// File:     UiDispatcher.cs
// Author:   Latency McLaughlin
// Date:     07/23/2021
// ****************************************************************************
// ReSharper disable UnusedMember.Global

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTask.Extensions
{
    public static class UiDispatcher
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SynchronizationContext UiContext { get; set; }


        /// <summary>
        ///     This method should be called once on the UI thread to ensure that
        ///     the <see cref="UiContext" /> property is initialized.
        ///     <para>
        ///         In a Silverlight application, call this method in the
        ///         Application_Startup event handler, after the MainPage is constructed.
        ///     </para>
        ///     <para>In WPF, call this method on the static App() constructor.</para>
        /// </summary>
        public static void Initialize(SynchronizationContext synchronizationContext)
        {
            UiContext = synchronizationContext ?? SynchronizationContext.Current;
        }


        /// <summary>
        ///     Invokes an action asynchronously on the UI thread.
        /// </summary>
        /// <param name="action">The action that must be executed.</param>
        public static Task InvokeAsync(Action action)
        {
            var t = Task.Run(CheckInitialization);

            UiContext.Post(_ => action(), null);

            return t;
        }


        /// <summary>
        ///     Executes an action on the UI thread. If this method is called
        ///     from the UI thread, the action is executed immendiately. If the
        ///     method is called from another thread, the action will be enqueued
        ///     on the UI thread's dispatcher and executed asynchronously.
        ///     <para>
        ///         For additional operations on the UI thread, you can get a
        ///         reference to the UI thread's context thanks to the property
        ///         <see cref="UiContext" />
        ///     </para>
        ///     .
        /// </summary>
        /// <param name="action">
        ///     The action that will be executed on the UI
        ///     thread.
        /// </param>
        public static void Invoke(Action action)
        {
            CheckInitialization();

            if (UiContext == SynchronizationContext.Current)
                action();
            else
                InvokeAsync(action);
        }


        private static void CheckInitialization()
        {
            if (UiContext == null)
                throw new InvalidOperationException("UiDispatcher is not initialized. Invoke Initialize() first.");
        }
    }
}
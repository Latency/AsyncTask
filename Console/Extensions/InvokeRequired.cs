// ****************************************************************************
// Project:  ConsoleApp1
// File:     InvokeRequired.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Windows.Forms;

namespace ConsoleApp1.Extensions
{
    public static class Controls
    {
        /// <summary>
        ///     Invoke with parameter array. (overload +1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TControlType"></typeparam>
        /// <param name="control"></param>
        /// <param name="act"></param>
        /// <param name="m"></param>
        /// <param name="del"></param>
        /// <param name="args"></param>
        public static void InvokeIfRequired<TControlType, T>(this TControlType control, Action<T[]> act, T[] m, Delegate del, params object[] args) where TControlType : Control
        {
            if (control.InvokeRequired)
                control.BeginInvoke(del, args);
            else
                act(m);
        }


        /// <summary>
        ///     Invoke with single parameter. (overload +2)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TControlType"></typeparam>
        /// <param name="control"></param>
        /// <param name="act"></param>
        /// <param name="m"></param>
        /// <param name="del"></param>
        /// <param name="args"></param>
        public static void InvokeIfRequired<TControlType, T>(this TControlType control, Action<T> act, T m, Delegate del, params object[] args) where TControlType : Control
        {
            if (control.InvokeRequired)
                control.BeginInvoke(del, args);
            else
                act(m);
        }


        /// <summary>
        ///     Invoke with no parameters. (overload +3)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TControlType"></typeparam>
        /// <param name="control"></param>
        /// <param name="func"></param>
        /// <param name="del"></param>
        /// <param name="args"></param>
        /// <returns>Value from func</returns>
        public static T InvokeIfRequired<TControlType, T>(this TControlType control, Func<T> func, Delegate del, params object[] args) where TControlType : Control
        {
            if (!control.InvokeRequired)
                return func();
            var iAsyncResult = control.BeginInvoke(del, args);
            return (T) control.EndInvoke(iAsyncResult);
        }


        /// <summary>
        ///     Invoke with no parameters. (overload +4)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="del"></param>
        /// <param name="args"></param>
        public static void InvokeIfRequired<TControlType>(this TControlType control, Action action, Delegate del, params object[] args) where TControlType : Control
        {
            if (control.InvokeRequired)
                control.BeginInvoke(del, args);
            else
                action();
        }
    }
}
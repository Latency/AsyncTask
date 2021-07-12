// ****************************************************************************
// Project:  ConsoleApp1
// File:     Logger.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using AsyncTask.Interfaces;
using ConsoleApp1.Extensions;

namespace Console.Extensions
{
    public static class Logger
    {
        public static void Message<T>(this RichTextBox control, object sender, T messageEventArgs) where T : IMessageEventArgs => control.InvokeIfRequired(a =>
        {
            control.SelectionColor = Color.White;
            control.AppendText(a.Message + Environment.NewLine);
        }, messageEventArgs, new Action<RichTextBox, object, T>(Message), control, sender, messageEventArgs);


        public static void Information<T>(this RichTextBox control, object sender, T messageEventArgs) where T : IMessageEventArgs => control.InvokeIfRequired(a =>
        {
            control.SelectionColor = Color.DodgerBlue;
            control.AppendText(a.Message + Environment.NewLine);
        }, messageEventArgs, new Action<RichTextBox, object, T>(Information), control, sender, messageEventArgs);


        public static void Warning<T>(this RichTextBox control, object sender, T messageEventArgs) where T : IMessageEventArgs => control.InvokeIfRequired(a =>
        {
            control.SelectionColor = Color.Yellow;
            control.AppendText(a.Message + Environment.NewLine);
        }, messageEventArgs, new Action<RichTextBox, object, T>(Warning), control, sender, messageEventArgs);


        public static void Error<T>(this RichTextBox control, object sender, T messageEventArgs) where T : IMessageEventArgs => control.InvokeIfRequired(a =>
        {
            control.SelectionColor = Color.Red;
            control.AppendText(a.Message);
            if (messageEventArgs.Exception != null)
            {
                control.SelectionColor = Color.Gray;
                control.AppendText(a.Exception.Message + Environment.NewLine);
            }
        }, messageEventArgs, new Action<RichTextBox, object, T>(Error), control, sender, messageEventArgs);


        public static void Debug<T>(this RichTextBox control, object sender, T messageEventArgs) where T : IMessageEventArgs => control.InvokeIfRequired(a =>
        {
            control.SelectionColor = Color.DarkOrange;
            control.AppendText(a.Message + Environment.NewLine);
        }, messageEventArgs, new Action<RichTextBox, object, T>(Debug), control, sender, messageEventArgs);
    }
}
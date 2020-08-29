// ****************************************************************************
// Project:  ConsoleApp1
// File:     Logger.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Windows.Forms;
using AsyncTask.EventArgs;
using AsyncTask.Logging;
using ConsoleApp1.Extensions;

namespace ConsoleApp1.Logging
{
    /// <summary>
    ///     Logger
    /// </summary>
    public sealed class Logger : DefaultLogger
    {
        private readonly RichTextBox _control;

        public Logger(RichTextBox control)
        {
            _control = control;
        }


        public override void Debug(string msg) => _control.Debug(this, new MessageEventArgs { Message = msg });

        public override void Info(string msg) => _control.Info(this, new MessageEventArgs { Message = msg });

        public override void Warning(string msg) => _control.Warning(this, new MessageEventArgs { Message = msg });

        public override void Error(string msg, Exception ex = null) => _control.Error(this, new MessageEventArgs { Message = msg, Exception = ex });
    }
}
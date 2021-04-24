// ****************************************************************************
// Project:  AsyncTask
// File:     Message.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.Interfaces;

namespace AsyncTask.EventArgs
{
    public class MessageEventArgs : System.EventArgs, IMessageEventArgs
    {
        public string    Message   { get; set; } = string.Empty;
        public Exception Exception { get; set; }
    }
}
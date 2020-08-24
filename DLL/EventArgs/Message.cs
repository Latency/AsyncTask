// ****************************************************************************
// Project:  AsyncTask
// File:     Message.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;

namespace AsyncTask.EventArgs
{
    public class MessageEventArgs : System.EventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
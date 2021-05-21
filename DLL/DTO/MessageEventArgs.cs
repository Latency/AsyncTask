// ****************************************************************************
// Project:  AsyncTask
// File:     MessageEventArgs.cs
// Author:   Latency McLaughlin
// Date:     08/24/2020
// ****************************************************************************

using System;
using AsyncTask.Interfaces;

namespace AsyncTask.DTO
{
    // ReSharper disable once UnusedMember.Global
    public class MessageEventArgs : EventArgs, IMessageEventArgs
    {
        public string    Message   { get; set; } = string.Empty;
        public Exception Exception { get; set; }
    }
}
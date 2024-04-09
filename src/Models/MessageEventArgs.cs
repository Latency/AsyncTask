// ****************************************************************************
// Project:  AsyncTask
// File:     MessageEventArgs.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using AsyncTask.Interfaces;

namespace AsyncTask.Models;

public class MessageEventArgs : EventArgs, IMessageEventArgs
{
    public string     Message   { get; set; } = string.Empty;
    public Exception? Exception { get; set; }
}
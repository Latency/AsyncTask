// ****************************************************************************
// Project:  AsyncTask
// File:     IMessageEventArgs.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace AsyncTask.Interfaces;

public interface IMessageEventArgs
{
    string     Message   { get; set; }
    Exception? Exception { get; set; }
}
// *****************************************************************************
// File:       Message.cs
// Solution:   ORM-Monitor
// Project:    ORM-Monitor
// Date:       08/22/2020
// Author:     Latency McLaughlin
// Copywrite:  Bio-Hazard Industries - 1998-2020
// *****************************************************************************

using System;

namespace ORM_Monitor.EventArgs
{
    public class MessageEventArgs : System.EventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
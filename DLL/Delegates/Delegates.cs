// ****************************************************************************
// Project:  AsyncTask
// File:     Delegates.cs
// Author:   Latency McLaughlin
// Date:     04/23/2021
// ****************************************************************************

using System;

namespace AsyncTask.Delegates
{
    public class Delegates
    {
        public delegate void LogHandler(string msg, Exception ex = null);
    }
}
//  *****************************************************************************
//  File:       IRunningEvent.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;

namespace ORM_Monitor.Interfaces {
  public interface IRunningEvent<out T> : ITaskHandler<T> {
    event EventHandler<TaskEventArgs> OnRunning;
    T Invoke();
  }
}
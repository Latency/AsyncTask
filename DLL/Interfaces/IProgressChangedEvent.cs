//  *****************************************************************************
//  File:       IProgressChangedEvent.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/09/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;

namespace ORM_Monitor.Interfaces {
  public interface IProgressChangedEvent<T> : ITaskHandler {
    event EventHandler<TaskEventArgs<T>> OnProgressChanged;
  }
}
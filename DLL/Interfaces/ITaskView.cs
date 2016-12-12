//  *****************************************************************************
//  File:       ITaskView.cs
//  Solution:   ORM-Monitor
//  Project:    ORM-Monitor
//  Date:       11/09/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

namespace ORM_Monitor.Interfaces {
  public interface ITaskView {
    TaskEvent<T> Run<T>(TaskEventArgs.Expression expression, params dynamic[] arguments);
  }
}
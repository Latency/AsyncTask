//  *****************************************************************************
//  File:       ITaskHandler.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

namespace ORM_Monitor.Interfaces {
  public interface ITaskHandler<out T> {
    bool IsSubscribed { get; }
  }
}
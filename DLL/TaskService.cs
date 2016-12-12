//  *****************************************************************************
//  File:       TaskService.cs
//  Solution:   ORM-Monitor
//  Project:    DLL
//  Date:       11/04/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading.Tasks;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor {
  /// <summary>
  ///   ServiceTask
  /// </summary>
  public class TaskService : ITaskService {
    /// <summary>
    ///   Copy Constructor
    /// </summary>
    /// <param name="view"></param>
    /// <param name="taskName"></param>
    /// <param name="description"></param>
    /// <param name="imageName"></param>
    /// <param name="priority"></param>
    /// <param name="date"></param>
    /// <param name="progress"></param>
    public TaskService(dynamic view, string taskName, string description, string imageName, int priority, DateTime? date = null, ushort progress = 0) {
      View = view;
      TaskName = taskName;
      ImageName = imageName;
      Date = date ?? DateTime.Now;
      Description = description;
      Priority = priority;
      Progress = progress;
    }

    #region Properties

    // -----------------------------------------------------------------------

    /// <summary>
    ///   View
    /// </summary>
    public dynamic View { get; set; }

    /// <summary>
    ///   Index
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    ///   TaskName
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    ///   Task
    /// </summary>
    public Task Task { get; set; }

    /// <summary>
    ///   Event
    /// </summary>
    public TaskEvent<dynamic> Event { get; set; }

    /// <summary>
    ///   Date
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    ///   ImageName
    /// </summary>
    public string ImageName { get; set; }

    /// <summary>
    ///   Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///   Priority
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    ///   Progress
    /// </summary>
    public int Progress { get; set; }

    // -----------------------------------------------------------------------

    #endregion Properties
  }
}
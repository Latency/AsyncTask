//  *****************************************************************************
//  File:      ServiceTask.cs
//  Solution:  ORM-Monitor
//  Project:   DLL
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading.Tasks;

namespace ORM_Monitor.Models {
  /// <summary>
  ///   ServiceTask
  /// </summary>
  public class ServiceTask : IServiceTask {
    /// <summary>
    ///   Copy Constructor
    /// </summary>
    /// <param name="view"></param>
    /// <param name="task"></param>
    /// <param name="description"></param>
    /// <param name="imageName"></param>
    /// <param name="status"></param>
    /// <param name="priority"></param>
    /// <param name="date"></param>
    /// <param name="progress"></param>
    public ServiceTask(dynamic view, string task, string description, string imageName, TaskStatus status, int priority,
      DateTime? date = null, ushort progress = 0) {
      View = view;
      Task = task;
      ImageName = imageName;
      Date = date ?? DateTime.Now;
      Description = description;
      Status = status;
      Priority = priority;
      Progress = progress;
    }


    /// <summary>
    ///   NextAction
    /// </summary>
    public string NextAction {
      get {
        switch (Status) {
          case TaskStatus.Running:
            return "Cancel";
          case TaskStatus.WaitingForChildrenToComplete:
            return "Stopping";
          case TaskStatus.WaitingToRun:
            return "Start";
          case TaskStatus.RanToCompletion:
          case TaskStatus.Canceled:
            return "Remove";
          default:
            return "[unknown]";
        }
      }
    }


    /// <summary>
    ///   AdvanceToNextState
    /// </summary>
    public void AdvanceToNextState() {
      switch (Status) {
        case TaskStatus.Created:
          Status = TaskStatus.WaitingToRun;
          break;
        case TaskStatus.WaitingToRun:
          Status = TaskStatus.Running;
          break;
        case TaskStatus.Running:
          Status = TaskStatus.RanToCompletion;
          break;
        case TaskStatus.RanToCompletion:
          Status = TaskStatus.Canceled;
          break;
        case TaskStatus.Canceled:
          Status = TaskStatus.WaitingForChildrenToComplete;
          break;
        case TaskStatus.WaitingForChildrenToComplete:
          Status = TaskStatus.Faulted;
          break;
        case TaskStatus.Faulted:
          Status = TaskStatus.WaitingForActivation;
          break;
        case TaskStatus.WaitingForActivation:
          Status = TaskStatus.Created;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
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
    ///   Scheduler
    /// </summary>
    public ITaskScheduler Scheduler { get; set; }

    /// <summary>
    ///   Task
    /// </summary>
    public string Task { get; set; }

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
    ///   Status
    /// </summary>
    public TaskStatus Status { get; set; }

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
//  *****************************************************************************
//  File:       TaskRecordSet.cs
//  Solution:   ORM-Monitor
//  Project:    ~GUI2
//  Date:       02/21/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ORM_Monitor.Models {
  public class TaskRecordSet : TaskRecordSetBase, ITaskRecordSet {
    /// <summary>
    ///   Copy Constructor
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="description"></param>
    /// <param name="priority"></param>
    /// <param name="date"></param>
    /// <param name="progress"></param>
    public TaskRecordSet(string taskName, string description, int priority, DateTime? date = null, ushort progress = 0) {
      TaskName = taskName;
      Date = date ?? DateTime.Now;
      Description = description;
      Priority = priority;
      Progress = progress;
    }


    /// <summary>
    ///  Copy Constructor
    /// </summary>
    /// <param name="rst"></param>
    protected TaskRecordSet(ITaskRecordSet rst) : this(rst.TaskName, rst.Description, rst.Priority, rst.Date, rst.Progress) {}


    #region Properties
    // -----------------------------------------------------------------------

    /// <summary>
    ///   TaskName
    /// </summary>
    [GridColumn(Header = "Task", Name = "ColTask", Width = 80, IsReadOnly = true)]
    public string TaskName {
      get { return Get<string>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Description
    /// </summary>
    [GridColumn(Name = "ColDescription", Width = 280, IsReadOnly = true, Visibility = Visibility.Collapsed)]
    public string Description {
      get { return Get<string>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Priority
    /// </summary>
    [GridColumn(Name = "ColPriority", Width = 50, IsReadOnly = true)]
    public int Priority {
      get { return Get<int>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Progress
    /// </summary>
    [GridColumn(Name = "ColProgress", Width = 60, IsReadOnly = true)]
    public ushort Progress {
      get { return Get<ushort>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Status
    /// </summary>
    [GridColumn(Name = "ColStatus", Width = 100, IsReadOnly = true)]
    public TaskStatus Status {
      get { return Get<TaskStatus>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Action
    /// </summary>
    [GridColumn(Name = "ColAction", Width = 100, IsReadOnly = true)]
    public Button Action {
      get { return Get<Button>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Date
    /// </summary>
    [GridColumn(Name = "ColDate", Width = 120, IsReadOnly = true)]
    public DateTime? Date {
      get { return Get<DateTime?>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }

    /// <summary>
    ///   Tag
    /// </summary>
    [GridColumn(IsReadOnly = true, Visibility = Visibility.Collapsed)]
    public object Tag {
      get { return Get<object>(MethodBase.GetCurrentMethod().Name.Substring(4)); }
      set { Set(MethodBase.GetCurrentMethod().Name.Substring(4), value); }
    }
    // -----------------------------------------------------------------------
    #endregion Properties
  }
}
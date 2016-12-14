//  *****************************************************************************
//  File:      V_TaskList.Designer.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using BrightIdeasSoftware;
using GUI.Models;
using GUI.WinForms;

namespace GUI.Views {
  internal sealed partial class V_TaskList {
    #region Properties
    // -----------------------------------------------------------------------

    private RadForm1 Context { get; }
    private ObjectListView Controller { get; }
    private ITaskService Service { get; }

    // -----------------------------------------------------------------------
    #endregion Properties
  }
}

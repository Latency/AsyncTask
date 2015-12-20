//  *****************************************************************************
//  File:      RadForm1.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using GUI.Views;
using ORM_Monitor;
using ORM_Monitor.Models;
using ReflectSoftware.Insight.Common;
using Telerik.WinControls.UI;

namespace GUI.WinForms {
  internal sealed partial class RadForm1 {
    /// <summary>
    ///   Static constructor
    /// </summary>
    static RadForm1() {
      Images = new Dictionary<string, Image>();
      var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
      if (declaringType == null) {
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name,
          new NullReferenceException(nameof(declaringType)));
      }

      try {
        Images.Add("Error", Bitmap.FromHicon(SystemIcons.Error.Handle));
        Images.Add("WaitingForChildrenToComplete", Bitmap.FromHicon(SystemIcons.Hand.Handle));
        Images.Add("Canceled", Bitmap.FromHicon(SystemIcons.Warning.Handle));
      }
      catch (Exception ex) {
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, ex);
      }
    }


    /// <summary>
    ///   RadForm1
    /// </summary>
    public RadForm1() {
      _scheduler = new Scheduler();
      Text = @"Task Scheduler";

      // UIContext designer
      InitializeComponent();

      // Embed panel.
      _taskObj = new TabDescribedTask(this) {
        Dock = DockStyle.Fill
      };
      Controls.Add(_taskObj);
      _taskObj.olvTasks.ItemsAdding += OlvTasksOnItemsAdding;
    }

    #region Private Fields

    // -----------------------------------------------------------------------

    private static readonly Dictionary<string, Image> Images;
    private Scheduler _scheduler;
    private readonly TabDescribedTask _taskObj;

    // -----------------------------------------------------------------------

    #endregion Private Fields

    #region Event Handlers

    //------------------------------------------------------------------------

    /// <summary>
    ///   OnListViewAdding
    /// </summary>
    private void OlvTasksOnItemsAdding(object sender, ItemsAddingEventArgs itemsAddingEventArgs) {
      var olv = sender as ObjectListView;
      if (olv == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(olv)));

      foreach (ServiceTask st in itemsAddingEventArgs.ObjectsToAdd) {
        st.Index = olv.GetItemCount();
        _scheduler.RunTask(st.View = new V_StatusBar(olv, st));
      }
    }


    /// <summary>
    ///   RadForm1_Resize
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadForm1_Resize(object sender, EventArgs e) {
      var pnl = _taskObj.Controls[0] as RadCollapsiblePanel;
      if (pnl != null) {
        _taskObj.Controls[1].Size = new Size(ClientSize.Width - (pnl.IsExpanded ? pnl.Width : 20),
          ClientSize.Height - radPanel_Bottom.Height);
      }
    }


    //------------------------------------------------------------------------

    #endregion Event Handlers
  }
}
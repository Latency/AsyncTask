//  *****************************************************************************
//  File:       RadForm1.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       11/06/2016
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using GUI.Views;
using ORM_Monitor;
using ReflectSoftware.Insight;
using ReflectSoftware.Insight.Common;
using Telerik.WinControls.UI;

namespace GUI.WinForms {
  internal sealed partial class RadForm1 {
    #region Private Fields
    // -----------------------------------------------------------------------

    /// <summary>
    ///  Concurrent llist container.
    /// </summary>
    internal volatile Dictionary<string, dynamic> RunningTasks = new Dictionary<string, dynamic>();

    /// <summary>
    ///  Panel
    /// </summary>
    private readonly TabDescribedTask _taskObj;

    // -----------------------------------------------------------------------
    #endregion Private Fields


    /// <summary>
    ///   Static constructor
    /// </summary>
    static RadForm1() {
      var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
      if (declaringType == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(declaringType)));

      try {
        var images = new Dictionary<string, Image> {
          {
            "Error", Bitmap.FromHicon(SystemIcons.Error.Handle)
          }, {
            "WaitingForChildrenToComplete", Bitmap.FromHicon(SystemIcons.Hand.Handle)
          }, {
            "Canceled", Bitmap.FromHicon(SystemIcons.Warning.Handle)
          }
        };
      } catch (Exception ex) {
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, ex);
      }
    }


    /// <summary>
    ///   RadForm1
    /// </summary>
    public RadForm1() {
      Text = @"Task Scheduler";

      // UIContext designer
      InitializeComponent();

      // Embed panel.
      _taskObj = new TabDescribedTask(this) {
        Dock = DockStyle.Fill
      };
      _taskObj.olvTasks.ItemsAdding += OlvTasksOnItemsAdding;
      Controls.Add(_taskObj);
    }


    #region Event Handlers
    //------------------------------------------------------------------------


    /// <summary>
    ///   OnListViewAdding
    /// </summary>
    private void OlvTasksOnItemsAdding(object sender, ItemsAddingEventArgs itemsAddingEventArgs) {
      var olv = sender as ObjectListView;
      if (olv == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(olv)));

      TaskEventArgs.Expression action = args => {
        var obj = args[0];
        var str = args[1] as string;
        var testNo = obj != null ? $"{((TaskEvent<dynamic>)obj).Name}: " : string.Empty;
        RILogManager.Default.SendInformation(testNo + str);
      };

      foreach (TaskService st in itemsAddingEventArgs.ObjectsToAdd) {
        st.Index = olv.GetItemCount();
        st.View = new V_StatusBar(this, olv, st);
        st.Event = st.View.Run<dynamic>(action, this);
        try {
          RunningTasks.Add(st.TaskName, st.Event);
          st.Task = st.Event.AsyncMonitor();
        } catch (Exception ex) {
          RILogManager.Default.SendError(ex.Message);
        }
      }
    }


    /// <summary>
    ///   RadForm1_Resize
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadForm1_Resize(object sender, EventArgs e) {
      var pnl = _taskObj.Controls[0] as RadCollapsiblePanel;
      if (pnl != null)
        _taskObj.Controls[1].Size = new Size(ClientSize.Width - (pnl.IsExpanded ? pnl.Width : 20), ClientSize.Height - radPanel_Bottom.Height);
    }


    /// <summary>
    ///  RadForm1_FormClosed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadForm1_FormClosed(object sender, FormClosedEventArgs e) {
      // Cancel Applife online updating.
      foreach (var t in RunningTasks.Where(t => !t.Value.IsDisposed))
        t.Value.TokenSource.Cancel();
    }


    //------------------------------------------------------------------------
    #endregion Event Handlers
  }
}
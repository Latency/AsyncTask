//  *****************************************************************************
//  File:      TabDescribedTask.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using GUI.Views;
using ORM_Monitor;
using ORM_Monitor.Models;
using ReflectSoftware.Insight.Common;

namespace GUI.WinForms {
  internal sealed partial class TabDescribedTask {
    // ReSharper disable once InconsistentNaming
    private readonly IForm _GUIContext;
    private string _prefixForNextSelectionMessage;


    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="ctx"></param>
    public TabDescribedTask(IForm ctx) {
      InitializeComponent();

      _GUIContext = ctx;

      SetupColumns();

      olvTasks.ButtonClick += RemoveButton_Click;
      olvTasks.SelectionChanged += delegate { HandleSelectionChanged(olvTasks); };
      olvTasks.HotItemChanged += HandleHotItemChanged;
      olvTasks.GroupTaskClicked +=
        delegate(object sender, GroupTaskClickedEventArgs args) {
          ShowMessage("Clicked on group task: " + args.Group.Name);
        };
      olvTasks.GroupStateChanged +=
        delegate(object sender, GroupStateChangedEventArgs e) {
          Debug.WriteLine("Group '{0}' was {1}{2}{3}{4}{5}{6}", e.Group.Header, e.Selected ? "Selected" : "",
            e.Focused ? "Focused" : "", e.Collapsed ? "Collapsed" : "", e.Unselected ? "Unselected" : "",
            e.Unfocused ? "Unfocused" : "", e.Uncollapsed ? "Uncollapsed" : "");
        };
    }


    /// <summary>
    ///   HandleSelectionChanged
    /// </summary>
    /// <param name="listView"></param>
    private void HandleSelectionChanged(ObjectListView listView) {
      var p = listView.SelectedObject as IServiceTask;
      var msg = p == null ? listView.SelectedIndices.Count.ToString(CultureInfo.CurrentCulture) : $"'{p.Task}'";
      var focused = listView.FocusedObject as IServiceTask;
      var focusedMsg = focused == null ? "" : $". Focused on '{focused.Task}'";
      _GUIContext.StatusBar.Text = string.IsNullOrEmpty(_prefixForNextSelectionMessage)
        ? $"Selected {msg} of {listView.GetItemCount()} items{focusedMsg}"
        : $"{_prefixForNextSelectionMessage}. Selected {msg} of {listView.GetItemCount()} items{focusedMsg}";
      _prefixForNextSelectionMessage = null;
    }


    /// <summary>
    ///   ShowMessage
    /// </summary>
    /// <param name="message"></param>
    private static void ShowMessage(string message) {
      MessageBox.Show(message, @"Object List View", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    /// <summary>
    ///   HandleHotItemChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleHotItemChanged(object sender, HotItemChangedEventArgs e) {
      if (sender == null) {
        _GUIContext.StatusControl.Text = "";
        return;
      }

      switch (e.HotCellHitLocation) {
        case HitTestLocation.Nothing:
          _GUIContext.StatusControl.Text = @"Over nothing";
          break;
        case HitTestLocation.Header:
        case HitTestLocation.HeaderCheckBox:
        case HitTestLocation.HeaderDivider:
          _GUIContext.StatusControl.Text = $"Over {e.HotCellHitLocation} of column #{e.HotColumnIndex}";
          break;
        case HitTestLocation.Group:
          _GUIContext.StatusControl.Text = $"Over group '{e.HotGroup.Header}', {e.HotCellHitLocationEx}";
          break;
        case HitTestLocation.GroupExpander:
          _GUIContext.StatusControl.Text = $"Over group expander of '{e.HotGroup.Header}'";
          break;
        default:
          _GUIContext.StatusControl.Text = $"Over {e.HotCellHitLocation} of ({e.HotRowIndex}, {e.HotColumnIndex})";
          break;
      }
    }


    /// <summary>
    ///   SetupColumns
    /// </summary>
    private void SetupColumns() {
      olvColumnTask.Renderer = new DescribedTaskRenderer {
        ImageList = imageListTasks,
        DescriptionAspectName = "Description",
        TitleFont = new Font("Tahoma", 11, FontStyle.Bold),
        DescriptionFont = new Font("Tahoma", 9),
        ImageTextSpace = 8,
        TitleDescriptionSpace = 1,
        UseGdiTextRendering = true,
        TitleColor = Color.DarkBlue,
        DescriptionColor = Color.CornflowerBlue
      };

      // We want the coins to overlap
      olvColumnPriority.Renderer = new MultiImageRenderer("Lamp", 4, 0, 5) {
        Spacing = -12
      };

      olvColumnStatus.AspectToStringConverter = delegate(object model) {
        var status = (TaskStatus) model;
        switch (status) {
          case TaskStatus.WaitingForChildrenToComplete:
            return "Canceling";
          case TaskStatus.WaitingToRun:
            return "Not started";
          case TaskStatus.RanToCompletion:
            return "Complete";
          default:
            return status.ToString();
        }
      };

      olvColumnStatus.ImageGetter = delegate(object model) {
        var task = (ServiceTask) model;
        switch (task.Status) {
          case TaskStatus.Running:
            return "Heart";
          case TaskStatus.WaitingToRun:
            return "Add";
          case TaskStatus.RanToCompletion:
            return "Tick";
          case TaskStatus.Canceled:
            return "Cancel";
          case TaskStatus.WaitingForChildrenToComplete:
            return "Coin";
          case TaskStatus.Faulted:
            return "Lamp";
          default:
            return "";
        }
      };
    }


    /// <summary>
    ///   RebuildFilters
    /// </summary>
    private void RebuildFilters() {
      // Build a composite filter that unify the three possible filtering criteria
      var filters = new List<IModelFilter>();

      if (checkBoxHighPriority.Checked)
        filters.Add(new ModelFilter(model => ((ServiceTask) model).Priority > 3));

      if (checkBoxIncomplete.Checked)
        filters.Add(new ModelFilter(model => ((ServiceTask) model).Status != TaskStatus.RanToCompletion));

      if (!string.IsNullOrEmpty(textBoxFilter.Text))
        filters.Add(new TextMatchFilter(olvTasks, textBoxFilter.Text));

      // Use AdditionalFilter (instead of ModelFilter) since AdditionalFilter plays well with any extra filtering the user might specify via the column header
      olvTasks.AdditionalFilter = filters.Count == 0 ? null : new CompositeAllFilter(filters);
    }


    private void textBoxFilter_TextChanged(object sender, EventArgs e) {
      RebuildFilters();
    }


    private void checkBoxHighPriority_CheckedChanged(object sender, EventArgs e) {
      RebuildFilters();
    }


    private void checkBoxIncomplete_CheckedChanged(object sender, EventArgs e) {
      RebuildFilters();
    }


    /// <summary>
    ///   radCollapsiblePanel1_Expanded
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void radCollapsiblePanel1_Expanded(object sender, EventArgs e) {
      olvTasks.Location = new Point(groupBox9.Size.Width + 37, 0);
      olvTasks.Size = new Size(ClientSize.Width - radCollapsiblePanel1.Width + 1, olvTasks.Size.Height);
    }


    /// <summary>
    ///   radCollapsiblePanel1_Collapsing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void radCollapsiblePanel1_Collapsing(object sender, CancelEventArgs e) {
      olvTasks.Location = new Point(20, 0);
      olvTasks.Size = new Size(ClientSize.Width - 20, olvTasks.Size.Height);
    }


    /// <summary>
    ///   StopButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopButton_Click(object sender, EventArgs e) {
      if (_GUIContext.Scheduler.Cancel())
        _GUIContext.StatusBar.Text += @"Canceling tasks" + Environment.NewLine;
      else
        _GUIContext.StatusBar.Text += @"No tasks are running to be canceled" + Environment.NewLine;
    }


    /// <summary>
    ///   RemoveButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveButton_Click(object sender, CellClickEventArgs e) {
      var st = e.Model as ServiceTask;
      if (st == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("st"));

      _GUIContext.StatusBar.Text = $"Button clicked: ({e.RowIndex}, {e.SubItem}, {e.Model})";

      if (!olvTasks.IsDisabled(e.Model) && st.Status != TaskStatus.RanToCompletion && st.Status != TaskStatus.Canceled) {
        olvTasks.DisableObject(e.Model);

        var task = st.View as V_StatusBar;
        if (task == null || !task.IsBusy || task.CancellationPending)
          return;

        st.Status = TaskStatus.WaitingForChildrenToComplete;

        if (task.WorkerSupportsCancellation) {
          // Cancel the asynchronous operation.
          task.CancelAsync();
        }

        olvTasks.RefreshObject(e.Model);
      }
      else
        olvTasks.RemoveObject(st);
    }


    /// <summary>
    ///   StartButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StartButton_Click(object sender, EventArgs e) {
      olvTasks.AddObject(new ServiceTask(olvTasks, new Random().Next().ToString(),
        "Politely and informatively respond to all tech questions the employees may have", "faq", TaskStatus.Running,
        new Random().Next(0, 5)));
    }


    /// <summary>
    ///   ClearButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClearButton_Click(object sender, EventArgs e) {
      olvTasks.ClearObjects();
    }
  }
}
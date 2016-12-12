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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using ORM_Monitor;
using ORM_Monitor.Interfaces;
using ReflectSoftware.Insight.Common;

namespace GUI.WinForms {
  internal sealed partial class TabDescribedTask {
    // ReSharper disable once InconsistentNaming
    private readonly RadForm1 _GUIContext;
    private string _prefixForNextSelectionMessage;


    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="ctx"></param>
    public TabDescribedTask(RadForm1 ctx) {
      InitializeComponent();
      _GUIContext = ctx;

      SetupColumns();

      olvTasks.ButtonClick += RemoveButton_Click;
      olvTasks.HotItemChanged += HandleHotItemChanged;
      olvTasks.SelectionChanged += (sender, e) => HandleSelectionChanged(olvTasks);
      olvTasks.GroupTaskClicked += (sender, e) => ShowMessage("Clicked on group task: " + e.Group.Name);
      olvTasks.GroupStateChanged += (sender, e) => Debug.WriteLine("Group '{0}' was {1}{2}{3}{4}{5}{6}", e.Group.Header, e.Selected ? "Selected" : "", e.Focused ? "Focused" : "", e.Collapsed ? "Collapsed" : "", e.Unselected ? "Unselected" : "", e.Unfocused ? "Unfocused" : "", e.Uncollapsed ? "Uncollapsed" : "");
    }


    /// <summary>
    ///   HandleSelectionChanged
    /// </summary>
    /// <param name="listView"></param>
    private void HandleSelectionChanged(ObjectListView listView) {
      var p = listView.SelectedObject as ITaskService;
      var msg = p == null ? listView.SelectedIndices.Count.ToString(CultureInfo.CurrentCulture) : $"'{p.TaskName}'";
      var focused = listView.FocusedObject as ITaskService;
      var focusedMsg = focused == null ? "" : $". Focused on '{focused.TaskName}'";
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

      olvColumnStatus.AspectGetter = olvColumnAction.AspectGetter = model => {
        var ts = model as TaskService;
        if (ts == null)
          throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(olvColumnStatus.AspectGetter)));
        return ts.Task.Status;
      };

      olvColumnStatus.AspectToStringConverter = model => {
        var status = TaskStatus.Created;
        if (model != null)
          status = (TaskStatus) model;

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

      olvColumnStatus.ImageGetter = model => {
        var ts = model as TaskService;
        if (ts == null)
          throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(olvColumnStatus.AspectGetter)));

        switch (ts.Task.Status) {
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

      olvColumnAction.AspectToStringConverter = status => {
        switch ((TaskStatus) status) {
          case TaskStatus.Running:
          case TaskStatus.WaitingToRun:
            return "Stop";
          case TaskStatus.RanToCompletion:
          case TaskStatus.Canceled:
          case TaskStatus.Faulted:
            return "Remove";
          case TaskStatus.WaitingForChildrenToComplete:
            return "Canceling";
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
        filters.Add(new ModelFilter(model => ((TaskService) model).Priority > 3));

      if (checkBoxIncomplete.Checked)
        filters.Add(new ModelFilter(model => ((TaskService) model).Task.Status != TaskStatus.RanToCompletion));

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
      _GUIContext.StatusBar.Text = _GUIContext.RunningTasks.Count > 0 ? @"Canceling tasks" : @"No tasks are running to be canceled";
      foreach (var task in _GUIContext.RunningTasks.Select(st => st.Value as TaskEvent<dynamic>)) {
        task?.TokenSource.Cancel();
      }
    }


    /// <summary>
    ///   RemoveButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveButton_Click(object sender, CellClickEventArgs e) {
      var st = e.Model as TaskService;
      if (st == null)
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, new NullReferenceException("st"));

      _GUIContext.StatusBar.Text = $"Button clicked: ({e.RowIndex}, {e.SubItem}, {e.Model})";

      if (!olvTasks.IsDisabled(e.Model) && !st.Task.IsCompleted && !st.Task.IsCanceled && !st.Task.IsFaulted) {
        olvTasks.DisableObject(e.Model);

        st.Event.TokenSource.Cancel();
        st.Task.Wait();

        olvTasks.RefreshObject(e.Model);
      } else
        olvTasks.RemoveObject(st);
    }


    /// <summary>
    ///   StartButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StartButton_Click(object sender, EventArgs e) {
      _GUIContext.StatusBar.Text = $"Starting task{(_GUIContext.RunningTasks.Count > 1 ? "s" : string.Empty)}";
      olvTasks?.AddObject(new TaskService(olvTasks, new Random().Next().ToString(), "Politely and informatively respond to all tech questions the employees may have", "faq", new Random().Next(0, 5)));
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
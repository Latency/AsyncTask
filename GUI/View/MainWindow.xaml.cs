//  *****************************************************************************
//  File:       MainWindow.xaml.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       02/21/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.Logging;
using ORM_Monitor.Controller;
using ORM_Monitor.Enums;
using ORM_Monitor.EventArgs;
using ORM_Monitor.Models;
using Color = System.Drawing.Color;
using ILogger = ORM_Monitor.Interfaces.ILogger;

namespace ORM_Monitor.View {
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow {
    private readonly Mutex _mutex = new Mutex();
    private readonly ILogger _logger;


    /// <summary>
    ///  Constructor
    /// </summary>
    public MainWindow() {
      InitializeComponent();

      // Dynamically create columns in DataGrid from Dependancy property databinding.
      DataBind_Columns(new ObservableCollection<TaskRecordSet>());

      ListView1.DataContext = this;
    
      DataContext = new TaskController();

      _logger.LoggingEvents[LogType.Info] += Info;
      _logger.LoggingEvents[LogType.Warning] += Warning;
      _logger.LoggingEvents[LogType.Error] += Error;
      _logger.LoggingEvents[LogType.Debug] += Debug;
    }


    /// <summary>
    ///   Constructor
    /// </summary>
    /// <param name="logger"></param>
    public MainWindow(ILogger logger) {
      _logger = logger;
    }

        #region Handler Handlers

        //------------------------------------------------------------------------



        public void Info(object sender, MessageEventArgs messageEventArgs)
        {
            TextBoxMessageLog.InvokeIfRequired(a =>
            {
                if (!(a is MessageEventArgs args))
                    throw new NullReferenceException();
                TextBoxMessageLog.SelectionColor = Color.DodgerBlue;
                TextBoxMessageLog.AppendText(args.Message);
            }, messageEventArgs, new EventHandler<MessageEventArgs>(Info), sender, messageEventArgs);
        }


        public void Warning(object sender, MessageEventArgs messageEventArgs)
        {
            TextBoxMessageLog.InvokeIfRequired(a =>
            {
                if (!(a is MessageEventArgs args))
                    throw new NullReferenceException();
                TextBoxMessageLog.SelectionColor = Color.Yellow;
                TextBoxMessageLog.AppendText(args.Message);
            }, messageEventArgs, new EventHandler<MessageEventArgs>(Warning), sender, messageEventArgs);
        }


        public void Error(object sender, MessageEventArgs messageEventArgs)
        {
            TextBoxMessageLog.InvokeIfRequired(a =>
            {
                if (!(a is MessageEventArgs args))
                    throw new NullReferenceException();
                TextBoxMessageLog.SelectionColor = Color.Red;
                TextBoxMessageLog.AppendText(args.Message);
                if (args.Exception != null)
                {
                    TextBoxMessageLog.SelectionColor = Color.Gray;
                    TextBoxMessageLog.AppendText(args.Exception.Message);
                }
            }, messageEventArgs, new EventHandler<MessageEventArgs>(Error), sender, messageEventArgs);
        }


        public void Debug(object sender, MessageEventArgs messageEventArgs)
        {
            TextBoxMessageLog.InvokeIfRequired(a =>
            {
                if (!(a is MessageEventArgs args))
                    throw new NullReferenceException();
                TextBoxMessageLog.SelectionColor = Color.DarkOrange;
                TextBoxMessageLog.AppendText(args.Message);
            }, messageEventArgs, new EventHandler<MessageEventArgs>(Debug), sender, messageEventArgs);
        }


        /// <summary>
        ///   RadForm1_FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, System.EventArgs e) {
      if (!(DataContext is TaskController dc))
        return;
      dc.Dispose();
      DataContext = null;
    }


    /// <summary>
    ///   RemoveButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveButton_Click(object sender, RoutedEventArgs e) {
      if (!(sender is Button btn))
        throw new NullReferenceException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(btn)));

      // Traverse up the VisualTree and find the DataGridRow.
      var row = Extensions.FindAncestorOrSelf<DataGridRow>(btn);
      if (row == null)
        throw new NullReferenceException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(row)));

      var index = row.GetIndex();

      var rst = ListView1.Items[index] as TaskRecordSet;

      if (!(rst?.Tag is TaskService st))
        return;

      lblStatusBar.Text = $"Button clicked: (Row: {index + 1}, Action: {btn.Content})";

      if (btn.IsEnabled && rst.Status == TaskStatus.Running) {
        try {
          st.Event.TokenSource.Cancel();
          btn.IsEnabled = false;
        } catch (Exception ex) {
          Debug.WriteLine(ex.Message);
        }
      } else {
        lock (_mutex) {
          ListView1.Items.RemoveAt(index);
        }
      }
    }


    /// <summary>
    ///   ActionCollection
    /// </summary>
    /// <returns></returns>
    private static List<Delegate> ActionCollection() {
      return new List<Delegate>(
        new Delegate[] {
          (Action<string,string>)  ((name, status) =>         _logger.Log(LogLevel.Information, new EventId(), ); ($"@{name}: {status}")),
        }
      );
    }


    /// <summary>
    ///   StartButton_MouseDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StartButton_Click(object sender, RoutedEventArgs e) {
      if (!(DataContext is TaskController dc))
        return;

      try {
        var rst = new TaskRecordSet(taskName: new Random().Next().ToString(), description: "Politely and informatively respond to all tech questions the employees may have.", priority: new Random().Next(0, 5));

        lblStatusBar.Text = $"Starting task \"{rst.TaskName}\".";

        lock (_mutex) {
          var idx = ListView1.Items.Add(rst);
          ListView1.UpdateLayout();

          var dgr = ListView1.ItemContainerGenerator.ContainerFromIndex(idx) as DataGridRow;
          var st = new TaskService(this, rst, dc, dgr);
          rst.Tag = st;

          var a = Extensions.FindFirstChild<DataGridCellsPanel>(dgr);
          foreach (DataGridCell b in a.Children) {
            b.Name = b.Column.Header.ToString();
            if (b.Name != "Action")
              continue;
            var btn = Extensions.FindFirstChild<Button>(b.Content as FrameworkElement);
            rst.Action = btn;
          }

          st.Event = st.Controller.Run();
          st.Event.Name = "Task Scheduler";
          st.Event.Tag = rst;
          st.Event.Expression = ActionCollection();

          dc.RunningTasks.Add(rst.TaskName, st.Event);

          // Run the task asynchronously wrapped with the monitor.
          st.Event.AsyncMonitor();
        }
      } catch (Exception ex) {
        _logger.Log(LogLevel.Error, new EventId(), ex, ex.Message);
      }
    }


    /// <summary>
    ///   StopButton_MouseDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopButton_Click(object sender, RoutedEventArgs e) {
      if (!(DataContext is TaskController dc))
        return;

      lblStatusBar.Text = dc.RunningTasks.Count > 0 ? @"Canceling tasks." : @"No tasks are running to be canceled.";
      foreach (var task in dc.RunningTasks.Select(st => st.Value))
        task?.TokenSource.Cancel();
    }


    /// <summary>
    ///   ClearButton_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClearButton_Click(object sender, RoutedEventArgs e) {
      if (ListView1.Items.Count > 1) {
        lblStatusBar.Text = @"Clearing task list.";
        lock (_mutex) {
          ListView1.Items.Clear();
        }
      } else
        lblStatusBar.Text = @"Task list has already been cleared.";
    }


    /// <summary>
    ///   ListView_SelectionChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      var msg = new StringBuilder();
      
      foreach (TaskRecordSet item in ListView1.SelectedItems) {
        msg.Append($"{(msg.Length > 0 ? ", " : string.Empty)}{item.TaskName}");
      }
      if (msg.Length <= 0)
        return;
      lblStatusBar.Text = $"Selected '{msg}' of {ListView1.SelectedItems.Count} item{(ListView1.SelectedItems.Count != 1 ? "s" : string.Empty)}.";
    }


    /// <summary>
    ///   ListView1_MouseMove
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal void ListView1_MouseEnter(object sender, MouseEventArgs e) {
      if (ListView1.Items.Count <= 0)
        return;

      // Callback to return the result of the hit test.
      // ReSharper disable once ConvertToLocalFunction
      HitTestResultCallback myHitTestResult = result => {
        var hit = result.VisualHit;

        if (hit is Border)
          hit = Extensions.FindAncestorOrSelf<Button>(hit);

        if (!(hit is TextBlock || hit is Button))
          return HitTestResultBehavior.Stop;

        // Traverse up the VisualTree and find the DataGridCell.
        var col = Extensions.FindAncestorOrSelf<DataGridCell>(hit);

        // Traverse up the VisualTree and find the DataGridRow.
        var row = Extensions.FindAncestorOrSelf<DataGridRow>(hit);

        lblCursorPosition.Text = $"Over {col.Column.Header} at (Row: {row.GetIndex() + 1}, Col: {col.Column.DisplayIndex + 1})";

        // Set the behavior to return visuals at all z-order levels.
        return HitTestResultBehavior.Continue;
      };

      // Set up a callback to receive the hit test result enumeration.
      VisualTreeHelper.HitTest((Visual)sender, null, myHitTestResult, new PointHitTestParameters(e.GetPosition((UIElement)sender)));
    }


    /// <summary>
    ///  ListView1_MouseLeave
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListView1_MouseLeave(object sender, MouseEventArgs e) {
      lblCursorPosition.Text = string.Empty;
    }


    /// <summary>
    ///  MyButton_MouseEnter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MyButton_MouseEnter(object sender, MouseEventArgs e) {
      if (!(sender is Button btn))
        return;

      ListView1_MouseEnter(sender, e);

      try {
        Mouse.OverrideCursor = Cursors.Hand;
        btn.Background = new ImageBrush {
          ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Button-Hover.bmp"))
        };
      } catch (FileNotFoundException) {
      }
    }


    /// <summary>
    ///  MyButton_MouseLeave
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MyButton_MouseLeave(object sender, MouseEventArgs e) {
      if (!(sender is Button btn))
        return;

      ListView1_MouseLeave(sender, e);

      try {
        Mouse.OverrideCursor = Cursors.Arrow;
        btn.Background = new ImageBrush {
          ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Button-Normal.bmp"))
        };
      } catch (FileNotFoundException) {
      }
    }


    /// <summary>
    ///  MyButton_MouseDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MyButton_MouseDown(object sender, MouseButtonEventArgs e) {
      if (!(sender is Button btn))
        return;

      try {
        btn.Background = new ImageBrush {
          ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Button-Pressed.bmp"))
        };
      } catch (FileNotFoundException) {
      }
    }

    //------------------------------------------------------------------------
    #endregion Handler Handlers
  }
}
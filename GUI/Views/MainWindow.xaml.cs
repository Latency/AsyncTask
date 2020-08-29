// ****************************************************************************
// Project:  GUI
// File:     MainWindow.xaml.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ORM_Monitor.Extensions;
using ORM_Monitor.Models;

namespace ORM_Monitor.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Dynamically create columns in DataGrid from Dependancy property databinding.
            DataBind_Columns(new ObservableCollection<TaskRecordSet>());

            ListView1.DataContext = this;
        }
        

        /// <summary>
        ///     RadForm1_FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            DataContext = null;
        }


        /// <summary>
        ///     RemoveButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn))
                throw new NullReferenceException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(btn)));

            // Traverse up the VisualTree and find the DataGridRow.
            var row = Extensions.Extensions.FindAncestorOrSelf<DataGridRow>(btn);
            if (row == null)
                throw new NullReferenceException(MethodBase.GetCurrentMethod().Name, new NullReferenceException(nameof(row)));

            var index = row.GetIndex();

            var rst = ListView1.Items[index] as TaskRecordSet;

            if (!(rst?.Tag is TaskService st))
                return;

            lblStatusBar.Text = $"Button clicked: (Row: {index + 1}, Action: {btn.Content})";

            if (btn.IsEnabled && rst.Status == TaskStatus.Running)
                try
                {
                    st.Task.Cancel();
                    btn.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            else
                lock (ListView1.Items)
                {
                    ListView1.Items.RemoveAt(index);
                }
        }


        /// <summary>
        ///     StartButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rst = new Tasks.AsyncTask
                {
                    TaskInfo = new TaskRecordSet
                    {
                        ID = new Random().Next(),
                        Name = ToString()
                    },
                    Delegate = async (asyncTask, args) =>
                    {
                        double current;
                        var ts = args.TaskInfo;
                        var targetTime = DateTime.Now.AddSeconds(3);
                        var currentTime = DateTime.Now;
                        var diffTime = targetTime.Subtract(currentTime).TotalMilliseconds;
                        var service = ts.Tag as TaskService;

                        service?.Owner.Dispatcher.Invoke(() =>
                        {
                            ts.Action.Content = "Stop";
                            ts.Status = TaskStatus.Running;
                        });

                        while ((current = targetTime.Subtract(currentTime).TotalMilliseconds) > 0)
                        {
                            if (args.CancellationTokenSource.IsCancellationRequested)
                                return;

                            var val = 1 - current / diffTime;

                            service?.Owner.Dispatcher.Invoke(() => { ts.Progress = (ushort)(val * 100); });

                            // Pulse 10x per second.
                            await Task.Delay(200);

                            currentTime = DateTime.Now;
                        }

                        service?.Owner.Dispatcher.Invoke(() => { ts.Progress = 100; });
                    },
                    OnAdd = (asyncTask, args) =>
                    {
                        lblStatusBar.InvokeIfRequired(xx => lblStatusBar.Text = $"Starting task \"{args.TaskInfo.Name}\".", DispatcherPriority.Normal, sender, args);

                        var idx = ListView1.Items.Add(args);
                        ListView1.UpdateLayout();

                        var dgr = ListView1.ItemContainerGenerator.ContainerFromIndex(idx) as DataGridRow;
                        var st = new TaskService
                        {
                            Owner = this,
                            TaskInfo = args.TaskInfo,
                            GridRow = dgr,
                            Task = asyncTask
                        };
                        args.TaskInfo.Tag = st;

                        var a = Extensions.Extensions.FindFirstChild<DataGridCellsPanel>(dgr);
                        foreach (DataGridCell b in a.Children)
                        {
                            b.Name = b.Column.Header.ToString();
                            if (b.Name != "Action")
                                continue;
                            var btn = Extensions.Extensions.FindFirstChild<Button>(b.Content as FrameworkElement);
                            args.TaskInfo.Action = btn;
                        }
                    },
                    OnComplete = (asyncTask, args) =>
                    {
                        var ts = args.TaskInfo;
                        var service = ts.Tag as TaskService;
                        service?.Owner.Dispatcher.Invoke(() =>
                        {
                            ts.Action.Content = "Remove";
                            ts.Progress = 100;
                            ts.Status = TaskStatus.RanToCompletion;
                        });
                    },
                    OnTimeout = (asyncTask, args) =>
                    {
                        var ts = args.TaskInfo;
                        var service = ts.Tag as TaskService;
                        service?.Owner.Dispatcher.Invoke(() =>
                        {
                            ts.Action.IsEnabled = true;
                            ts.Action.Content = "Remove";
                            ts.Status = TaskStatus.Faulted;
                        });
                    },
                    OnCanceled = (asyncTask, args) =>
                    {
                        var service = args.TaskInfo.Tag as TaskService;
                        service?.Owner.Dispatcher.Invoke(() =>
                        {
                            args.TaskInfo.Status = TaskStatus.Canceled;
                            args.TaskInfo.Action.IsEnabled = true;
                            args.TaskInfo.Action.Content = "Remove";
                        });
                    }
                };

                // Run the task asynchronously wrapped with the monitor.
                rst.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        /// <summary>
        ///     StopButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Tasks.AsyncTask task))
                throw new NullReferenceException();

            task.Cancel();
        }


        /// <summary>
        ///     ClearButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListView1.Items.Count > 1)
            {
                lblStatusBar.Text = @"Clearing task list.";
                lock (ListView1.Items)
                {
                    ListView1.Items.Clear();
                }
            }
            else
            {
                lblStatusBar.Text = @"Task list has already been cleared.";
            }
        }


        /// <summary>
        ///     ListView_SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var msg = new StringBuilder();

            foreach (TaskRecordSet item in ListView1.SelectedItems)
                msg.Append($"{(msg.Length > 0 ? ", " : string.Empty)}{item.Name}");
            if (msg.Length <= 0)
                return;
            lblStatusBar.Text = $"Selected '{msg}' of {ListView1.SelectedItems.Count} item{(ListView1.SelectedItems.Count != 1 ? "s" : string.Empty)}.";
        }


        /// <summary>
        ///     ListView1_MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void ListView1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ListView1.Items.Count <= 0)
                return;

            // Callback to return the result of the hit test.
            // ReSharper disable once ConvertToLocalFunction
            HitTestResultCallback myHitTestResult = result =>
            {
                var hit = result.VisualHit;

                if (hit is Border)
                    hit = Extensions.Extensions.FindAncestorOrSelf<Button>(hit);

                if (!(hit is TextBlock || hit is Button))
                    return HitTestResultBehavior.Stop;

                // Traverse up the VisualTree and find the DataGridCell.
                var col = Extensions.Extensions.FindAncestorOrSelf<DataGridCell>(hit);

                // Traverse up the VisualTree and find the DataGridRow.
                var row = Extensions.Extensions.FindAncestorOrSelf<DataGridRow>(hit);

                lblCursorPosition.Text = $"Over {col.Column.Header} at (Row: {row.GetIndex() + 1}, Col: {col.Column.DisplayIndex + 1})";

                // Set the behavior to return visuals at all z-order levels.
                return HitTestResultBehavior.Continue;
            };

            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest((Visual) sender, null, myHitTestResult, new PointHitTestParameters(e.GetPosition((UIElement) sender)));
        }


        /// <summary>
        ///     ListView1_MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView1_MouseLeave(object sender, MouseEventArgs e) => lblCursorPosition.Text = string.Empty;


        /// <summary>
        ///     MyButton_MouseEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            ListView1_MouseEnter(sender, e);

            try
            {
                Mouse.OverrideCursor = Cursors.Hand;
                btn.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Button-Hover.bmp"))
                };
            }
            catch (FileNotFoundException)
            { }
        }


        /// <summary>
        ///     MyButton_MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            ListView1_MouseLeave(sender, e);

            try
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                btn.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Button-Normal.bmp"))
                };
            }
            catch (FileNotFoundException)
            { }
        }


        /// <summary>
        ///     MyButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            try
            {
                btn.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Button-Pressed.bmp"))
                };
            }
            catch (FileNotFoundException)
            { }
        }
    }
}
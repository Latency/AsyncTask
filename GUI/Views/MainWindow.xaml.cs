// ****************************************************************************
// Project:  GUI
// File:     MainWindow.xaml.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AsyncTask.DTO;
using AsyncTask.EventArgs;
using ORM_Monitor.Models;

namespace ORM_Monitor.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Dispatcher _dispatcher;


        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
            
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
                throw new NullReferenceException();

            if (!(btn.Tag is (Tasks.AsyncTask asyncTask, TaskEventArgs<TaskRecordSet, TaskList> args)))
                throw new NullReferenceException();

            _dispatcher.Invoke(() =>
            {
                var index = args.TaskInfo.GridRow.GetIndex();
                lblStatusBar.Text = $"Button clicked: (Row: {index + 1}, Action: {btn.Content})";

                if (btn.IsEnabled && args.Task.Status == TaskStatus.Running)
                    try
                    {
                        asyncTask.Cancel();
                        MyButton_MouseDown(sender, null);
                        btn.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                else
                    ListView1.Items.RemoveAt(index);
            });
        }


        /// <summary>
        ///     StartButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new Tasks.AsyncTask
            {
                TaskInfo = new TaskRecordSet
                {
                    Owner = this
                },
                Delegate = (asyncTask, args) =>
                {
                    while (!asyncTask.IsCancellationRequested && args.TaskInfo.Progress < 100)
                    {
                        args.TaskInfo.Progress += 1;

                        // Pulse
                        Thread.Sleep(100);
                    }
                    if (!asyncTask.IsCancellationRequested)
                        asyncTask.Cancel();
                },
                OnAdd = (asyncTask, args) =>
                {
                    _dispatcher.Invoke(() =>
                    {
                        var rst = args.TaskInfo;
                        rst.Tag = (asyncTask, args);

                        lblStatusBar.Text = $"Starting task \"{rst.Name}\".";
                        var index = ListView1.Items.Add(rst);
                        ListView1.UpdateLayout();

                        if (!(ListView1.ItemContainerGenerator.ContainerFromIndex(index) is DataGridRow rowContainer))
                            throw new NullReferenceException();

                        rst.GridRow = rowContainer;

                        var a = Extensions.Extensions.FindFirstChild<DataGridCellsPanel>(rowContainer);
                        foreach (DataGridCell b in a.Children)
                        {
                            b.Name = b.Column.Header.ToString();
                            switch (b.Name)
                            {
                                case "Action":
                                    var btn = Extensions.Extensions.FindFirstChild<Button>(b.Content as FrameworkElement);
                                    btn.Content = "Stop";
                                    btn.Tag = (asyncTask, args);
                                    btn.Click += RemoveButton_Click;
                                    btn.MouseDown += MyButton_MouseDown;
                                    btn.MouseEnter += MyButton_MouseEnter;
                                    btn.MouseLeave += MyButton_MouseLeave;
                                    args.TaskInfo.Action = btn;
                                    break;
                                case "ID":
                                    rst.ID = new Random().Next();
                                    break;
                                case "Priority":
                                    rst.Priority = new Random().Next(0, 5);
                                    break;
                                case "Date":
                                    rst.Date = DateTime.Now;
                                    break;
                            }
                        }
                    });
                },
                OnComplete = (asyncTask, args) =>
                {
                    _dispatcher.Invoke(() =>
                    {
                        args.TaskInfo.Action.IsEnabled = true;
                        args.TaskInfo.Action.Content = "Remove";
                    });
                },
                OnTimeout = (asyncTask, args) =>
                {
                    _dispatcher.Invoke(() =>
                    {
                        args.TaskInfo.Action.IsEnabled = true;
                        args.TaskInfo.Action.Content = "Remove";
                    });
                },
                OnCanceled = (asyncTask, args) =>
                {
                    _dispatcher.Invoke(() =>
                    {
                        args.TaskInfo.Action.IsEnabled = true;
                        args.TaskInfo.Action.Content = "Remove";
                    });
                }
            };

            // Run the task asynchronously wrapped with the monitor.
            task.Start();
        }


        /// <summary>
        ///     StopButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var rst in ListView1.Items.Cast<TaskRecordSet>()) {
                if (!(rst.Tag is (Tasks.AsyncTask asyncTask, TaskEventArgs<TaskRecordSet, TaskList> _)))
                    throw new NullReferenceException();
                if (!asyncTask.Task.IsCompleted)
                    RemoveButton_Click(rst.Action, null);
            }
        }


        /// <summary>
        ///     ClearButton_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            StopButton_Click(this, null);

            if (ListView1.Items.Count > 0)
            {
                lblStatusBar.Text = @"Clearing task list.";
                ListView1.Items.Clear();
            }
            else
                lblStatusBar.Text = @"Task list has already been cleared.";
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
        private void ListView1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ListView1.Items.Count == 0)
                return;

            var cell = sender switch
            {
                DataGridCell dgc => dgc,
                TextBlock tb => tb.Parent as DataGridCell,
                Button button => button.Parent as DataGridCell,
                _ => throw new ArgumentOutOfRangeException()
            };
            if (cell != null)
                lblCursorPosition.Text = $"Over {cell.Column.Header} at (Row: {Grid.GetRow(cell) + 1}, Col: {cell.Column.DisplayIndex + 1})";
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
        private static void MyButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            Mouse.OverrideCursor = Cursors.Hand;
            btn.Background = new ImageBrush
            {
                ImageSource = (ImageSource)Application.Current.Resources["Button-Hover"]
            };
        }


        /// <summary>
        ///     MyButton_MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            Mouse.OverrideCursor = Cursors.Arrow;
            btn.Background = new ImageBrush
            {
                ImageSource = (ImageSource)Application.Current.Resources["Button-Normal"]
            };
        }


        /// <summary>
        ///     MyButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            btn.MouseEnter -= MyButton_MouseEnter;
            btn.MouseLeave -= MyButton_MouseLeave;
            btn.SetValue(BackgroundProperty, new ImageBrush((ImageSource)Application.Current.Resources["Button-Pressed"]));
        }
    }
}
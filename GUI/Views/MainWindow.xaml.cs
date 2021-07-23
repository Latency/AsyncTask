// ****************************************************************************
// Project:  GUI
// File:     MainWindow.xaml.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AsyncTask.Interfaces;
using ORM_Monitor.Interfaces;
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
            if (sender is not Button btn)
                throw new NullReferenceException();

            if (btn.Tag is not (AsyncTask.AsyncTask asyncTask, ITaskEventArgs))
                throw new NullReferenceException();

            if (asyncTask.TaskInfo is not ITaskRecordSet rst)
                throw new NullReferenceException();

            var recordSet = ListView1.Items.Cast<ITaskRecordSet>().FirstOrDefault(r => r.GridRow == rst.GridRow);
            if (recordSet is null)
                throw new NullReferenceException();

            var index = recordSet.GridRow.GetIndex();
            lblStatusBar.Text = $"Button clicked: (Row: {index + 1}, Action: {btn.Content})";

            if (btn.IsEnabled && asyncTask.Status == TaskStatus.Running)
                try
                {
                    asyncTask.Cancel();
                    MyButton_MouseDown(sender, null);
                    btn.IsEnabled = false;
                }
                catch (Exception)
                {
                    // Handled
                }
            else
                ListView1.Items.RemoveAt(index);
        }


        /// <summary>
        ///     StartButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new AsyncTask.AsyncTask((asyncTask, _) =>
            {
                WaitHandle.WaitAny(new[] { asyncTask.TaskInfo.Token.WaitHandle });
            })
            {
                TaskInfo = new TaskRecordSet
                {
                    Name                   = nameof(AsyncTask),
                    PollInterval           = new TimeSpan(0, 0, 0, 0, 10),
                    Owner                  = this,
                    SynchronizationContext = SynchronizationContext.Current
                },
                OnAdd = (asyncTask, args) =>
                {
                    if (asyncTask.TaskInfo is not ITaskRecordSet rst)
                        throw new NullReferenceException();

                    rst.ID                =  new Random().Next();
                    rst.Priority          =  new Random().Next(0, 5);
                    rst.Date              =  DateTime.Now;
                    rst.Tag               =  (asyncTask, args);

                    lblStatusBar.Text = $"Starting task \"{rst.ID}\".";

                    var index = ListView1.Items.Add(rst);
                    ListView1.UpdateLayout();
                    ListView1.ScrollIntoView(ListView1.Items[index]);

                    if (ListView1.ItemContainerGenerator.ContainerFromIndex(index) is not DataGridRow rowContainer)
                        throw new NullReferenceException();

                    rst.GridRow = rowContainer;

                    var a   = Extensions.Extensions.FindFirstChild<DataGridCellsPanel>(rowContainer);
                    if (a is null)
                        return;

                    var content = a.Children.Cast<DataGridCell>().FirstOrDefault(c => c.Column.Header.ToString() == "Action")?.Content;
                    rst.Action            =  Extensions.Extensions.FindFirstChild<Button>(content as FrameworkElement);
                    rst.Action.Content    =  "Stop";
                    rst.Action.Tag        =  rst.Tag;
                    rst.Action.Click      += RemoveButton_Click;
                    rst.Action.MouseDown  += MyButton_MouseDown;
                    rst.Action.MouseEnter += MyButton_MouseEnter;
                    rst.Action.MouseLeave += MyButton_MouseLeave;
                },
                OnComplete = (asyncTask, _) =>
                {
                    if (asyncTask.TaskInfo is not ITaskRecordSet rst)
                        throw new NullReferenceException();

                    if (rst.Action is not null)
                    {
                        rst.Action.IsEnabled = true;
                        rst.Action.Content   = "Remove";
                    }
                },
                OnTick = (asyncTask, _) =>
                {
                    if (asyncTask.TaskInfo is not ITaskRecordSet rst)
                        throw new NullReferenceException();

                    if (rst.Progress < 100)
                        rst.Progress += 1;
                    else
                        asyncTask.Cancel();
                },
                OnTimeout = (asyncTask, _) =>
                {
                    if (asyncTask.TaskInfo is not TaskRecordSet rst)
                        throw new NullReferenceException();

                    if (rst.Action is not null)
                    {
                        rst.Action.IsEnabled = true;
                        rst.Action.Content   = "Remove";
                    }
                },
                OnCanceled = (asyncTask, _) =>
                {
                    if (asyncTask.TaskInfo is not TaskRecordSet rst)
                        throw new NullReferenceException();

                    if (rst.Action is not null)
                    {
                        rst.Action.IsEnabled = true;
                        rst.Action.Content   = "Remove";
                    }
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
            foreach (var rst in ListView1.Items.Cast<ITaskRecordSet>().ToList())
            {
                if (rst.Tag is not (AsyncTask.AsyncTask asyncTask, ITaskEventArgs))
                    throw new NullReferenceException();

                if (!asyncTask.IsCompleted)
                    asyncTask.Cancel();
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
                msg.Append($"{(msg.Length > 0 ? ", " : string.Empty)}{item.ID}");
            if (msg.Length <= 0)
                return;
            lblStatusBar.Text = $"Selected '{msg}' of {ListView1.SelectedItems.Count} item{(ListView1.SelectedItems.Count != 1 ? "s" : string.Empty)}.";
        }


        /// <summary>
        ///     ListView1_MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ListView1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ListView1.Items.Count == 0)
                return;

            var cell = sender switch
            {
                DataGridCell dgc => dgc,
                TextBlock tb     => tb.Parent as DataGridCell,
                Button button    => button.Parent as DataGridCell,
                _                => throw new ArgumentOutOfRangeException(nameof(sender))
            };

            if (cell != null)
                lblCursorPosition.Text = $"Over {cell.Column.Header} at (Row: {(cell.DataContext as ITaskRecordSet)?.GridRow.GetIndex() + 1}, Col: {cell.Column.DisplayIndex + 1})";
        }


        /// <summary>
        ///     ListView1_MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView1_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCursorPosition.Text = string.Empty;
        }


        /// <summary>
        ///     MyButton_MouseEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Button btn)
                return;

            Mouse.OverrideCursor = Cursors.Hand;
            btn.Background = new ImageBrush
            {
                ImageSource = (ImageSource) Application.Current.Resources["Button-Hover"]
            };
        }


        /// <summary>
        ///     MyButton_MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Button btn)
                return;

            Mouse.OverrideCursor = Cursors.Arrow;
            btn.Background = new ImageBrush
            {
                ImageSource = (ImageSource) Application.Current.Resources["Button-Normal"]
            };
        }


        /// <summary>
        ///     MyButton_MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is not Button btn)
                return;

            btn.MouseEnter -= MyButton_MouseEnter;
            btn.MouseLeave -= MyButton_MouseLeave;
            btn.Background = new ImageBrush
            {
                ImageSource = (ImageSource)Application.Current.Resources["Button-Pressed"]
            };
        }
    }
}
// ****************************************************************************
// Project:  Console
// File:     MainWindow.xaml.cs
// Author:   Latency McLaughlin
// Date:     09/24/2024
// ****************************************************************************

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using AsyncTask.Interfaces;
using AsyncTask.Logging;
using AsyncTask.Models;
using Microsoft.Extensions.Logging;
using ILogger = AsyncTask.Interfaces.ILogger;

namespace Console.Views;

/// <summary>
///     Interaction logic for Window1.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ILogger?             _logger;
    private AsyncTask.AsyncTask? _t2;


    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow() => InitializeComponent();


    private void OnButtonChanged(bool enabled)
    {
        switch (enabled)
        {
            case true:
                ButtonDisable.IsEnabled = true;
                ButtonEnable.IsEnabled  = false;
                break;
            case false:
                ButtonDisable.IsEnabled = false;
                ButtonEnable.IsEnabled  = true;
                break;
        }
    }


    private void ButtonEnable_Click(object sender, EventArgs e)
    {
        var blockTime = NumericUpDownBlockTime.Value!.Value;
        var timeout   = NumericUpDownTimeout.Value!.Value;

        _t2 = new((task, args) =>
            {
                do
                {
                    task.TaskInfo.Token.ThrowIfCancellationRequested();
                    Task.Delay(250).GetAwaiter().GetResult();
                }
                while (args.Duration < TimeSpan.FromSeconds(blockTime));
            }
        )
        {
            TaskInfo = new TaskInfo
            {
                Name                   = "t2",
                Timeout                = timeout < 0 ? null : TimeSpan.FromSeconds(timeout),
                Logger                 = _logger,
                PollInterval           = new(0, 0, 1),
                SynchronizationContext = SynchronizationContext.Current
            },
            OnAdd = (asyncTask, _) =>
            {
                asyncTask.TaskInfo.Logger?.Trace($"Adding task for {asyncTask.TaskInfo.Name}");
                asyncTask.TaskInfo.Logger?.Warning($"Executing **** t2 **** test.  Blocking for {blockTime} seconds.  Timeout {(timeout < 0 ? "is infinate" : $"at {timeout} second{(timeout == 1 ? string.Empty : "s")}")}.");
                OnButtonChanged(true);
            },
            OnRemove = (asyncTask, _) =>
            {
                asyncTask.TaskInfo.Logger?.Trace($"Removing task for {asyncTask.TaskInfo.Name}");
                OnButtonChanged(false);
            },
            OnComplete = (asyncTask, _) => asyncTask.TaskInfo.Logger?.Warning($"Completing task for '{asyncTask.TaskInfo.Name}'."),
            OnTick     = (asyncTask, args) => asyncTask.TaskInfo.Logger?.Information($@"Duration:  {args.Duration:hh\:mm\:ss}"),
            OnTimeout  = (asyncTask, _) => asyncTask.TaskInfo.Logger?.Critical($"Timeout expired!  Aborting task for '{asyncTask.TaskInfo.Name}'."),
            OnCanceled = (asyncTask, _) => asyncTask.TaskInfo.Logger?.Critical($"Canceling task for '{asyncTask.TaskInfo.Name}'."),
            OnError    = (asyncTask, args) => asyncTask.TaskInfo.Logger?.Error($"Error occured for '{asyncTask.TaskInfo.Name}'.{Environment.NewLine}\t{args.Exception?.Message}")
        };
        _t2.Start();
    }


    private void ButtonDisable_Click(object sender, EventArgs e)
    {
        OnButtonChanged(false);
        _t2?.Cancel();
    }


    private void ButtonClear_Click(object sender, EventArgs e) => TextBoxMessageLog.Document.Blocks.Clear();


    private void Form1_Load(object sender, EventArgs e)
    {
        _logger = new DefaultLogger(SynchronizationContext.Current);
        _logger.Add(LogLevel.Trace,       arg => Color(arg, new(Colors.Green)));
        _logger.Add(LogLevel.Debug,       arg => Color(arg, new(Colors.DarkOrange)));
        _logger.Add(LogLevel.Information, arg => Color(arg, new(Colors.DodgerBlue)));
        _logger.Add(LogLevel.Warning,     arg => Color(arg, new(Colors.Yellow)));
        _logger.Add(LogLevel.Error,       Error);
        _logger.Add(LogLevel.Critical,    arg => Color(arg, new(Colors.HotPink)));
        _logger.Add(LogLevel.None,        arg => Color(arg, new(Colors.White)));

        OnButtonChanged(false);
    }


    #region Callbacks

    // =-=-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private void Color(IMessageEventArgs args, SolidColorBrush color)
    {
        // If a message contains line breaks, the code bellow will
        // add an empty blank line for every line break in the message.
        // To avoid that we have to replace all new lines in the mesage with '\r' symbol.
        var message = Regex.Replace(args.Message, @"(\r\n)|(\n\r)|(\n)", "\r");

        var tr = new TextRange(TextBoxMessageLog.Document.ContentEnd, TextBoxMessageLog.Document.ContentEnd)
        {
            Text = message
        };
        tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);

        TextBoxMessageLog.ScrollToEnd();
    }


    private void Error(IMessageEventArgs args)
    {
        Color(args, new(Colors.Red));

        if (args.Exception == null)
            return;

        args.Message = args.Exception.Message;
        Color(args, new(Colors.Gray));
    }

    // =-=-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #endregion Callbacks
}
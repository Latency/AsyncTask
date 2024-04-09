// ****************************************************************************
// Project:  Console
// File:     Form1.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using AsyncTask.Interfaces;
using AsyncTask.Logging;
using AsyncTask.Models;
using Microsoft.Extensions.Logging;
using ILogger = AsyncTask.Interfaces.ILogger;

namespace Console;

public partial class Form1 : Form
{
    private ILogger?             _logger;
    private AsyncTask.AsyncTask? _t2;


    /// <summary>
    ///     Constructor
    /// </summary>
    public Form1() => InitializeComponent();


    private void TextBoxMessageLog_TextChanged(object sender, EventArgs e)
    {
        // set the current caret position to the end
        TextBoxMessageLog.SelectionStart = TextBoxMessageLog.Text.Length;
        // scroll it automatically
        TextBoxMessageLog.ScrollToCaret();
    }


    private void OnButtonChanged(bool enabled)
    {
        switch (enabled)
        {
            case true:
                ButtonDisable.Enabled = true;
                ButtonEnable.Enabled  = false;
                break;
            case false:
                ButtonDisable.Enabled = false;
                ButtonEnable.Enabled  = true;
                break;
        }
    }


    private void ButtonEnable_Click(object sender, EventArgs e)
    {
        var blockTime = (int)numericUpDownBlockTime.Value;
        var timeout   = (int)numericUpDownTimeout.Value;
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
            OnTick     = (asyncTask, args) => asyncTask.TaskInfo.Logger?.Information($"Duration:  {args.Duration:hh\\:mm\\:ss}"),
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


    private void ButtonClear_Click(object sender, EventArgs e) => TextBoxMessageLog.Clear();


    private void Form1_Load(object sender, EventArgs e)
    {
        _logger = new DefaultLogger(SynchronizationContext.Current);
        _logger.Add(LogLevel.Trace,       Trace);
        _logger.Add(LogLevel.Debug,       Debug);
        _logger.Add(LogLevel.Information, Information);
        _logger.Add(LogLevel.Warning,     Warning);
        _logger.Add(LogLevel.Error,       Error);
        _logger.Add(LogLevel.Critical,    Critical);
        _logger.Add(LogLevel.None,        None);

        OnButtonChanged(false);
    }


    #region Callbacks

    // =-=-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private void Critical(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.HotPink;
        TextBoxMessageLog.AppendText(args.Message);
    }


    private void Trace(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.Green;
        TextBoxMessageLog.AppendText(args.Message);
    }


    private void None(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.White;
        TextBoxMessageLog.AppendText(args.Message);
    }


    private void Information(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.DodgerBlue;
        TextBoxMessageLog.AppendText(args.Message);
    }


    private void Warning(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.Yellow;
        TextBoxMessageLog.AppendText(args.Message);
    }


    private void Error(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.Red;
        TextBoxMessageLog.AppendText(args.Message);
        if (args.Exception != null)
        {
            TextBoxMessageLog.SelectionColor = Color.Gray;
            TextBoxMessageLog.AppendText(args.Exception.Message);
        }
    }


    private void Debug(IMessageEventArgs args)
    {
        TextBoxMessageLog.SelectionColor = Color.DarkOrange;
        TextBoxMessageLog.AppendText(args.Message);
    }

    // =-=-=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #endregion Callbacks
}
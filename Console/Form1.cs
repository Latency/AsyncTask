// ****************************************************************************
// Project:  ConsoleApp1
// File:     Form1.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Windows.Forms;
using AsyncTask.DTO;
using AsyncTask.Interfaces;
using ConsoleApp1;
using ConsoleApp1.Extensions;
using Logger = Console.Logging.Logger;

namespace Console
{
    public partial class Form1 : Form
    {
        private event EventHandler OnButtonChanged;
        private readonly ILogger _logger;
        private ButtonType _buttonType;
        private AsyncTask.AsyncTask _t2;


        /// <summary>
        ///     Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            OnButtonChanged += ButtonChanged_Event;
            ButtonDisable.Enabled = false;
            _logger = new Logger(TextBoxMessageLog);
        }


        private void TextBoxMessageLog_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            TextBoxMessageLog.SelectionStart = TextBoxMessageLog.Text.Length;
            // scroll it automatically
            TextBoxMessageLog.ScrollToCaret();
        }


        private void ButtonChanged_Event(object sender, EventArgs e)
        {
            switch (_buttonType)
            {
                case ButtonType.Disabled:
                    ButtonDisable.Enabled = true;
                    ButtonEnable.Enabled = false;
                    break;
                case ButtonType.Enabled:
                    ButtonDisable.Enabled = false;
                    ButtonEnable.Enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ButtonChanged_Event));
            }
        }


        private void ChangeState(ButtonType type)
        {
            if (_buttonType == type)
                return;
            _buttonType = type;
            OnButtonChanged?.Invoke(this, EventArgs.Empty);
        }


        private void ButtonEnable_Click(object sender, EventArgs e)
        {
            ButtonEnable.Enabled  = false;
            ButtonDisable.Enabled = true;

            var blockTime = (int) numericUpDownBlockTime.Value;
            var timeout   = (int) numericUpDownTimeout.Value;
            _t2 = new AsyncTask.AsyncTask(token =>
            {
                var startTime = DateTime.Now;
                while (!token.IsCancellationRequested && DateTime.Now < startTime.Add(TimeSpan.FromSeconds(blockTime)))
                {
                    if (!token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }
            })
            {
                TaskInfo = new TaskInfo
                {
                    Name = "t2"
                },
                Timeout  = timeout < 0 ? null : TimeSpan.FromSeconds(timeout),
                Logger   = _logger,
                TaskList = new TaskList(),
                OnAdd    = (_, _) =>
                {
                    _logger.Warning($"Executing **** t2 **** test.  Blocking for {blockTime} seconds.  Timeout {(timeout < 0 ? "is infinate" : $"at {timeout} second{(timeout == 1 ? string.Empty : "s")}")}.");
                    ChangeState(ButtonType.Disabled);
                },
                OnRemove   = (_,         _) => this.InvokeIfRequired(ChangeState, ButtonType.Enabled, new Action<ButtonType>(ChangeState), ButtonType.Enabled),
                OnComplete = (asyncTask, _) => _logger.Info($"Completing task for '{asyncTask.TaskInfo?.Name}'."),
                OnTick     = (_,         args) => _logger.Info($"Duration:  {args.Duration:hh\\:mm\\:ss}"),
                OnTimeout  = (asyncTask, _) => _logger.Info($"Timeout expired!  Aborting task for '{asyncTask.TaskInfo?.Name}'."),
                OnCanceled = (asyncTask, _) => _logger.Info($"Canceling task for '{asyncTask.TaskInfo?.Name}'."),
                OnError    = (asyncTask, args) => _logger.Error($"Error occured for '{asyncTask.TaskInfo?.Name}'.{Environment.NewLine}\t{args.Exception?.Message}")
            };
            _t2.Start();
        }


        private void ButtonDisable_Click(object sender, EventArgs e)
        {
            ChangeState(ButtonType.Enabled);

            _t2?.Cancel();
        }


        private void ButtonClear_Click(object sender, EventArgs e)
        {
            TextBoxMessageLog.InvokeIfRequired(() => TextBoxMessageLog.Clear(), new EventHandler(ButtonClear_Click), sender, e);
        }
    }
}
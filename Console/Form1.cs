// ****************************************************************************
// Project:  ConsoleApp1
// File:     Form1.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Threading;
using System.Windows.Forms;
using AsyncTask.DTO;
using AsyncTask.Interfaces;
using ConsoleApp1.Extensions;
using Logger = ConsoleApp1.Logging.Logger;

namespace ConsoleApp1
{
    public partial class Form1 : Form
    {
        private readonly ILogger _logger;
        private ButtonType _buttonType;
        private AsyncTask.Tasks.AsyncTask _t2;

        public Form1()
        {
            InitializeComponent();
            OnButtonChanged += ButtonChanged_Event;
            ButtonDisable.Enabled = false;
            _logger = new Logger(TextBoxMessageLog);
        }

        public ButtonType State
        {
            get => _buttonType;
            set
            {
                if (_buttonType == value)
                    return;
                _buttonType = value;
                OnButtonChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private event EventHandler OnButtonChanged;


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


        private void ChangeState(ButtonType type) => State = type;


        private void ButtonEnable_Click(object sender, EventArgs e)
        {
            var blockTime = (int) numericUpDownBlockTime.Value;
            var timeout = (int) numericUpDownTimeout.Value;
            _t2 = new AsyncTask.Tasks.AsyncTask
            {
                TaskInfo = new TaskInfo
                {
                    Name = "t2"
                },
                Timeout = timeout < 0 ? (TimeSpan?) null : TimeSpan.FromSeconds(timeout),
                Logger = _logger,
                TaskList = new TaskList(),
                Delegate = (asyncTask, args) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(blockTime));
                },
                OnAdd = (asyncTask, args) =>
                {
                    _logger.Warning($"Executing **** t2 **** test.  Blocking for {blockTime} seconds.  Timeout {(timeout < 0 ? "is infinate" : $"at {timeout} second{(timeout == 1 ? string.Empty : "s")}")}.");
                    this.InvokeIfRequired(ChangeState, ButtonType.Disabled, new Action<ButtonType>(ChangeState), ButtonType.Disabled);
                },
                OnRemove = (asyncTask, args) => { this.InvokeIfRequired(ChangeState, ButtonType.Enabled, new Action<ButtonType>(ChangeState), ButtonType.Enabled); },
                OnComplete = (asyncTask, args) => _logger.Info($"Completing task for '{args.TaskInfo.Name}'."),
                OnTimeout = (asyncTask, args) => _logger.Info($"Timeout expired!  Aborting task for '{args.TaskInfo.Name}'."),
                OnCanceled = (asyncTask, args) => _logger.Info($"Canceling task for '{args.TaskInfo.Name}'.")
            };
            _t2.Start();
        }

        private void ButtonDisable_Click(object sender, EventArgs e)
        {
            if (_t2 == null)
                return;

            ButtonDisable.InvokeIfRequired(() => ButtonDisable.Enabled = false, new EventHandler(ButtonDisable_Click), sender, e);
            _t2.Cancel();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            TextBoxMessageLog.InvokeIfRequired(() => TextBoxMessageLog.Clear(), new EventHandler(ButtonClear_Click), sender, e);
        }
    }
}
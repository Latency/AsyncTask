//  *****************************************************************************
//  File:      Form1.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Threading;
using System.Windows.Forms;
using GUI.Properties;
using ORM_Monitor;
using ReflectSoftware.Insight;

namespace GUI.WinForms {
  public partial class Form1 : Form {
    private readonly ReflectInsight _log = RILogManager.Default;

    /// <summary>
    ///   Constructor
    /// </summary>
    public Form1() {
      InitializeComponent();
    }


    /// <summary>
    ///   button1_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button1_Click(object sender, EventArgs e) {
      Action<Control, string> action = (ctl, a) => {
        if (ctl.InvokeRequired)
          ctl.Invoke((MethodInvoker) delegate { ctl.Text += a + Environment.NewLine; });
        else
          ctl.Text += a + Environment.NewLine;
      };

      TaskEventArgs.Expression expression = args => {
        var obj = args[0];
        var str = args[1] as string;
        var testNo = obj != null ? $"{((TaskEvent<dynamic>)obj).Name}: " : string.Empty;
        _log.SendInformation(testNo + str);
      };


      richTextBox1.Text += "***Starting Test***\n";

      var t = new TaskEvent<Action<Control, MethodInvoker>>(expression, TimeSpan.FromSeconds(Settings.Default.TTL)) {
        Name = "task"
      };

      t.OnRunning((th, tea) => { SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 20)); });
      t.OnCompleted(
        (th, tea) => {
          action.Invoke(richTextBox1, "Completed");
        }
      );
      t.OnCanceled(
        (th, tea) => {
          action.Invoke(richTextBox1, "Canceled");
        }
      );
      t.OnTimeout(
        (th, tea) => {
          action.Invoke(richTextBox1, "Timed out");
        }
      );

      // Invoke extension method.
      t.AsyncMonitor();
    }


    /// <summary>
    ///   richTextBox1_TextChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void richTextBox1_TextChanged(object sender, EventArgs e) {
      if (richTextBox1.Visible) {
        richTextBox1.SelectionStart = richTextBox1.TextLength;
        richTextBox1.ScrollToCaret();
      }
    }
  }
}
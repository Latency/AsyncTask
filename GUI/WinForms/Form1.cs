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
using ORM_Monitor.Controllers;
using ORM_Monitor.Models;

namespace GUI.WinForms {
  public partial class Form1 : Form {
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
      Action<Control, MethodInvoker> action = (ctl, a) => {
        if (ctl.InvokeRequired)
          ctl.Invoke(a);
        else
          a();
      };

      richTextBox1.Text += "***Starting Test***\n"; // Cross-Threaded call to UI thread will throw exception.

      var t = new TaskEvent<Action<Control, MethodInvoker>>(5000);
      t.RunningAction.OnRunning += (th, tea) => { SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 20)); };
      t.CompletedAction.OnCompleted += (th, tea) => {
        if (tea.Expression != null)
          tea.Expression(richTextBox1, delegate { richTextBox1.Text += "Completed\n"; });
        else
          richTextBox1.Text += "Completed\n"; // Cross-Threaded call to UI thread will throw exception.
      };
      t.CanceledAction.OnCanceled += (th, tea) => {
        // Is the same as above without the else clause using null propagation matching signature from Action<T1, T2>.Invoke Method in namespace System (System.Runtime.dll)
        tea.Expression?.Invoke(richTextBox1, delegate { richTextBox1.Text += "Canceled\n"; });
      };
      t.TimeoutAction.OnTimeout +=
        (th, tea) => { tea.Expression?.Invoke(richTextBox1, delegate { richTextBox1.Text += "Timed out\n"; }); };

      // Invoke extension method.
      //Async_IO.AsyncMonitor(null, t);   /* Explicit use Will cause cross-threaded operations on 'richTextbox.Text' due to null exception. */
#pragma warning disable 4014
      action.AsyncMonitor(t);
#pragma warning restore 4014
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
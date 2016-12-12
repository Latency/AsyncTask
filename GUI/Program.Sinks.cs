//  *****************************************************************************
//  File:      Program.Sinks.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using ReflectSoftware.Insight;
using ReflectSoftware.Insight.Common;

namespace GUI {
  internal abstract partial class Program {
    protected static event EventHandler OnError;


    /// <summary>
    ///   LogException
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected static void LogException(object sender, EventArgs e) {
      var methodBase = MethodBase.GetCurrentMethod();
      if (methodBase == null)
        return;
      var strBase = string.Empty;
      if (methodBase.DeclaringType != null)
        strBase += methodBase.DeclaringType.FullName + '.';
      strBase += methodBase.Name;
      // Hooks into ReflectInsight listner for LiveView / Database / STDOUT / etc. error logging.   See 'app.config'
      RILogManager.Default.SendException(strBase, sender as Exception);
    }


    /// <summary>
    ///   ExceptionSinkTrigger
    /// </summary>
    /// <param name="innerException"></param>
    /// <returns></returns>
    private static Exception ExceptionSinkTrigger(Exception innerException) {
      // Fix the exception to include the stack trace from the current frame.
      Exception exception;
      try {
        throw new ReflectInsightException(MethodBase.GetCurrentMethod().Name, innerException);
      }
      catch (Exception ex) {
        ex.Source = new StackFrame(2).GetMethod().Name;
        exception = ex;
      }
      // Remote logging and system messaging trigger.
      OnError?.Invoke(exception, new EventArgs());
      return exception;
    }


    /// <summary>
    ///   Handle the UI exceptions by showing a dialog box, and asking the user whether or not they wish to abort execution.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="t"></param>
    protected static void ThreadException(object sender, ThreadExceptionEventArgs t) {
      var exception = ExceptionSinkTrigger(t.Exception);

      // Unwind the call stack and attribute the 'data' tags for the ExceptionMessageBox.
      for (var e = exception; e != null; e = e.InnerException) {
        if (e.Data.Count <= 0)
          continue;
        var collection = e.Data.Cast<DictionaryEntry>().ToDictionary<DictionaryEntry, object, object>(kvp => "AdvancedInformation." + kvp.Key, kvp => kvp.Value ?? string.Empty);
        e.Data.Clear();
        foreach (var item in collection)
          e.Data.Add(item.Key, item.Value);
      }

      var exceptionMessageBox = new ExceptionMessageBox(exception, ExceptionMessageBoxButtons.AbortRetryIgnore, ExceptionMessageBoxSymbol.Error);
      switch (exceptionMessageBox.Show(sender as Form)) {
        case DialogResult.Abort:
          Application.Exit();
          break;

        case DialogResult.Cancel:
          break;

        case DialogResult.Retry:
          var currentProcess = Process.GetCurrentProcess();
          RILogManager.Default.SendInformation($"Restarting application `{currentProcess.ProcessName}' after exception `{exception.GetType().Name}' on pid #{currentProcess.Id}");
          Application.Restart();
          break;
      }
    }


    /// <summary>
    ///   UnhandledException
    /// </summary>
    /// <remarks>Exception sink if all else fails and was never trapped.</remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected static void UnhandledException(object sender, UnhandledExceptionEventArgs e) {
      var ex = ExceptionSinkTrigger((Exception) e.ExceptionObject);
      var exceptionMessageBox = new ExceptionMessageBox(ex, ExceptionMessageBoxButtons.OK, ExceptionMessageBoxSymbol.Error);
      exceptionMessageBox.Show(null);
    }
  }
}
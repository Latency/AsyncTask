//  *****************************************************************************
//  File:      Program.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using Telerik.WinControls.UI;

namespace GUI {
  internal abstract partial class Program {
    /// <summary>
    ///   The main entry point for the application.
    /// </summary>
    /// <param name="form"></param>
    protected static void MainWrapper(RadForm form) {
      #region Exception Sink Handlers

      // ---------------------------------------------------------------------

      // Add the event handler for handling UI thread exceptions to the event.
      Application.ThreadException += ThreadException;
      OnError += LogException;

      // Add the event handler for handling non-UI thread exceptions to the event. 
      AppDomain.CurrentDomain.UnhandledException += UnhandledException;

      // ---------------------------------------------------------------------

      #endregion Exception Sink Handlers

      //====================================================================
      try {
        Application.Run(form);
      } catch (Exception e) {
        var ex = ExceptionSinkTrigger(e);
        var exceptionMessageBox = new ExceptionMessageBox(ex, ExceptionMessageBoxButtons.OK, ExceptionMessageBoxSymbol.Error);
        exceptionMessageBox.Show(null);
      }
    }
  }
}
//  *****************************************************************************
//  File:      Loader1.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Windows.Forms;
using GUI.WinForms;

namespace GUI {
  internal sealed class Loader1 : Program {
    /// <summary>
    ///   The main entry point for the application.
    /// </summary>
    /// <param name="args">Application input arguments and/or switch commands - (unused)</param>
    [STAThread]
    public static void Main(string[] args) {
      // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
      Application.SetCompatibleTextRenderingDefault(false);
      Application.EnableVisualStyles();
      MainWrapper(new Form1());
    }
  }
}
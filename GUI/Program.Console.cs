//  *****************************************************************************
//  File:      Program.Console.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GUI {
  internal static class Terminal {
    /// <summary>Identifies the console of the parent of the current process as the console to be attached.</summary>
    // ReSharper disable once InconsistentNaming
    private const uint ATTACH_PARENT_PROCESS = 0xFFFFFFFF;

    /// <summary>
    ///   calling process is already attached to a console
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private const int ERROR_ACCESS_DENIED = 5;


    /// <summary>
    ///   Allocates a new console for the calling process.
    /// </summary>
    /// <returns>
    ///   If the function succeeds, the return value is nonzero.
    ///   If the function fails, the return value is zero.
    ///   To get extended error information, call Marshal.GetLastWin32Error.
    /// </returns>
    [DllImport("kernel32", SetLastError = true)]
    private static extern bool AllocConsole();


    /// <summary>
    ///   Detaches the calling process from its console
    /// </summary>
    /// <returns>
    ///   If the function succeeds, the return value is nonzero.
    ///   If the function fails, the return value is zero.
    ///   To get extended error information, call Marshal.GetLastWin32Error.
    /// </returns>
    [DllImport("kernel32", SetLastError = true)]
    private static extern bool FreeConsole();


    /// <summary>
    ///   Attaches the calling process to the console of the specified process.
    /// </summary>
    /// <param name="dwProcessId">[in] Identifier of the process, usually will be ATTACH_PARENT_PROCESS</param>
    /// <returns>
    ///   If the function succeeds, the return value is nonzero.
    ///   If the function fails, the return value is zero.
    ///   To get extended error information, call Marshal.GetLastWin32Error.
    /// </returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AttachConsole(uint dwProcessId);


    /// <summary>
    ///   Allocate a console if application started from within windows GUI.
    ///   Detects the presence of an existing console associated with the application and
    ///   attaches itself to it if available.
    /// </summary>
    public static void Allocate() {
      var isConsoleApp = Console.OpenStandardInput(1) != Stream.Null;
      if (isConsoleApp)
        return;

      //
      // the following should only be used in a non-console application type (C#)
      // (since a console is allocated/attached already when you define a console app)
      //
      if (AttachConsole(ATTACH_PARENT_PROCESS))
        return;

      if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED) // Console already running.
        return;

      // A console was not allocated, so we need to make one.
      if (AllocConsole())
        return;

      MessageBox.Show(Marshal.GetLastWin32Error().ToString());
      throw new Exception("Console Allocation Failed");
    }


    /// <summary>
    ///   Dispose the console that was allocated.
    /// </summary>
    public static void Free() {
      if (!Environment.UserInteractive)
        return;

      Console.ReadKey();
      FreeConsole();
    }
  }
}
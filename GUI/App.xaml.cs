//  *****************************************************************************
//  File:       App.xaml.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       02/18/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Extensions.Logging;
using ORM_Monitor.View;

namespace ORM_Monitor {
  /// <summary>
  ///   Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    /// <summary>
    ///   Application entry point.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <summary>
    ///   Handles the start event of the application.
    /// </summary>
    private void Application_OnStartup(object sender, StartupEventArgs e) {
      #region Exception Sink Handlers

      // ---------------------------------------------------------------------
      DispatcherUnhandledException += Sinks.ReportUnhandledException;

      // Add the event handler for handling UI thread exceptions to the event.
      AppDomain.CurrentDomain.AssemblyLoad += (o, args) => {
#if ASMLOADING
        Log.Debug(args.LoadedAssembly.FullName);
#endif
      };

      AppDomain.CurrentDomain.ProcessExit += (o, args) => {
      };

      AppDomain.CurrentDomain.FirstChanceException += (o, args) => {
        // Log.Error($"FirstChanceException event raised in {AppDomain.CurrentDomain.FriendlyName}", args.Exception);
      };

      // Add the event handler for handling non-UI thread exceptions to the event. 
      AppDomain.CurrentDomain.UnhandledException += Sinks.UnhandledException;

      // ---------------------------------------------------------------------

      #endregion Exception Sink Handlers


      try {
        var loggerFactory = LoggerFactory.Create(builder => {
          builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
            .AddConsole()
            .AddDebug();
        });
        ILogger logger = loggerFactory.CreateLogger<App>();

        var mainView = new MainWindow(logger);
        mainView.Show();
      } catch (Exception ex) {
        Debug.WriteLine(ex);
      }
    }
  }
}
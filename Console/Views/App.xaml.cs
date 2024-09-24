// ****************************************************************************
// Project:  Console
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     09/24/2024
// ****************************************************************************

using System.Windows;

namespace Console.Views;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    ///     Application entry point.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <summary>
    ///     Handles the start event of the application.
    /// </summary>
    private void Application_OnStartup(object sender, StartupEventArgs e) => new MainWindow().Show();
}
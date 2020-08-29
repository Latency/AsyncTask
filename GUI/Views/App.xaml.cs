// ****************************************************************************
// Project:  GUI
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System.Windows;

namespace ORM_Monitor.Views
{
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
}
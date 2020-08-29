// ****************************************************************************
// Project:  GUI
// File:     TaskService.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System.Windows;
using System.Windows.Controls;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Models
{
    /// <summary>
    ///     ServiceTask
    /// </summary>
    public class TaskService
    {
        #region Properties

        // -----------------------------------------------------------------------
        /// <summary>
        ///     Owner
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        ///     RecordSet
        /// </summary>
        public ITaskRecordSet TaskInfo { get; set; }

        /// <summary>
        ///     View
        /// </summary>
        public DataGridRow GridRow { get; set; }

        /// <summary>
        ///     Handler
        /// </summary>
        public Tasks.AsyncTask Task { get; set; }

        // -----------------------------------------------------------------------

        #endregion Properties
    }
}
// ****************************************************************************
// Project:  GUI
// File:     TaskRecordSet.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Models
{
    public class TaskRecordSet : TaskRecordSetBase, ITaskRecordSet
    {
        /// <inheritdoc />
        [GridColumn(Header = "ID", Name = "ColID", Width = 30, IsReadOnly = true)]
        public int ID
        {
            get => Get<int>(MethodBase.GetCurrentMethod().Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Header = "TaskName", Name = "ColTask", Width = 80, IsReadOnly = true)]
        public string Name
        {
            get => Get<string>(MethodBase.GetCurrentMethod().Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Name = "ColDescription", Width = 280, IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public string Description
        {
            get => Get<string>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Name = "ColPriority", Width = 50, IsReadOnly = true)]
        public int Priority
        {
            get => Get<int>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Name = "ColProgress", Width = 60, IsReadOnly = true)]
        public ushort Progress
        {
            get => Get<ushort>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Name = "ColStatus", Width = 100, IsReadOnly = true)]
        public TaskStatus Status
        {
            get => Get<TaskStatus>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Name = "ColAction", Width = 100, IsReadOnly = true)]
        public Button Action
        {
            get => Get<Button>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(Name = "ColDate", Width = 120, IsReadOnly = true)]
        public DateTime? Date
        {
            get => Get<DateTime?>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        [GridColumn(IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public object Tag
        {
            get => Get<object>(MethodBase.GetCurrentMethod()?.Name.Substring(4));
            set => Set(MethodBase.GetCurrentMethod()?.Name.Substring(4), value);
        }

        /// <inheritdoc />
        public override string ToString() => $"Task Scheduler #{ID}";
    }
}
// ****************************************************************************
// Project:  GUI
// File:     TaskRecordSet.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Models
{
    public sealed class TaskRecordSet : TaskRecordSetBase, ITaskRecordSet
    {
        public TaskRecordSet()
        {
            Name = ToString();
        }

        [GridColumn(Name = "ColID", Width = 80, IsReadOnly = true)]
        public int ID
        {
            get => Get<int>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColName", Width = 80, IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public string Name
        {
            get => Get<string>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name ="ColDescription", Width = 280, IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public string Description
        {
            get => Get<string>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColPriority", Width = 50, IsReadOnly = true)]
        public int Priority
        {
            get => Get<int>(MethodBase.GetCurrentMethod()?.Name[4..]);
            set => Set(MethodBase.GetCurrentMethod()?.Name[4..], value);
        }

        [GridColumn(Name = "ColProgress", Width = 60, IsReadOnly = true)]
        public ushort Progress
        {
            get => Get<ushort>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColAction", Width = 100, IsReadOnly = true)]
        public Button Action
        {
            get => Get<Button>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColDate", Width = 120, IsReadOnly = true)]
        public DateTime? Date
        {
            get => Get<DateTime?>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColOwner", IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public Window Owner
        {
            get => Get<Window>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColGridRow", IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public DataGridRow GridRow 
        {
            get => Get<DataGridRow>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        [GridColumn(Name = "ColTag", IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public object Tag
        {
            get => Get<object>((MethodBase.GetCurrentMethod()?.Name)?[4..]);
            set => Set((MethodBase.GetCurrentMethod()?.Name)?[4..], value);
        }

        public override string ToString() => $"Task Scheduler #{ID}";
    }
}
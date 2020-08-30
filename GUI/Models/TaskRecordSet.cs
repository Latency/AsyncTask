// ****************************************************************************
// Project:  GUI
// File:     TaskRecordSet.cs
// Author:   Latency McLaughlin
// Date:     08/28/2020
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System;
using System.Windows;
using System.Windows.Controls;
using AsyncTask.DTO;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Models
{
    public sealed class TaskRecordSet : TaskInfo, ITaskRecordSet
    {
        public TaskRecordSet()
        {
            Name = ToString();
        }

        [GridColumn(Width = 80, IsReadOnly = true)]
        public int ID { get; set; } = new Random().Next();

        [GridColumn(Width = 80, IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        [GridColumn(Width = 280, IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public string Description { get; set; }

        [GridColumn(Width = 50, IsReadOnly = true)]
        public int Priority { get; set; }

        [GridColumn(Width = 60, IsReadOnly = true)]
        public ushort Progress { get; set; }

        [GridColumn(Width = 100, IsReadOnly = true)]
        public Button Action { get; set; }

        [GridColumn(Width = 120, IsReadOnly = true)]
        public DateTime? Date { get; set; } = DateTime.Now;

        [GridColumn(IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public Window Owner { get; set; }

        [GridColumn(IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public DataGridRow GridRow { get; set; }

        public override string ToString() => $"Task Scheduler #{ID}";
    }
}
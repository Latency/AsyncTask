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
        #region Fields
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        private string      _description;
        private int         _priority;
        private ushort      _progress;
        private Button      _action;
        private DateTime?   _date;
        private Window      _owner;
        private object      _tag;
        private DataGridRow _gridRow;

        private int _id;
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Fields


        #region Properties

        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        [GridColumn(Name = "ColID", Width = 80, IsReadOnly = true)]
        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        [GridColumn(Name = "ColDescription", Width = 280, IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public string Description
        {
            get => _description;
            set {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        [GridColumn(Name = "ColPriority", Width = 50, IsReadOnly = true)]
        public int Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        [GridColumn(Name = "ColProgress", Width = 60, IsReadOnly = true)]
        public ushort Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        [GridColumn(Name = "ColAction", Width = 100, IsReadOnly = true)]
        public Button Action
        {
            get => _action;
            set
            {
                _action = value;
                OnPropertyChanged(nameof(Action));
            }
        }

        [GridColumn(Name = "ColDate", Width = 120, IsReadOnly = true)]
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        [GridColumn(Name = "ColOwner", IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public Window Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }

        [GridColumn(Name = "ColGridRow", IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public DataGridRow GridRow
        {
            get => _gridRow;
            set
            {
                _gridRow = value;
                OnPropertyChanged(nameof(GridRow));
            }
        }

        [GridColumn(Name = "ColTag", IsReadOnly = true, Visibility = Visibility.Collapsed)]
        public object Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged(nameof(Tag));
            }
        }
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        #endregion Properties

        public override string ToString() => $"Task Scheduler #{ID}";
    }
}
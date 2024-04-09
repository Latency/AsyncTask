// ****************************************************************************
// Project:  GUI
// File:     TaskRecordSet.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Controls;
using AsyncTask.Models;
using ORM_Monitor.Interfaces;

namespace ORM_Monitor.Models;

public sealed class TaskRecordSet : TaskInfo, ITaskRecordSet
{
    public override string ToString() => $"Task Scheduler #{ID}";

    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private string?      _description;
    private int          _priority;
    private ushort       _progress;
    private Button?      _action;
    private DateTime?    _date;
    private Window?      _owner;
    private object?      _tag;
    private DataGridRow? _gridRow;

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
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColDescription", Width = 280, IsReadOnly = true, Visibility = Visibility.Collapsed)]
    public string? Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColPriority", Width = 50, IsReadOnly = true)]
    public int Priority
    {
        get => _priority;
        set
        {
            _priority = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColProgress", Width = 60, IsReadOnly = true)]
    public ushort Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColAction", Width = 100, IsReadOnly = true)]
    public Button? Action
    {
        get => _action;
        set
        {
            _action = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColDate", Width = int.MaxValue, IsReadOnly = true)]
    public DateTime? Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColOwner", IsReadOnly = true, Visibility = Visibility.Collapsed)]
    public Window? Owner
    {
        get => _owner;
        set
        {
            _owner = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColGridRow", IsReadOnly = true, Visibility = Visibility.Collapsed)]
    public DataGridRow? GridRow
    {
        get => _gridRow;
        set
        {
            _gridRow = value;
            OnPropertyChanged();
        }
    }

    [GridColumn(Name = "ColTag", IsReadOnly = true, Visibility = Visibility.Collapsed)]
    public object? Tag
    {
        get => _tag;
        set
        {
            _tag = value;
            OnPropertyChanged();
        }
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}
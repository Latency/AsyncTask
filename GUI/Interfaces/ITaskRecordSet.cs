﻿// ****************************************************************************
// Project:  GUI
// File:     ITaskRecordSet.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Controls;

namespace ORM_Monitor.Interfaces;

/// <summary>
///     IServiceTask
/// </summary>
public interface ITaskRecordSet
{
    /// <summary>
    ///     ID
    /// </summary>
    int ID { get; set; }

    /// <summary>
    ///     Description
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     Priority
    /// </summary>
    int Priority { get; set; }

    /// <summary>
    ///     Progress
    /// </summary>
    ushort Progress { get; set; }

    /// <summary>
    ///     Action
    /// </summary>
    Button? Action { get; set; }

    /// <summary>
    ///     Date
    /// </summary>
    DateTime? Date { get; set; }

    /// <summary>
    ///     Owner
    /// </summary>
    Window? Owner { get; set; }

    /// <summary>
    ///     View
    /// </summary>
    DataGridRow? GridRow { get; set; }

    /// <summary>
    ///     Tag
    /// </summary>
    object? Tag { get; set; }
}
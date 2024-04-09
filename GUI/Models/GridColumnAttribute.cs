// ****************************************************************************
// Project:  GUI
// File:     GridColumnAttribute.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Windows;

namespace ORM_Monitor.Models;

[AttributeUsage(AttributeTargets.Property)]
public class GridColumnAttribute : Attribute
{
    public int Width { get; set; }

    public bool IsReadOnly { get; set; }

    public Visibility Visibility { get; set; }

    public string? Header { get; set; }

    public bool IsEnabled { get; set; }

    public string? Name { get; set; }
}
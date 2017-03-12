//  *****************************************************************************
//  File:       GridColumnAttribute.cs
//  Solution:   ORM-Monitor
//  Project:    GUI
//  Date:       03/05/2017
//  Author:     Latency McLaughlin
//  Copywrite:  Bio-Hazard Industries - 1998-2017
//  *****************************************************************************

using System;
using System.Windows;

namespace ORM_Monitor {
  [AttributeUsage(AttributeTargets.Property)]
  public class GridColumnAttribute : Attribute {
    public int Width { get; set; }

    public bool IsReadOnly { get; set; }

    public Visibility Visibility { get; set; }

    public string Header { get; set; }

    public bool IsEnabled { get; set; }

    public string Name { get; set; }
  }
}
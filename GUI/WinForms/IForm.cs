//  *****************************************************************************
//  File:      IForm.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using ORM_Monitor;
using Telerik.WinControls.UI;

namespace GUI.WinForms {
  internal interface IForm {
    RadLabelElement StatusBar { get; set; }
    RadLabelElement StatusControl { get; set; }
  }
}
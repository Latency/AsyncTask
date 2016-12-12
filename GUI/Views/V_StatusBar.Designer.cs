//  *****************************************************************************
//  File:      V_StatusBar.Designer.cs
//  Solution:  ORM-Monitor
//  Project:   GUI
//  Date:      01/03/2016
//  Author:    Latency McLaughlin
//  Copywrite: Bio-Hazard Industries - 1998-2016
//  *****************************************************************************

using System.Windows.Forms;
using BrightIdeasSoftware;
using GUI.WinForms;

namespace GUI.Views {
  internal sealed partial class V_StatusBar : Control {
    #region Properties
    // -----------------------------------------------------------------------

    private RadForm1 Context { get; }
    public ObjectListView Controller { get; }

    // -----------------------------------------------------------------------
    #endregion Properties
  }
}

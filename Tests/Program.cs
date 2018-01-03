// **************************************************************************
// File:      Program.cs
// Solution:  ORM-Monitor
// Project:   Tests
// Date:      01/02/2018
// Author:    Latency McLaughlin
// Copywrite: Bio-Hazard Industries - 1998-2018
// ***************************************************************************

using System;

namespace Tests {
  internal class Program {
    [STAThread]
    static void Main() {
      var t = new T_Async_IO();

      t.Setup();
      t.Monitor_Test();
      t.TearDown();
    }
  }
}
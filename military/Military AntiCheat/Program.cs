// Decompiled with JetBrains decompiler
// Type: WindowsFormsApp1.Program
// Assembly: Military AntiCheat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C83463F-71F0-4389-A27B-D75DAC99C0E7
// Assembly location: D:\steam\steamapps\common\rocford\Military AntiCheat.exe

using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Form1());
    }
  }
}

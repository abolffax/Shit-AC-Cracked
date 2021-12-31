// Decompiled with JetBrains decompiler
// Type: RFSync.Program
// Assembly: RFSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AAE9F6E8-918E-4CB9-902E-F76FAF7F6E76
// Assembly location: D:\steam\steamapps\common\rocford\RFSync.exe

using System;
using System.Windows.Forms;

namespace RFSync
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

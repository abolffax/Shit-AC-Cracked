// Decompiled with JetBrains decompiler
// Type: WindowsFormsApp1.Properties.Resources
// Assembly: Military AntiCheat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C83463F-71F0-4389-A27B-D75DAC99C0E7
// Assembly location: D:\steam\steamapps\common\rocford\Military AntiCheat.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WindowsFormsApp1.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (WindowsFormsApp1.Properties.Resources.resourceMan == null)
          WindowsFormsApp1.Properties.Resources.resourceMan = new ResourceManager("WindowsFormsApp1.Properties.Resources", typeof (WindowsFormsApp1.Properties.Resources).Assembly);
        return WindowsFormsApp1.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => WindowsFormsApp1.Properties.Resources.resourceCulture;
      set => WindowsFormsApp1.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap launcher => (Bitmap) WindowsFormsApp1.Properties.Resources.ResourceManager.GetObject(nameof (launcher), WindowsFormsApp1.Properties.Resources.resourceCulture);
  }
}

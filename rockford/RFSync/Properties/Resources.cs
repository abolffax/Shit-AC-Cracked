// Decompiled with JetBrains decompiler
// Type: RFSync.Properties.Resources
// Assembly: RFSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AAE9F6E8-918E-4CB9-902E-F76FAF7F6E76
// Assembly location: D:\steam\steamapps\common\rocford\RFSync.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RFSync.Properties
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
        if (RFSync.Properties.Resources.resourceMan == null)
          RFSync.Properties.Resources.resourceMan = new ResourceManager("RFSync.Properties.Resources", typeof (RFSync.Properties.Resources).Assembly);
        return RFSync.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => RFSync.Properties.Resources.resourceCulture;
      set => RFSync.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap alpha_bg => (Bitmap) RFSync.Properties.Resources.ResourceManager.GetObject(nameof (alpha_bg), RFSync.Properties.Resources.resourceCulture);
  }
}

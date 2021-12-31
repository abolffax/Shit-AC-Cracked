// Decompiled with JetBrains decompiler
// Type: RFSync.PostObject
// Assembly: RFSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AAE9F6E8-918E-4CB9-902E-F76FAF7F6E76
// Assembly location: D:\steam\steamapps\common\rocford\RFSync.exe

namespace RFSync
{
  public class PostObject
  {
    public string key;
    public string val;

    public PostObject(string key, string val)
    {
      this.key = key;
      this.val = val;
    }

    public string GetString() => this.key + "=" + this.val;
  }
}

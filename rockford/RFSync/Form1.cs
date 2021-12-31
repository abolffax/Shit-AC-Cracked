// Decompiled with JetBrains decompiler
// Type: RFSync.Form1
// Assembly: RFSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AAE9F6E8-918E-4CB9-902E-F76FAF7F6E76
// Assembly location: D:\steam\steamapps\common\rocford\RFSync.exe

using RFSync.Properties;
using Steamworks;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RFSync
{
  public class Form1 : Form
  {
    private IContainer components;
    private ProgressBar progressBar1;
    private Label stats;
    private Button button1;

    public Form1() => this.InitializeComponent();

    [DllImport("iphlpapi.dll", CharSet = CharSet.Ansi)]
    public static extern int GetAdaptersInfo(IntPtr intptr0, ref long long0);

    internal static string GetMac()
    {
      string str = string.Empty;
      try
      {
        long long0 = (long) Marshal.SizeOf(typeof (Form1.IpAdapterInfo));
        IntPtr num = Marshal.AllocHGlobal(new IntPtr(long0));
        int adaptersInfo = Form1.GetAdaptersInfo(num, ref long0);
        if (adaptersInfo == 111)
        {
          num = Marshal.ReAllocHGlobal(num, new IntPtr(long0));
          adaptersInfo = Form1.GetAdaptersInfo(num, ref long0);
        }
        if (adaptersInfo == 0)
        {
          Form1.IpAdapterInfo structure = (Form1.IpAdapterInfo) Marshal.PtrToStructure(num, typeof (Form1.IpAdapterInfo));
          for (int index = 0; (long) index < (long) structure.AddressLength; ++index)
            str += structure.Address[index].ToString("X2");
          Marshal.FreeHGlobal(num);
        }
        else
          Marshal.FreeHGlobal(num);
      }
      catch
      {
      }
      if (str == string.Empty)
        str = "";
      return str;
    }

    private static string GetProcessorId()
    {
      string empty = string.Empty;
      try
      {
        foreach (ManagementObject instance in new ManagementClass("Win32_Processor").GetInstances())
        {
          if (!(empty != string.Empty))
          {
            try
            {
              empty = instance.Properties["ProcessorId"].Value.ToString();
              if (empty.Length == 0)
                empty = string.Empty;
              else
                break;
            }
            catch
            {
            }
          }
        }
      }
      catch
      {
      }
      return empty;
    }

    private static string GetMacAddress()
    {
      string str = string.Empty;
      try
      {
        str = Form1.GetMac();
        if (str.Length == 0)
        {
          str = string.Empty;
          foreach (ManagementObject instance in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
          {
            if (!(str != string.Empty))
            {
              try
              {
                if (instance["IPEnabled"] != null)
                {
                  if ((bool) instance["IPEnabled"])
                  {
                    if (instance["MacAddress"] != null)
                    {
                      if (instance["MacAddress"].ToString().Length > 0)
                      {
                        str = instance["MacAddress"].ToString().ToUpper();
                        str = str.Replace(":", "");
                      }
                    }
                  }
                }
              }
              catch
              {
              }
            }
            else
              break;
          }
        }
      }
      catch
      {
      }
      return str;
    }

    private static string GetMotherBoardId()
    {
      try
      {
        return Form1.GetProduct() + "-" + Form1.GetManufacturer() + "-" + Form1.GetSerialNumber();
      }
      catch
      {
        return string.Empty;
      }
    }

    private static string GetManufacturer()
    {
      try
      {
        string empty = string.Empty;
        foreach (ManagementObject instance in new ManagementClass("Win32_BaseBoard").GetInstances())
        {
          try
          {
            if (empty == string.Empty)
            {
              if (instance.Properties["Manufacturer"].Value != null)
              {
                empty = instance.Properties["Manufacturer"].Value.ToString();
                if (empty.Length == 0)
                  empty = string.Empty;
                else
                  break;
              }
            }
          }
          catch
          {
          }
        }
        return empty;
      }
      catch
      {
        return string.Empty;
      }
    }

    private static string GetSerialNumber()
    {
      try
      {
        string empty = string.Empty;
        foreach (ManagementObject instance in new ManagementClass("Win32_BaseBoard").GetInstances())
        {
          try
          {
            if (empty == string.Empty)
            {
              if (instance.Properties["SerialNumber"].Value != null)
              {
                empty = instance.Properties["SerialNumber"].Value.ToString();
                if (empty.Length == 0)
                  empty = string.Empty;
                else
                  break;
              }
            }
          }
          catch
          {
          }
        }
        return empty;
      }
      catch
      {
        return string.Empty;
      }
    }

    private static string GetProduct()
    {
      try
      {
        string empty = string.Empty;
        foreach (ManagementObject instance in new ManagementClass("Win32_BaseBoard").GetInstances())
        {
          try
          {
            if (instance.Properties["Product"].Value != null)
            {
              if (empty == string.Empty)
              {
                empty = instance.Properties["Product"].Value.ToString();
                if (empty.Length == 0)
                  empty = string.Empty;
                else
                  break;
              }
            }
          }
          catch
          {
          }
        }
        return empty;
      }
      catch
      {
        return string.Empty;
      }
    }

    private static string GetDiskId()
    {
      try
      {
        ArrayList arrayList = new ArrayList();
        foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get())
        {
          if (managementObject["DeviceID"] != null && managementObject["InterfaceType"] != null && !(managementObject["InterfaceType"].ToString() == "USB") && !(managementObject["InterfaceType"].ToString() == "1394") && (managementObject["MediaType"] == null ? 1 : (!(managementObject["MediaType"].ToString() == "Removable Media") ? 1 : 0)) != 0)
          {
            object obj = managementObject["SerialNumber"];
            if (obj != null && obj.ToString().Trim() != string.Empty && (int) obj.ToString()[0] != (int) Convert.ToChar(31))
              return obj.ToString().Trim();
            arrayList.Add((object) managementObject["DeviceID"].ToString());
          }
        }
        ManagementObjectCollection objectCollection = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia").Get();
        foreach (string str in arrayList)
        {
          foreach (ManagementObject managementObject in objectCollection)
          {
            if (managementObject["Tag"] != null && !(managementObject["Tag"].ToString() != str) && managementObject["SerialNumber"] != null)
            {
              object obj = managementObject["SerialNumber"];
              if (obj != null)
              {
                if (obj.ToString() != string.Empty)
                {
                  if ((int) obj.ToString()[0] != (int) Convert.ToChar(31))
                    return obj.ToString().Trim().Replace(" ", "");
                  break;
                }
                break;
              }
              break;
            }
          }
        }
      }
      catch
      {
        return string.Empty;
      }
      return string.Empty;
    }

    private string md5(string source)
    {
      using (MD5 md5 = MD5.Create())
      {
        byte[] bytes = Encoding.UTF8.GetBytes(source);
        return BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
      }
    }

    public string PostToUrl(string url, PostObject[] objects)
    {
      string str = "";
      for (int index = 0; index < objects.Length; ++index)
        str = str + objects[index].GetString() + "&";
      string data = str.Substring(0, str.Length - 1);
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
          return webClient.UploadString(url, data);
        }
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    public void start()
    {
      if (SteamAPI.Init())
      {
        string val1 = SteamUser.GetSteamID().ToString();
        string personaName = SteamFriends.GetPersonaName();
        string val2 = this.md5(Form1.GetMac() + Form1.GetMacAddress());
        string val3 = this.md5(Form1.GetProcessorId());
        string val4 = this.md5(Form1.GetMotherBoardId() + Form1.GetManufacturer() + Form1.GetSerialNumber() + Form1.GetProduct());
        string val5 = this.md5(Form1.GetDiskId());
        this.BeginInvoke((Delegate) (() => this.stats.Text = "درحال اتصال..."));
        PostObject[] objects = new PostObject[7]
        {
          new PostObject("steamid", val1),
          new PostObject("name", personaName),
          new PostObject("mac", val2),
          new PostObject("cpuid", val3),
          new PostObject("mbid", val4),
          new PostObject("hdid", val5),
          new PostObject("token", this.md5(val1 + val3 + val5 + Encoding.UTF8.GetString(Convert.FromBase64String("cmZzMjM="))))
        };
        if (this.PostToUrl(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovLzUuNjMuMTAuOTgvcmZzeW5jL3NhdmUucGhw")), objects) == "ok")
        {
          this.BeginInvoke((Delegate) (() => this.stats.Text = "حالا میتوانید وارد سرور شوید"));
          this.BeginInvoke((Delegate) (() =>
          {
            this.progressBar1.Style = ProgressBarStyle.Blocks;
            this.progressBar1.Value = 100;
          }));
        }
        else
          this.BeginInvoke((Delegate) (() => this.stats.Text = "سیستم شما بن شده است"));
      }
      else
      {
        int num = (int) MessageBox.Show("لطفا استیم خود را باز کنید");
        this.BeginInvoke((Delegate) (() => this.Close()));
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.stats.Text = "درحال دریافت اطلاعات";
      new Thread(new ThreadStart(this.start)).Start();
    }

    private void button1_Click(object sender, EventArgs e) => this.Close();

    private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.progressBar1 = new ProgressBar();
      this.stats = new Label();
      this.button1 = new Button();
      this.SuspendLayout();
      this.progressBar1.Location = new Point(12, 306);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(332, 10);
      this.progressBar1.Step = 1;
      this.progressBar1.Style = ProgressBarStyle.Marquee;
      this.progressBar1.TabIndex = 0;
      this.progressBar1.Value = 10;
      this.stats.AutoSize = true;
      this.stats.BackColor = Color.Transparent;
      this.stats.Font = new Font("Tahoma", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.stats.ForeColor = SystemColors.HighlightText;
      this.stats.Location = new Point(134, 275);
      this.stats.Name = "stats";
      this.stats.Size = new Size(84, 18);
      this.stats.TabIndex = 1;
      this.stats.Text = "درحال اتصال";
      this.stats.TextAlign = ContentAlignment.MiddleCenter;
      this.button1.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.button1.Location = new Point(137, 325);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "بستن";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.BackgroundImage = (Image) Resources.alpha_bg;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(360, 360);
      this.ControlBox = false;
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.stats);
      this.Controls.Add((Control) this.progressBar1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Form1);
      this.Text = "RFSync";
      this.Load += new EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    internal struct IpAddrString
    {
      internal IntPtr Next;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
      internal string IpAddress;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
      internal string IpMask;
      internal uint Context;
    }

    internal struct IpAdapterInfo
    {
      internal const int MaxAdapterDescriptionLength = 128;
      internal const int MaxAdapterNameLength = 256;
      internal const int MaxAdapterAddressLength = 8;
      internal IntPtr Next;
      internal uint ComboIndex;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      internal string AdapterName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
      internal string Description;
      internal uint AddressLength;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
      internal byte[] Address;
      internal uint Index;
      internal Form1.OldInterfaceType Type;
      internal bool DhcpEnabled;
      internal IntPtr CurrentIpAddress;
      internal Form1.IpAddrString IpAddressList;
      internal Form1.IpAddrString GatewayList;
      internal Form1.IpAddrString DhcpServer;
      [MarshalAs(UnmanagedType.Bool)]
      internal bool HaveWins;
      internal Form1.IpAddrString PrimaryWinsServer;
      internal Form1.IpAddrString SecondaryWinsServer;
      internal uint LeaseObtained;
      internal uint LeaseExpires;
    }

    internal enum OldInterfaceType
    {
      Unknown = 0,
      Ethernet = 6,
      TokenRing = 9,
      Fddi = 15, // 0x0000000F
      Ppp = 23, // 0x00000017
      Loopback = 24, // 0x00000018
      Slip = 28, // 0x0000001C
    }
  }
}

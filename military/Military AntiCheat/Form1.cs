// Decompiled with JetBrains decompiler
// Type: WindowsFormsApp1.Form1
// Assembly: Military AntiCheat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C83463F-71F0-4389-A27B-D75DAC99C0E7
// Assembly location: D:\steam\steamapps\common\rocford\Military AntiCheat.exe

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Steamworks;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
  public class Form1 : Form
  {
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 256;
    private const int WM_SYSKEYDOWN = 260;
    private const int WM_KEYUP = 257;
    private const int WM_SYSKEYUP = 261;
    private static IntPtr _hookID = IntPtr.Zero;
    private static SharpDX.Direct3D9.Device _device;
    private static SharpDX.Direct3D9.Surface _surface;
    private static byte[] _buffer;
    public static string ServerIp = "5.63.10.220";
    public static int Width = 0;
    public static int Height = 0;
    public static bool onCapture = false;
    public static string version = "4";
    public static string steamid;
    public static Thread checker;
    private IContainer components;
    private Label label1;
    private Button button1;

    public Form1() => this.InitializeComponent();

    public static string CreateMD5(string input)
    {
      using (MD5 md5 = MD5.Create())
      {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(bytes);
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < hash.Length; ++index)
          stringBuilder.Append(hash[index].ToString("X2"));
        return stringBuilder.ToString();
      }
    }

    private static string ping_server(string message)
    {
      try
      {
        int port = 35364;
        TcpClient tcpClient = new TcpClient(Form1.ServerIp, port);
        tcpClient.SendTimeout = 2000;
        tcpClient.ReceiveTimeout = 2000;
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(bytes, 0, bytes.Length);
        byte[] numArray = new byte[256];
        string empty = string.Empty;
        int count = stream.Read(numArray, 0, numArray.Length);
        string str = Encoding.ASCII.GetString(numArray, 0, count);
        stream.Close();
        tcpClient.Close();
        return str;
      }
      catch (ArgumentNullException ex)
      {
      }
      catch (SocketException ex)
      {
      }
      return "0";
    }

    public static void SendMe()
    {
      try
      {
        string s = DateTime.Now.ToString("yyyy");
        string md5 = Form1.CreateMD5((long.Parse(Form1.steamid) + (long) int.Parse(s)).ToString() + "gwambdd");
        if (Form1.ping_server(Form1.version + ":" + Form1.steamid + ":" + md5).Equals("TAKE_SCR") && Form1.GetActiveWindowTitle().StartsWith("FiveM"))
          new Thread(new ThreadStart(Form1.capture2)).Start();
        Console.WriteLine("Ping Sended");
      }
      catch (Exception ex)
      {
      }
    }

    public static void RunMe()
    {
      while (true)
      {
        new Thread(new ThreadStart(Form1.SendMe)).Start();
        Thread.Sleep(3000);
      }
    }

    public void start()
    {
      if (SteamAPI.Init())
      {
        Form1.steamid = SteamUser.GetSteamID().ToString();
        string s = DateTime.Now.ToString("yyyy");
        string md5 = Form1.CreateMD5((long.Parse(Form1.steamid) + (long) int.Parse(s)).ToString() + "gwambdd");
        string str = Form1.ping_server(Form1.version + ":" + Form1.steamid + ":" + md5);
        Console.WriteLine(str);
        if (str == "0")
        {
          int num = (int) MessageBox.Show("Server Error");
          this.BeginInvoke((Delegate) (() => this.Close()));
        }
        else if (str == "NO_VERSION")
        {
          int num = (int) MessageBox.Show("Please Update Your Launcher");
          this.BeginInvoke((Delegate) (() => this.Close()));
        }
        else if (str == "INC")
        {
          int num = (int) MessageBox.Show("Incorrent Version - Check Your System Date");
          this.BeginInvoke((Delegate) (() => this.Close()));
        }
        else
        {
          if (!(str == "OK") && !(str == "TAKE_SCR"))
            return;
          Form1.checker = new Thread(new ThreadStart(Form1.RunMe));
          Form1.checker.Start();
          this.label1.BeginInvoke((Delegate) (() => this.label1.Text = "Now You Can Join Server"));
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Please Open Your Steam");
        this.BeginInvoke((Delegate) (() => this.Close()));
      }
    }

    public static void test()
    {
      int num = 1;
      while (true)
        ++num;
    }

    public static void test2()
    {
      new Thread(new ThreadStart(Form1.test)).Start();
      for (int index = 1; index <= 1000; ++index)
      {
        Console.WriteLine("T2 : " + index.ToString());
        Thread.Sleep(100);
      }
    }

    private void Form1_Load(object sender, EventArgs e) => new Thread(new ThreadStart(this.start)).Start();

    protected override void OnClosed(EventArgs e)
    {
      if (Form1.checker == null || !Form1.checker.IsAlive)
        return;
      Form1.checker.Abort();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    public static string get_all_windows()
    {
      Process[] processes = Process.GetProcesses();
      string str = "";
      foreach (Process process in processes)
      {
        if (!string.IsNullOrEmpty(process.MainWindowTitle))
          str = str + process.ProcessName + " : " + process.MainWindowTitle + "\n";
      }
      return str;
    }

    private static string GetActiveWindowTitle()
    {
      StringBuilder text = new StringBuilder(256);
      return Form1.GetWindowText(Form1.GetForegroundWindow(), text, 256) > 0 ? text.ToString() : "";
    }

    public static Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
    {
      Image image = Image.FromFile(stPhotoPath);
      int width1 = image.Width;
      int height1 = image.Height;
      if (width1 < height1)
      {
        int num = newWidth;
        newWidth = newHeight;
        newHeight = num;
      }
      int x1 = 0;
      int y1 = 0;
      int x2 = 0;
      int y2 = 0;
      float num1 = (float) newWidth / (float) width1;
      float num2 = (float) newHeight / (float) height1;
      float num3;
      if ((double) num2 < (double) num1)
      {
        num3 = num2;
        x2 = (int) Convert.ToInt16((float) (((double) newWidth - (double) width1 * (double) num3) / 2.0));
      }
      else
      {
        num3 = num1;
        y2 = (int) Convert.ToInt16((float) (((double) newHeight - (double) height1 * (double) num3) / 2.0));
      }
      int width2 = (int) ((double) width1 * (double) num3);
      int height2 = (int) ((double) height1 * (double) num3);
      Bitmap bitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
      bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.Clear(Color.Black);
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.DrawImage(image, new Rectangle(x2, y2, width2, height2), new Rectangle(x1, y1, width1, height1), GraphicsUnit.Pixel);
      graphics.Dispose();
      image.Dispose();
      bitmap.Save(stPhotoPath);
      return (Image) bitmap;
    }

    public static void capture2()
    {
      try
      {
        Form1.onCapture = true;
        Factory1 factory1 = new Factory1();
        Adapter1 adapter1 = factory1.GetAdapter1(0);
        SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device((Adapter) adapter1);
        Output1 output1 = adapter1.GetOutput(0).QueryInterface<Output1>();
        int width = Screen.PrimaryScreen.Bounds.Width;
        int height = Screen.PrimaryScreen.Bounds.Height;
        Texture2DDescription description = new Texture2DDescription()
        {
          CpuAccessFlags = CpuAccessFlags.Read,
          BindFlags = BindFlags.None,
          Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
          Width = width,
          Height = height,
          OptionFlags = ResourceOptionFlags.None,
          MipLevels = 1,
          ArraySize = 1,
          SampleDescription = {
            Count = 1,
            Quality = 0
          },
          Usage = ResourceUsage.Staging
        };
        Texture2D texture2D1 = new Texture2D(device, description);
        OutputDuplication outputDuplication = output1.DuplicateOutput((IUnknown) device);
        bool flag = false;
        int num1 = 0;
        while (!flag)
        {
          SharpDX.DXGI.Resource desktopResourceOut;
          outputDuplication.AcquireNextFrame(10000, out OutputDuplicateFrameInformation _, out desktopResourceOut);
          if (num1 > 0)
          {
            using (Texture2D texture2D2 = desktopResourceOut.QueryInterface<Texture2D>())
              device.ImmediateContext.CopyResource((SharpDX.Direct3D11.Resource) texture2D2, (SharpDX.Direct3D11.Resource) texture2D1);
            DataBox dataBox = device.ImmediateContext.MapSubresource((SharpDX.Direct3D11.Resource) texture2D1, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            IntPtr num2 = dataBox.DataPointer;
            IntPtr num3 = bitmapdata.Scan0;
            for (int index = 0; index < height; ++index)
            {
              Utilities.CopyMemory(num3, num2, width * 4);
              num2 = IntPtr.Add(num2, dataBox.RowPitch);
              num3 = IntPtr.Add(num3, bitmapdata.Stride);
            }
            bitmap.UnlockBits(bitmapdata);
            device.ImmediateContext.UnmapSubresource((SharpDX.Direct3D11.Resource) texture2D1, 0);
            bitmap.Save("ScreenCapture.jpg");
            flag = true;
          }
          desktopResourceOut.Dispose();
          outputDuplication.ReleaseFrame();
          ++num1;
        }
        outputDuplication.Dispose();
        output1.Dispose();
        factory1.Dispose();
        adapter1.Dispose();
        device.Dispose();
        Form1.resizeImage(800, 500, "ScreenCapture.jpg");
        int port = 35365;
        TcpClient tcpClient = new TcpClient(Form1.ServerIp, port);
        NetworkStream stream = tcpClient.GetStream();
        byte[] bytes1 = Encoding.ASCII.GetBytes(Form1.steamid + "\n");
        stream.Write(bytes1, 0, bytes1.Length);
        using (FileStream fileStream = new FileStream("ScreenCapture.jpg", FileMode.Open, FileAccess.Read))
        {
          byte[] buffer = new byte[fileStream.Length];
          int length = (int) fileStream.Length;
          int offset = 0;
          int num4;
          for (; length > 0; length -= num4)
          {
            num4 = fileStream.Read(buffer, offset, length);
            if (num4 != 0)
              offset += num4;
            else
              break;
          }
          fileStream.Close();
          File.Delete("ScreenCapture.jpg");
          byte[] bytes2 = Encoding.ASCII.GetBytes(buffer.Length.ToString() + "\n");
          stream.Write(bytes2, 0, bytes2.Length);
          Thread.Sleep(500);
          stream.Write(buffer, 0, buffer.Length);
          Thread.Sleep(500);
          byte[] bytes3 = Encoding.ASCII.GetBytes(Form1.get_all_windows());
          stream.Write(bytes3, 0, bytes3.Length);
        }
        stream.Close();
        tcpClient.Close();
      }
      catch (Exception ex)
      {
      }
      Form1.onCapture = false;
    }

    private void button1_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.label1 = new Label();
      this.button1 = new Button();
      this.SuspendLayout();
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Arial Narrow", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(75, 279);
      this.label1.Name = "label1";
      this.label1.Size = new Size(197, 26);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loading...";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.button1.BackColor = Color.Black;
      this.button1.FlatStyle = FlatStyle.Popup;
      this.button1.ForeColor = Color.LightGray;
      this.button1.Location = new Point(12, 315);
      this.button1.Name = "button1";
      this.button1.Size = new Size(41, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Close";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ActiveCaptionText;
      this.BackgroundImage = (Image) Resources.launcher;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(350, 350);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Form1);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "MilitaryAntiCheat";
      this.Load += new EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
  }
}

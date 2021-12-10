using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HGMC.Source.HardwareMonitor;
using HGMC.Source.TCP;
using GI.Screenshot;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Windows.Forms;
using System.Windows.Interop;
using Image = System.Drawing.Image;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using MouseEventHandler = System.Windows.Input.MouseEventHandler;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace HGMC
{
  /// <summary>
  /// A screen capture class
  /// </summary>
  public partial class MyCapture
  {
    /// <summary>
    /// Capture screen and return a Bitmap
    /// </summary>
    /// <param name="rect">Rect which you want to capture</param>
    /// <returns>Bitmap</returns>
    public static Bitmap Capture(Rect rect)
    {
      var bitmap = new Bitmap((int)rect.Width, (int)rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

      var graphics = Graphics.FromImage((Image)bitmap);
      var x = (int)rect.X;
      var y = (int)rect.Y;
      var size = rect.Size;
      var width = (int)size.Width;
      size = rect.Size;
      var height = (int)size.Height;
      var blockRegionSize = new System.Drawing.Size(width, height);
      graphics.CopyFromScreen(x, y, 0, 0, blockRegionSize, CopyPixelOperation.SourceCopy);
      return bitmap;
    }

    /// <summary>
    /// Save the Bitmap into memory with designate format
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <param name="fmt">ImageFormat</param>
    /// <param name="size">Size</param>
    /// <returns></returns>
    public static MemoryStream SaveBitMapToMemoryStream(Bitmap bitmap, ImageFormat fmt, Size size)
    {
      var bitmapRaw = new Bitmap(bitmap, size);
      var bitmap565 = new Bitmap(size.Width, size.Height, PixelFormat.Format16bppRgb565);

      var g = Graphics.FromImage((Image)bitmap565);
      g.DrawImage(bitmapRaw, new Point(0, 0));
      g.Dispose();

      var hBitmap = bitmap565.GetHbitmap();
      Image img = Image.FromHbitmap(hBitmap);
      var imgMem = new MemoryStream();
      img.Save(imgMem, fmt);
      return imgMem;
    }
  }

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private static readonly int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
    private static readonly int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;

    private const int SrHeight = 135;
    private const int SrWidth = 240;

    private static MemoryStream _imgMemory;
    private static BitmapSource _imageBitmapSource;
    private static Bitmap _imageBitmap;
    private static Image _image;
    private Thread _captureThread = new Thread(capture_thread);

    /// <summary>
    /// capture_thread
    /// </summary>
    private static void capture_thread()
    {
      while (true)
      {
        var rect = new Rect(0, 0, ScreenWidth, ScreenHeight);
        _imageBitmap = MyCapture.Capture(rect);
        _imgMemory = MyCapture.SaveBitMapToMemoryStream(_imageBitmap, ImageFormat.Jpeg, new Size(SrWidth, SrHeight));
        _imageBitmapSource = _imageBitmap.ToBitmapSource();
        //_imageBitmapSource = Screenshot.CaptureRegion(rect);
        Thread.Sleep(10);
      }
      // ReSharper disable once FunctionNeverReturns
    }

    public static HardwareMonitor HardwareMonitor;
    private static TcpClientUtil _tcu = new TcpClientUtil();
    public static Thread Tcur = new Thread(() => { _tcu.ReceiveMsgThread(); });
    public static Thread Tcuw;

    /// <summary>
    /// Main entrance
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();

      var paletteHelper = new PaletteHelper();
      var theme = paletteHelper.GetTheme();

      theme.SetBaseTheme(Theme.Light);
      theme.PrimaryMid = new ColorPair(Colors.Teal, Colors.WhiteSmoke);
      paletteHelper.SetTheme(theme);

      _imageBitmapSource = Screenshot.CaptureAllScreens();
      var showThread = new Thread(() =>
      {
        while (true)
        {
          Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate()
          {
            if (ScrCapImg != null)
              ScrCapImg.Source = _imageBitmapSource;
          });
          Thread.Sleep(10);
        }
      });

      showThread.Start();


      notifyIcon.BalloonTipText = "HGMC运行中..."; //托盘气泡显示内容
      notifyIcon.Text = "HGMC";
      notifyIcon.Visible = true; //托盘按钮是否可见
      notifyIcon.Icon = new System.Drawing.Icon("m.ico"); //托盘中显示的图标
      notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
      //notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
      var exit = new System.Windows.Forms.MenuItem("关闭");
      exit.Click += new EventHandler(exit_Click);
      var childen = new System.Windows.Forms.MenuItem[] { exit };
      notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
      this.StateChanged += MainWindow_StateChanged;
    }

    /// <summary>
    /// Update Monitor view
    /// </summary>
    private void update_monitor()
    {
      while (true)
      {
        Dispatcher.BeginInvoke(DispatcherPriority.Background,
          (Action)delegate() { _tcu.SendMsg(HardwareMonitor.UpdateHardware()); });
        Thread.Sleep(2000);
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      Environment.Exit(0);
    }

    private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      DragMove();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
    {
      // Todo: 
      Close();
    }

    NotifyIcon notifyIcon = new NotifyIcon();

    private void minimizeBtn_Click(object sender, RoutedEventArgs e)
    {
      this.Visibility = Visibility.Hidden;
      notifyIcon.ShowBalloonTip(1000); //托盘气泡显示时间
    }

    // 退出选项
    private void exit_Click(object sender, EventArgs e)
    {
      if (System.Windows.MessageBox.Show("确定退出吗?",
            "HGMC",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.Yes) == MessageBoxResult.Yes)
      {
        Environment.Exit(0);
      }
    }

    private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      //鼠标左键，实现窗体最小化隐藏或显示窗体
      if (e.Button == MouseButtons.Left)
      {
        if (this.Visibility == Visibility.Visible)
        {
          this.Visibility = Visibility.Hidden;
          this.ShowInTaskbar = false;
        }
        else
        {
          this.Visibility = Visibility.Visible;
          //解决最小化到任务栏可以强行关闭程序的问题。
          this.ShowInTaskbar = true;
          this.Activate();
        }
      }
    }

    private void MainWindow_StateChanged(object sender, EventArgs e)
    {
      if (this.WindowState == WindowState.Minimized)
      {
        this.Visibility = Visibility.Hidden;
      }
    }


    private void closeBtn_Click(object sender, EventArgs e)
    {
      Close();
    }

    bool _flag = false;

    private void sr_Click(object sender, RoutedEventArgs e)
    {
      if (_flag == true)
      {
        //ScreenCaptureArea.Visibility = Visibility.Collapsed;
        _captureThread.Abort();
        _flag = false;
        SwBtn.Content = "开始投屏";
      }
      else
      {
        //ScreenCaptureArea.Visibility = Visibility.Visible;
        _captureThread = new Thread(capture_thread);
        _captureThread.Start();
        _flag = true;
        SwBtn.Content = "关闭投屏";
      }
    }

    private void HardwareArea_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
      {
        RoutedEvent = MouseWheelEvent,
        Source = sender
      };
      //HardwareArea.RaiseEvent(eventArg);
    }

    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
      if (sender is Hyperlink link)
        Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
    }

    private void Monitor_Click(object sender, RoutedEventArgs e)
    {
      new Thread(() =>
      {
        Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate()
        {
          HardwareMonitor = new HardwareMonitor();
          var txt = HardwareMonitor.UpdateHardware();

          const string matchTxt = "{\"Header\":\"HgmTCP\",\"DataType\":\"5\",\"Data\":\"match\"}";
          _tcu.SetServerParams("192.168.213.234", 20);
          _tcu.Connect();
          _tcu.SendMsg(matchTxt);

          if (Tcuw == null)
          {
            Tcuw = new Thread(update_monitor);
            Tcuw.Start();
          }
          else
          {
            Tcuw.Abort();
            Tcuw = null;
          }
        });
      }).Start();
    }
  }
}
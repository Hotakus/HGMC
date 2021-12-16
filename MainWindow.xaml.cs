using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
using HGMC.Source;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using static System.String;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;
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
      var bitmap = new Bitmap((int)rect.Width, (int)rect.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

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
    private Thread _captureThread = new Thread(capture_thread);
    private static ImageFormat _imageFormat = ImageFormat.Jpeg;
    private static ImageFormat _imageFormat2;
    private static PixelFormat _pixelFormat;

    private static int _length = 0;
    private static long _length1 = 0;
    private static int _length2 = 0;

    /// <summary>
    /// capture_thread
    /// </summary>
    private static void capture_thread()
    {
      while (true)
      {
        var rect = new Rect(0, 0, ScreenWidth, ScreenHeight);
        _imageBitmap = MyCapture.Capture(rect);
        _imgMemory = MyCapture.SaveBitMapToMemoryStream(_imageBitmap, _imageFormat, new Size(SrWidth, SrHeight));
        _imageBitmapSource = _imageBitmap.ToBitmapSource();
        //_imageBitmapSource = Screenshot.CaptureRegion(rect);
        Thread.Sleep(10);
        _length = _imgMemory.Capacity;
        _length1 = _imgMemory.Length;

        _imageFormat2 = _imageBitmap.RawFormat;
        _pixelFormat = _imageBitmap.PixelFormat;
      }
      // ReSharper disable once FunctionNeverReturns
    }

    private static readonly HardwareMonitor HardwareMonitor = new HardwareMonitor();
    private static TcpClientUtil _tcu = new TcpClientUtil();
    public static Thread Tcur = null;
    public static Thread Tcuw = null;

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

      _notifyIcon.BalloonTipText = "HGMC运行中..."; //托盘气泡显示内容
      _notifyIcon.Text = "HGMC";
      _notifyIcon.Visible = true; //托盘按钮是否可见
      _notifyIcon.Icon = new System.Drawing.Icon("m.ico"); //托盘中显示的图标
      _notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
      //notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
      var exit = new System.Windows.Forms.MenuItem("关闭");
      exit.Click += new EventHandler(exit_Click);
      var childen = new System.Windows.Forms.MenuItem[] { exit };
      _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
      this.StateChanged += MainWindow_StateChanged;
    }

    ~MainWindow()
    {
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
        Thread.Sleep(1000);
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


    private readonly NotifyIcon _notifyIcon = new NotifyIcon();

    private void minimizeBtn_Click(object sender, RoutedEventArgs e)
    {
      this.Visibility = Visibility.Hidden;
      _notifyIcon.ShowBalloonTip(1000); //托盘气泡显示时间
    }

    // 退出选项
    private static void exit_Click(object sender, EventArgs e)
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
      if (e.Button != MouseButtons.Left)
        return;
      if (this.Visibility == Visibility.Visible)
      {
        this.Visibility = Visibility.Hidden;
        this.ShowInTaskbar = false;
      }
      else
      {
        this.Visibility = Visibility.Visible;
        this.ShowInTaskbar = true;
        this.Activate();
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
      Environment.Exit(0);
    }

    bool _flag = false;
    private Thread _srThread;

    private void sr_Click(object sender, RoutedEventArgs e)
    {
      var fh = new JsonPacks.FrameHead();
      fh.Header = "HgmTCP";

      if (_flag)
      {
        //_captureThread.Abort();
        _srThread.Abort();
        _flag = false;
        SwBtn.Content = "进入投屏";

        fh.DataType = "7";
        var endTxt = JsonConvert.SerializeObject(fh);
        _tcu.SendMsg("end");
      }
      else
      {
        //_captureThread = new Thread(capture_thread);
        //_captureThread.Start();
        _flag = true;
        SwBtn.Content = "退出投屏";

        fh.DataType = "6";
        fh.Data.cf = FmtComboBox.SelectionBoxItem.ToString();
        var beginTxt = JsonConvert.SerializeObject(fh);
        _tcu.SendMsg(beginTxt);

        Thread.Sleep(300);

        _srThread = new Thread(sr_thread);
        _srThread.Start();
      }
    }

    private void sr_thread()
    {
      while (true)
      {
        var rect = new Rect(0, 0, ScreenWidth, ScreenHeight);
        _imageBitmap = MyCapture.Capture(rect);
        _imgMemory = MyCapture.SaveBitMapToMemoryStream(_imageBitmap, _imageFormat, new Size(SrWidth, SrHeight));
        _imageBitmapSource = _imageBitmap.ToBitmapSource();

        const byte head0 = 0x20;
        const byte head1 = 0x21;
        const byte head2 = 0x12;

        byte size0 = (byte)((_imgMemory.Length >> 16) & 0xFF);
        byte size1 = (byte)((_imgMemory.Length >> 8) & 0xFF);
        byte size2 = (byte)((_imgMemory.Length >> 0) & 0xFF);

        byte[] src = _imgMemory.GetBuffer();
        byte[] arr = new byte[_imgMemory.Length + 9];

        /* head */
        arr[0] = head0;
        arr[1] = head1;
        arr[2] = head2;

        /* size */
        arr[3] = size0;
        arr[4] = size1;
        arr[5] = size2;

        for (var i = 0; i < _imgMemory.Length; i++) arr[i + 6] = src[i];

        /* tail */
        arr[arr.Length - 1] = head0;
        arr[arr.Length - 2] = head1;
        arr[arr.Length - 3] = head2;

        Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate() { _tcu.SendMsg(arr, 0, arr.Length); });

        Thread.Sleep(200);
      }
    }


    private void SwBtn2_OnClick(object sender, RoutedEventArgs e)
    {
      var rect = new Rect(0, 0, ScreenWidth, ScreenHeight);
      _imageBitmap = MyCapture.Capture(rect);
      _imgMemory = MyCapture.SaveBitMapToMemoryStream(_imageBitmap, _imageFormat, new Size(SrWidth, SrHeight));
      _imageBitmapSource = _imageBitmap.ToBitmapSource();

      const byte head0 = 0x20;
      const byte head1 = 0x21;
      const byte head2 = 0x12;

      byte size0 = (byte)((_imgMemory.Length >> 16) & 0xFF);
      byte size1 = (byte)((_imgMemory.Length >> 8) & 0xFF);
      byte size2 = (byte)((_imgMemory.Length >> 0) & 0xFF);

      byte[] src = _imgMemory.GetBuffer();
      byte[] arr = new byte[_imgMemory.Length + 9];

      arr[0] = head0;
      arr[1] = head1;
      arr[2] = head2;
      arr[3] = size0;
      arr[4] = size1;
      arr[5] = size2;
      for (var i = 0; i < _imgMemory.Length; i++)
        arr[i + 6] = src[i];
      arr[arr.Length - 1] = head0;
      arr[arr.Length - 2] = head1;
      arr[arr.Length - 3] = head2;


      _tcu.SendMsg(arr, 0, arr.Length);
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

    public static bool HardwareOpenState = false;

    private void Monitor_Click(object sender, RoutedEventArgs e)
    {
      if (!_tcu.Connected())
      {
        if (MessageBox.Show("TCP未连接", "TCP未连接", MessageBoxButton.YesNo, MessageBoxImage.Information) ==
            MessageBoxResult.Yes)
        {
        }
        else
        {
        }
      }

      new Thread(() =>
      {
        if (Tcuw == null)
        {
          HardwareMonitor.Begin();
          Tcuw = new Thread(update_monitor);
          Tcuw.Start();
          Dispatcher.BeginInvoke(DispatcherPriority.Background,
            (Action)delegate() { MonitorCtlBtn.Content = "停止发送硬件信息"; });
        }
        else
        {
          HardwareMonitor.End();
          Tcuw.Abort();
          Tcuw = null;
          Dispatcher.BeginInvoke(DispatcherPriority.Background,
            (Action)delegate() { MonitorCtlBtn.Content = "开始发送硬件信息"; });
        }
      })
      {
        Priority = ThreadPriority.Lowest
      }.Start();
    }


    private void FmtComboBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var value = FmtComboBox.Text;

      switch (value)
      {
        case "JPG":
          _imageFormat = ImageFormat.Jpeg;
          break;
        case "PNG":
          _imageFormat = ImageFormat.Png;
          break;
        case "RAW":
          // TODO:
          break;
      }
    }


    private bool isHGM = false;

    private void HGMMode_OnClick(object sender, RoutedEventArgs e)
    {
      if (HGMMode.IsChecked != null)
        isHGM = (bool)HGMMode.IsChecked;
    }

    private void TcpConnBtn_OnClick(object sender, RoutedEventArgs e)
    {
      var matchTxt = "{\"Header\":\"HgmTCP\",\"DataType\":\"5\",\"Data\":\"match\"}";


      if (_tcu.Connected())
      {
        _tcu.DisConnect();
        TcpConnStatus.Text = "(未连接)";
        TcpConnBtn.Content = "连接";
        //Tcur.Abort();
        //Tcur = null;
      }
      else
      {
        if (Ip0.Text == Empty || Ip1.Text == Empty || Ip2.Text == Empty || Ip3.Text == Empty || Port.Text == Empty)
        {
          return;
        }

        var ip = Ip0.Text + '.' + Ip1.Text + '.' + Ip2.Text + '.' + Ip3.Text;
        var port = Port.Text;
        _tcu.SetServerParams(ip, Convert.ToInt16(port));

        TcpConnStatus.Text = "(正在连接)";
        new Thread(() =>
        {
          _tcu.Connect();
          if (_tcu.Connected())
          {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate()
            {
              TcpConnStatus.Text = "(已连接)";
              TcpConnBtn.Content = "断开";
              if ((bool)HGMMode.IsChecked)
              {
                _tcu.SendMsg(matchTxt);
              }
            });

            // Tcur = new Thread(() =>
            // {
            //   Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate() { _tcu.ReceiveMsgThread(); });
            // });
            // Tcur.Start();
          }
          else
          {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate() { TcpConnStatus.Text = "(未连接)"; });
          }
        }).Start();
      }
    }

    private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventargs)
    {
      //throw new NotImplementedException();
    }
  }

  public class NotEmptyValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      return string.IsNullOrWhiteSpace((value ?? "").ToString())
        ? new ValidationResult(false, null)
        : ValidationResult.ValidResult;
    }
  }
}
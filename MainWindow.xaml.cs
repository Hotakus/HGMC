using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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

namespace HGMC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly int _screenHeight = Screen.PrimaryScreen.Bounds.Height;
        private readonly int _screenWidth = Screen.PrimaryScreen.Bounds.Width;

        public MainWindow()
        {
            InitializeComponent();

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(Theme.Light);
            theme.PrimaryMid = new ColorPair(Colors.Teal, Colors.WhiteSmoke);
            paletteHelper.SetTheme(theme);

            var image = Screenshot.CaptureAllScreens();

            var t = new Thread(() =>
            {
                while (true)
                {
                    image = Screenshot.CaptureRegion(new Rect(0, 0, _screenWidth, _screenHeight));
                    Thread.Sleep(1);
                }
            });
            t.Start();

            var t2 = new Thread(() =>
            {
                while (true)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate () { scrCapImg.Source = image; });
                    Thread.Sleep(1);
                }
            });
            //t2.Start();
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
            Close();
        }

        private void minimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        bool flag = false;
        private void test_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                ScreenCaptureArea.Visibility = Visibility.Visible;
                flag = false;
            }
            else
            {
                ScreenCaptureArea.Visibility = Visibility.Collapsed;
                flag = true;
            }
        }

        private void HardwareArea_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = MouseWheelEvent;
            eventArg.Source = sender;
            HardwareArea.RaiseEvent(eventArg);
        }
    }
}
using System.Threading;
using HGMC.Source.HardwareMonitor;
using HGMC.Source.TCP;

namespace HGMC
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {

    public void Test()
    {
      var hardwareMonitor = new HardwareMonitor();
      var txt = hardwareMonitor.UpdateHardware();
      //Console.WriteLine(txt);

      var tcu = new TcpClientUtil();
      tcu.SetServerParams("10.88.171.96", 8080);
      tcu.Connect();

      tcu.SendMsg("Hello, I am HellGateMonitor's TCP");
      tcu.SendMsg(txt);

      new Thread(() => { tcu.ReceiveMsgThread(); }).Start();
    }
    
    public MainWindow()
    {
      InitializeComponent();
    }
  }
}
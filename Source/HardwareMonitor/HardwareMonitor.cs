using System;
using System.Threading;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;

namespace HGMC.Source.HardwareMonitor
{
  public class UpdateVisitor : IVisitor
  {
    public void VisitComputer(IComputer computer)
    {
      computer.Traverse(this);
    }

    public void VisitHardware(IHardware hardware)
    {
      hardware.Update();
      foreach (var subHardware in hardware.SubHardware)
        subHardware.Accept(this);
    }

    public void VisitSensor(ISensor sensor)
    {
    }

    public void VisitParameter(IParameter parameter)
    {
    }
  }

  public class HardwareMonitor
  {
    private readonly Computer _computer;

    // Hardware Data
    public HardwareJsonPack hardwareJsonPack;

    public string hardwareStr = "";

    private readonly UpdateVisitor _updateVisitor = new UpdateVisitor();

    public Mutex _packMutex = new Mutex(true);
    
    public HardwareMonitor()
    {
      Console.WriteLine("Hardware Monitor");

      _computer = new Computer
      {
        IsCpuEnabled = true,
        IsGpuEnabled = true,
        IsMemoryEnabled = true,
        IsMotherboardEnabled = true,
        IsControllerEnabled = true,
        IsNetworkEnabled = true,
        IsStorageEnabled = true
      };

      _computer.Open();
      
    }

    ~HardwareMonitor()
    {
      _computer.Close();
    }

    public string UpdateHardware()
    {
      _computer.Accept(_updateVisitor);

      _packMutex.WaitOne();
      StartMonitor();
      _packMutex.ReleaseMutex();

      return GetHardwareString();
    }


    private static float FloatConvert(float f, byte b) =>
      Convert.ToSingle(f.ToString("F" + b));

    public string GetHardwareString()
    {
      var tmp = "";
      _packMutex.WaitOne();
      tmp = hardwareStr;
      _packMutex.ReleaseMutex();
      return tmp;
    }

    private void StartMonitor()
    {
      hardwareJsonPack = new HardwareJsonPack();
      hardwareJsonPack.Header = "HgmTCP";
      hardwareJsonPack.DataType = "4";

      foreach (var hardware in _computer.Hardware)
      {
        switch (hardware.HardwareType)
        {
          case HardwareType.Cpu:
          {
            hardwareJsonPack.Data.CPU.name = hardware.Name;
            foreach (var sensor in hardware.Sensors)
            {
              if (sensor.SensorType == SensorType.Clock)
              {
                if (string.Compare(sensor.Name, "Bus Speed", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.CPU.freq.bus = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                hardwareJsonPack.Data.CPU.freq.current.Add(FloatConvert(sensor.Value ?? 0, 2));
              }

              if (sensor.SensorType == SensorType.Temperature)
              {
                if (sensor.Name.Contains("Distance"))
                  continue;

                if (string.Compare(sensor.Name, "Core Max", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.CPU.temp.max = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "Core Average", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.CPU.temp.average = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "CPU Package", StringComparison.Ordinal) == 0)
                {
                  //
                  continue;
                }

                hardwareJsonPack.Data.CPU.temp.current.Add(FloatConvert(sensor.Value ?? 0, 2));
              }

              if (sensor.SensorType == SensorType.Load)
              {
                if (string.Compare(sensor.Name, "CPU Total", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.CPU.load.total = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                hardwareJsonPack.Data.CPU.load.current.Add(FloatConvert(sensor.Value ?? 0, 2));
              }

              if (sensor.SensorType == SensorType.Power)
              {
                if (string.Compare(sensor.Name, "CPU Package", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.CPU.power.current = FloatConvert(sensor.Value ?? 0, 2);
                  hardwareJsonPack.Data.CPU.power.max = FloatConvert(sensor.Max ?? 0, 2);
                  continue;
                }
              }
            }

            hardwareJsonPack.Data.CPU.coreCount = hardwareJsonPack.Data.CPU.freq.current.Count;
            break;
          }
          case HardwareType.Motherboard:
            break;
          case HardwareType.SuperIO:
            break;
          case HardwareType.Memory:
          {
            foreach (var sensor in hardware.Sensors)
            {
              if (sensor.SensorType == SensorType.Load)
              {
                hardwareJsonPack.Data.Memory.load.current = FloatConvert(sensor.Value ?? 0, 2);
                hardwareJsonPack.Data.Memory.load.max = FloatConvert(sensor.Max ?? 0, 2);
                continue;
              }

              if (sensor.SensorType == SensorType.Data)
              {
                Console.WriteLine(sensor.Name + " " + sensor.Value);

                if (string.Compare(sensor.Name, "Memory Used", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.Memory.data.used = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "Memory Available", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.Memory.data.free = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }
              }
            }

            hardwareJsonPack.Data.Memory.data.total =
              hardwareJsonPack.Data.Memory.data.free +
              hardwareJsonPack.Data.Memory.data.used;
            hardwareJsonPack.Data.Memory.data.total =
              FloatConvert(hardwareJsonPack.Data.Memory.data.total, 2);
            break;
          }
          case HardwareType.GpuNvidia:
          case HardwareType.GpuAmd:
          {
            hardwareJsonPack.Data.GPU.name = hardware.Name;
            foreach (var sensor in hardware.Sensors)
            {
              if (sensor.SensorType == SensorType.Clock)
              {
                if (string.Compare(sensor.Name, "GPU Core", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.GPU.freq.core.current = FloatConvert(sensor.Value ?? 0, 2);
                  hardwareJsonPack.Data.GPU.freq.core.max = FloatConvert(sensor.Max ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "GPU Memory", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.GPU.freq.mem.current = FloatConvert(sensor.Value ?? 0, 2);
                  hardwareJsonPack.Data.GPU.freq.mem.max = FloatConvert(sensor.Max ?? 0, 2);
                  continue;
                }
              }

              if (sensor.SensorType == SensorType.Temperature)
              {
                hardwareJsonPack.Data.GPU.temp.current = FloatConvert(sensor.Value ?? 0, 2);
                hardwareJsonPack.Data.GPU.temp.max = FloatConvert(sensor.Max ?? 0, 2);
                continue;
              }

              if (sensor.SensorType == SensorType.Load)
              {
                if (string.Compare(sensor.Name, "GPU Core", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.GPU.load.coreCurrent = FloatConvert(sensor.Value ?? 0, 2);
                  hardwareJsonPack.Data.GPU.load.coreMax = FloatConvert(sensor.Max ?? 0, 2);
                  continue;
                }
              }

              if (sensor.SensorType == SensorType.Power)
              {
                hardwareJsonPack.Data.GPU.power.current = FloatConvert(sensor.Value ?? 0, 2);
                hardwareJsonPack.Data.GPU.power.max = FloatConvert(sensor.Max ?? 0, 2);
                continue;
              }

              if (sensor.SensorType == SensorType.SmallData)
              {
                if (string.Compare(sensor.Name, "GPU Memory Free", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.GPU.mem.free = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "GPU Memory Used", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.GPU.mem.used = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "GPU Memory Total", StringComparison.Ordinal) == 0)
                {
                  hardwareJsonPack.Data.GPU.mem.total = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }
              }
            }

            break;
          }
          case HardwareType.Storage:
          {
            var hdData = new HardDiskData
            {
              name = (hardware.Name == "" ? 
                "Nameless Hard Disk #" + hardwareJsonPack.Data.HardDisk.count : hardware.Name)
            };

            foreach (var sensor in hardware.Sensors)
            {
              if (sensor.SensorType == SensorType.Throughput)
              {
                if (string.Compare(sensor.Name, "Read Rate", StringComparison.Ordinal) == 0)
                {
                  hdData.throughput.readRate = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }

                if (string.Compare(sensor.Name, "Write Rate", StringComparison.Ordinal) == 0)
                {
                  hdData.throughput.writeRate = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }
              }

              if (sensor.SensorType == SensorType.Temperature)
              {
                hdData.temp.current = FloatConvert(sensor.Value ?? 0, 2);
                hdData.temp.max = FloatConvert(sensor.Max ?? 0, 2);
              }

              if (sensor.SensorType == SensorType.Load)
              {
                if (string.Compare(sensor.Name, "Used Space", StringComparison.Ordinal) == 0)
                {
                  hdData.load.current = FloatConvert(sensor.Value ?? 0, 2);
                  continue;
                }
              }
            }

            hardwareJsonPack.Data.HardDisk.data.Add(hdData);
            hardwareJsonPack.Data.HardDisk.count = hardwareJsonPack.Data.HardDisk.data.Count;
            break;
          }
          case HardwareType.Network:
          {
            var data = new NetworkData();
            var throughput = new Throughput();

            foreach (var sensor in hardware.Sensors)
            {
              if (sensor.SensorType == SensorType.Data)
              {
                if (string.Compare(sensor.Name, "Data Uploaded", StringComparison.Ordinal) == 0)
                {
                  data.uploaded = FloatConvert(sensor.Value ?? 0, 2);
                }

                if (string.Compare(sensor.Name, "Data Downloaded", StringComparison.Ordinal) == 0)
                {
                  data.downloaded = FloatConvert(sensor.Value ?? 0, 2);
                }
              }

              if (sensor.SensorType == SensorType.Throughput)
              {
                if (string.Compare(sensor.Name, "Upload Speed", StringComparison.Ordinal) == 0)
                {
                  throughput.upload = FloatConvert(sensor.Value ?? 0, 2);
                }

                if (string.Compare(sensor.Name, "Download Speed", StringComparison.Ordinal) == 0)
                {
                  throughput.download = FloatConvert(sensor.Value ?? 0, 2);
                }
              }
            }

            if (string.Compare(hardware.Name, "WLAN", StringComparison.Ordinal) == 0)
            {
              hardwareJsonPack.Data.Network.wlan.data = data;
              hardwareJsonPack.Data.Network.wlan.throughput = throughput;
              continue;
            }

            if (string.Compare(hardware.Name, "以太网", StringComparison.Ordinal) == 0)
            {
              hardwareJsonPack.Data.Network.ethernet.data = data;
              hardwareJsonPack.Data.Network.ethernet.throughput = throughput;
              continue;
            }

            break;
          }
          case HardwareType.Cooler:
            break;
          case HardwareType.EmbeddedController:
            break;
          case HardwareType.Psu:
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      hardwareStr = JsonConvert.SerializeObject(hardwareJsonPack);
    }
  }
}
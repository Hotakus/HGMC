using System.Collections.Generic;

namespace HGMC.Source.HardwareMonitor
{
  public class CPUFreq
  {
    public float bus { get; set; }
    public List<float> current { get; set; }

    public CPUFreq()
    {
      current = new List<float>();
    }
  }


  public class CPUTemp
  {
    public List<float> current { get; set; }
    public float max { get; set; }
    public float average { get; set; }

    public CPUTemp()
    {
      current = new List<float>();
    }
  }

  public class CPULoad
  {
    public float total { get; set; }
    public List<float> current { get; set; }

    public CPULoad()
    {
      current = new List<float>();
    }
  }

  public class CPUPower
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class CPU
  {
    public string name { get; set; }
    public int coreCount { get; set; }
    public CPUFreq freq { get; set; }
    public CPUTemp temp { get; set; }
    public CPULoad load { get; set; }
    public CPUPower power { get; set; }

    public CPU()
    {
      freq = new CPUFreq();
      temp = new CPUTemp();
      load = new CPULoad();
      power = new CPUPower();
    }
  }

  public class GPUTemp
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class Core
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class Mem
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class GPUFreq
  {
    public Core core { get; set; }
    public Mem mem { get; set; }

    public GPUFreq()
    {
      core = new Core();
      mem = new Mem();
    }
  }

  public class GPULoad
  {
    public float coreCurrent { get; set; }
    public float coreMax { get; set; }
  }

  public class GPUMem
  {
    public float free { get; set; }
    public float used { get; set; }
    public float total { get; set; }
  }

  public class GPUPower
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class GPU
  {
    public string name { get; set; }
    public GPUTemp temp { get; set; }
    public GPUFreq freq { get; set; }
    public GPULoad load { get; set; }
    public GPUMem mem { get; set; }
    public GPUPower power { get; set; }

    public GPU()
    {
      temp = new GPUTemp();
      freq = new GPUFreq();
      load = new GPULoad();
      mem = new GPUMem();
      power = new GPUPower();
    }
  }

  public class MemoryData
  {
    public float free { get; set; }
    public float used { get; set; }
    public float total { get; set; }
  }

  public class MemoryLoad
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class Memory
  {
    public MemoryData data { get; set; }
    public MemoryLoad load { get; set; }

    public Memory()
    {
      data = new MemoryData();
      load = new MemoryLoad();
    }
  }

  public class HardDiskTemp
  {
    public float current { get; set; }
    public float max { get; set; }
  }

  public class HardDiskLoad
  {
    public float current { get; set; }
  }

  public class HardDiskThroughput
  {
    public float readRate { get; set; }
    public float writeRate { get; set; }
  }

  public class HardDiskData
  {
    public string name { get; set; }
    public HardDiskTemp temp { get; set; }
    public HardDiskLoad load { get; set; }
    public HardDiskThroughput throughput { get; set; }

    public HardDiskData()
    {
      temp = new HardDiskTemp();
      load = new HardDiskLoad();
      throughput = new HardDiskThroughput();
    }
  }

  public class HardDisk
  {
    public int count { get; set; }
    public List<HardDiskData> data { get; set; }

    public HardDisk()
    {
      data = new List<HardDiskData>();
    }
  }

  public class NetworkData
  {
    public float uploaded { get; set; }
    public float downloaded { get; set; }
  }

  public class Throughput
  {
    public float upload { get; set; }
    public float download { get; set; }
  }

  public class Wlan
  {
    public NetworkData data { get; set; }
    public Throughput throughput { get; set; }

    public Wlan()
    {
      data = new NetworkData();
      throughput = new Throughput();
    }
  }

  public class Ethernet
  {
    public NetworkData data { get; set; }
    public Throughput throughput { get; set; }

    public Ethernet()
    {
      data = new NetworkData();
      throughput = new Throughput();
    }
  }

  public class Network
  {
    public Wlan wlan { get; set; }
    public Ethernet ethernet { get; set; }

    public Network()
    {
      wlan = new Wlan();
      ethernet = new Ethernet();
    }
  }

  public class Dat
  {
    public CPU CPU { get; set; }
    public GPU GPU { get; set; }
    public Memory Memory { get; set; }
    public HardDisk HardDisk { get; set; }
    public Network Network { get; set; }

    public Dat()
    {
      CPU = new CPU();
      GPU = new GPU();
      Memory = new Memory();
      HardDisk = new HardDisk();
      Network = new Network();
    }
  }

  public class HardwareJsonPack
  {
    public string Header { get; set; }
    public string DataType { get; set; }
    public Dat Data { get; set; }

    public HardwareJsonPack() => Data = new Dat();
  }
}
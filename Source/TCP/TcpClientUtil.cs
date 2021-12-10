using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace HGMC.Source.TCP
{
  public class TcpClientUtil
  {
    private string _serverIp = "";
    private int _port = -0;

    private string _readContent = "";
    private readonly Mutex _rcMutex;

    private TcpClient _tcpClient;
    private BinaryReader _binaryReader;
    private BinaryWriter _binaryWriter;
    private bool _released = true;

    private readonly List<string> _msgQueue = new List<string>();

    public TcpClientUtil()
    {
      _rcMutex = new Mutex(true);
    }

    ~TcpClientUtil()
    {
      if (!_released)
        _tcpClient.Close();
      _rcMutex.Close();
    }

    public void SetServerParams(string serverIp, int port = 20)
    {
      _serverIp = serverIp;
      _port = port;
    }

    public bool Connected() => _tcpClient.Connected;
    public bool Available() => _tcpClient.Available > 0;
    public int MsgCount() => _msgQueue.Count;

    public bool Connect()
    {
      try
      {
        if (_serverIp == null || _port < 0)
        {
          Console.WriteLine("ip and port is null.");
          return false;
        }

        _tcpClient = new TcpClient(_serverIp, _port);

        Thread.Sleep(100);

        if (Connected())
        {
          _binaryReader = new BinaryReader(_tcpClient.GetStream());
          _binaryWriter = new BinaryWriter(_tcpClient.GetStream());

          _released = false;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }

      return true;
    }

    public bool DisConnect()
    {
      _msgQueue.Clear();

      try
      {
        _tcpClient.Close();

        if (!Connected())
        {
          Console.WriteLine("TCP Client closed.");
          _released = true;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }

      return true;
    }

    public bool SendMsg(string msg)
    {
      if (this.Connected() == false)
        return false;

      try
      {
        _binaryWriter?.Write(msg.ToCharArray());
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }

      return true;
    }

    public void ReceiveMsgThread()
    {
      Console.WriteLine("ReceiveMsgThread");
      while (true)
      {
        if (this.Connected() == false)
        {
          Thread.Sleep(10);
          continue;
        }

        ReceiveMsg();
        if (MsgCount() != 0)
        {
          Console.WriteLine(GetMsg());
        }

        Thread.Sleep(10);
      }
    }

    public void ReceiveMsg()
    {
      if (!Connected() || !Available())
        return;

      _readContent = "";
      while (Available())
        _readContent += _binaryReader.ReadChar();
      _msgQueue.Add(_readContent);
    }

    public string GetMsg()
    {
      var tmp = "null";
      if (_msgQueue.Count > 0)
      {
        tmp = _msgQueue[0];
        _msgQueue.RemoveAt(0);
      }

      return tmp;
    }
  }
}
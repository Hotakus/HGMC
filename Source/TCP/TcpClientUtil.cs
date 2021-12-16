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
    private bool _connected = false;

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

    public bool Connected() => _connected;
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

        Thread.Sleep(1000);

        if (_tcpClient.Connected)
        {
          _binaryReader = new BinaryReader(_tcpClient.GetStream());
          _binaryWriter = new BinaryWriter(_tcpClient.GetStream());

          _released = false;
          _connected = true;
          return true;
        }
        return false;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public bool DisConnect()
    {
      _msgQueue.Clear();

      try
      {
        _tcpClient.Close();

        if (!_tcpClient.Connected)
        {
          Console.WriteLine("TCP Client closed.");
          _released = true;
          _connected = false;
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
    
    public bool SendMsg(byte[] msg, int index, int count)
    {
      if (this.Connected() == false)
        return false;

      try
      {
        _binaryWriter?.Write(msg, index, count);
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
        // if (MsgCount() != 0)
        // {
        //   Console.WriteLine(GetMsg());
        // }

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
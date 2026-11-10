using DirectShowLib.BDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace testCam.Comm;

public sealed class TcpServerComm : IDisposable
{

    private const char STX = '\x02';
    private const char ETX = '\x03';

    private TcpListener? _listener;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private Thread? _acceptThread;
    private volatile bool _running;
    private volatile bool _clientAlive;
    private readonly object _sendLock = new object();

    public event EventHandler<string>? DataReceived;
    public event EventHandler<string>? LogOccurred;
    public event EventHandler? ClientConnected;
    public event EventHandler? ClientDisconnected;

    public bool IsRunning => _running;
    public bool IsClientConnected => _clientAlive;


    public bool Start(string host, int port)
    {
        if (_running) return false;
        
        try
        {
            IPAddress ip = host == "any" ? IPAddress.Any : IPAddress.Parse(host);
            _listener = new TcpListener(ip, port);
            _listener.Start();
            _running = true;


            _acceptThread = new Thread(AcceptLoop) { IsBackground = true , Name = "vision-Accept"};
            _acceptThread.Start();
            
            return true;
        }
        catch (Exception ex)
        {
            _running = false;
            _listener = null;
            LogOccurred?.Invoke(this, $"Error starting server: {ex.Message}");
            return false;
        }
    }


    private void AcceptLoop()
    {
        TcpListener listener = _listener!;
        while (_running)
        {
            TcpClient incoming;
            try { incoming = listener.AcceptTcpClient(); }
            catch (Exception) { break; }

            if (_clientAlive)
            {
                try { incoming.Close(); } catch { }
                continue;
            }

            NetworkStream stream = incoming.GetStream();
            _client = incoming;
            _stream = stream;
            _clientAlive = true;

            ClientConnected?.Invoke(this, EventArgs.Empty);

            Thread rx = new Thread(() => ReceiveLoop(incoming, stream)) { IsBackground = true, Name = "Vision-RX" };
            rx.Start();
        }
    }

    public void ReceiveLoop(TcpClient client, NetworkStream stream)
    {
        byte[] buffer = new byte[4096];
        string rx = "";
        try
        {
            while (_running)
            {
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0) break;
                rx += Encoding.ASCII.GetString(buffer, 0, read);

                while (rx.Contains((char)0x03))
                {
                    int stx = rx.IndexOf(STX);
                    if (stx < 0) { rx = ""; break; };
                    int etx = rx.IndexOf(ETX, stx + 1);
                    if (etx < 0) { rx = rx.Substring(stx); break; };

                    if (stx >= 0 && stx < etx)
                    {
                        string msg = rx.Substring(stx + 1, etx - stx - 1);
                        rx = rx.Substring(etx + 1);
                        DataReceived?.Invoke(this, msg);
                    } else
                    {
                        rx = "";
                        break;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            if (_running && _clientAlive) Log($"수신 오류 {ex.Message}");
        }
        finally
        {
            try { stream.Close(); } catch { }
            try { client.Close(); } catch { }
            _stream = null;
            _client = null;
            _clientAlive = false;
            Log("클라이언트 접속 종료");
            ClientDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool Send(string payload)
    {

        NetworkStream? stream = _stream;
        if (stream == null || !_clientAlive) return false;

        byte[] body = Encoding.ASCII.GetBytes(payload);
        byte[] framed = new byte[body.Length + 2];
        framed[0] = 0x02;
        Array.Copy(body, 0, framed, 1, body.Length);
        framed[framed.Length - 1] = 0x03;

        try {
            lock (_sendLock) { stream.Write(framed, 0, framed.Length); };
                                                                      
            return true;
        } catch (Exception e) { 
        
            Log($"SEND FAILED {e.Message}");
            return false;
        }
    

    
    }



    public void Stop()
    {
        _running = false;
        try { _listener?.Stop(); } catch { }
        _listener = null;

        try { _stream?.Close(); } catch { }
        try { _client?.Close(); } catch { }
        _acceptThread?.Join(500);

    }




    private void Log(string message) => LogOccurred?.Invoke(this, message);



    public void Dispose() => Stop();
}

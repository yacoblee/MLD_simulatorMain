using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace copyApp.Comm
{
    public sealed class TcpComm : IDisposable
    {
        public static TcpComm Instance { get; } = new TcpComm();

        private TcpClient _client;
        private TcpListener _listener;


        private NetworkStream _stream;
        private Thread _rxThread;
        private Thread _serverThread; // 서버로 접속하면 
        
        private volatile bool _running;
        private volatile bool _serverRunning; //서버 동작시


        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private string _rxBuffer = "";

        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;
        public event EventHandler DisconnectedEvent;

        public bool IsConnected => _client != null && _client.Connected;
        public bool IsServerRunning => _serverRunning;

        private TcpComm() { }

        public bool ServerConnect(string host, int port)
        {
            try
            {
                Disconnect();
                IPAddress ipAddress = host == "any"? IPAddress.Any : IPAddress.Parse(host);

                _listener = new TcpListener(ipAddress, port);
                _listener.Start();

                _serverRunning = true;
                _serverThread = new Thread(ServerAccept) { IsBackground = true , Name = "Tcp-ServerAccept"};

                _serverThread.Start();
                OnLogOccurred("TCP 서버 동작중");


                return true;

            }
            catch (Exception ex)
            {
                _listener = null;
                _serverRunning = false;

                OnLogOccurred($"서버 실행 중 오류 발생 : {ex}");
                return false;
            }
        }

        private void ServerAccept()
        {
            try
            {
                while (_serverRunning)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    if (IsConnected)
                    {
                        Disconnect();
                    }

                    _client = client;
                    _stream = _client.GetStream();

                    _running = true;
                    _rxThread = new Thread(ReceiveLoop) { IsBackground = true, Name = "TCP-Client-RX" };
                    _rxThread.Start();

                    Connected?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                if (_serverRunning) OnLogOccurred($"서버 연결 대기중 오류 발생 {ex}");
            }
        }

        public async Task<bool> Connect(string ip, int port)
        {
            try
            {
                Disconnect();
                TcpClient localClient = new TcpClient();

                await localClient.ConnectAsync(ip, port);

                _stream = localClient.GetStream();
                _client = localClient;

                _running = true;
                _rxThread = new Thread(ReceiveLoop) { IsBackground = true, Name = "TCP-RX" };
                _rxThread.Start();

                Connected?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                OnLogOccurred($"TCP 연결 오류: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            _running = false;
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _stream = null;
            _client = null;
        }

        public void AllDisconnect()
        {
            _serverRunning = false;
            try { _listener?.Stop(); } catch { }
            _listener = null;

            Disconnect();

            if (_serverThread  != null && _serverThread.IsAlive)
            {
                _serverThread.Join(500);
            }
            _serverThread  = null;
        }

        public void SendFramed(string payload)
        {
            if (!IsConnected) return;
            byte[] body = Encoding.ASCII.GetBytes(payload);
            byte[] framed = new byte[body.Length + 2];
            framed[0] = STX;
            Array.Copy(body, 0, framed, 1, body.Length);

            framed[framed.Length - 1] = ETX;
            _stream.Write(framed, 0, framed.Length);
        }

        private void ReceiveLoop()
        {
            byte[] buffer = new byte[4096];
            try
            {
                while (_running)
                {
                    int read = _stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0) break;
                    string text = Encoding.ASCII.GetString(buffer, 0, read);
                    _rxBuffer += text;

                    while (_rxBuffer.Contains((char)0x03))
                    {
                        int stxIdx = _rxBuffer.IndexOf((char)0x02);
                        int etxIdx = _rxBuffer.IndexOf((char)0x03);

                        if (stxIdx >= 0 && etxIdx > stxIdx)
                        {
                            string msg = _rxBuffer.Substring(stxIdx + 1, etxIdx - stxIdx - 1);
                            _rxBuffer = _rxBuffer.Substring(etxIdx + 1);
                            if (!string.IsNullOrWhiteSpace(msg))
                            {
                                DataReceived?.Invoke(this, msg);
                            }
                        }
                        else
                        {
                            _rxBuffer = "";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_running) OnLogOccurred($"TCP 수신 오류: {ex.Message}");
            }
            finally
            {
                DisconnectedEvent?.Invoke(this, EventArgs.Empty);   // 끊김 알림
            }
        }

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);

        public void Dispose()
        {
            AllDisconnect();
            _rxThread?.Join(1000);
        }
    }
}

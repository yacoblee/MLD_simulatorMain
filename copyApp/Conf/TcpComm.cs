using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace copyApp.Comm
{
    /// <summary>
    /// TCP 통신 전담 싱글톤 모듈. 연결/송수신을 담당하고 수신 데이터를
    /// 이벤트로 broadcast 한다. (UI/Model을 모름) — SerialComm과 동일 구조.
    /// </summary>
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

        public bool Connect(string ip, int port)
        {
            try
            {
                Disconnect();
                _client = new TcpClient();
                _client.Connect(ip, port);
                _stream = _client.GetStream();

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
            // 1. 서버 리스너 정지
            _serverRunning = false;
            try { _listener?.Stop(); } catch { }
            _listener = null;

            // 2. 클라이언트 소켓 및 스트림 정지
            Disconnect();

            // 3. 서버 대기 스레드 종료 확인
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

            framed[framed.Length - 1] = ETX; // ETX
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
                    if (read <= 0) break;                       // 상대 종료
                    string text = Encoding.ASCII.GetString(buffer, 0, read);
                    DataReceived?.Invoke(this, text);
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

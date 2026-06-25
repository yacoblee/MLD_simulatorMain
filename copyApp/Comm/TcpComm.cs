using System;
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
        private NetworkStream _stream;
        private Thread _rxThread;
        private volatile bool _running;

        // 프레이밍: STX + payload + CR LF (sendProject 규약과 동일)
        private const byte STX = 0x02;

        public event EventHandler<string> DataReceived;   // 수신 raw 데이터 broadcast
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;

        public bool IsConnected => _client != null && _client.Connected;

        private TcpComm() { }

        public bool Connect(string ip, int port)
        {
            try
            {
                Disconnect();                       // 기존 연결 정리
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

        public void SendFramed(string payload)
        {
            if (!IsConnected) return;
            byte[] body = Encoding.ASCII.GetBytes(payload);
            byte[] framed = new byte[body.Length + 3];
            framed[0] = STX;
            Array.Copy(body, 0, framed, 1, body.Length);
            framed[framed.Length - 2] = 0x0D; // CR
            framed[framed.Length - 1] = 0x0A; // LF
            _stream.Write(framed, 0, framed.Length);
        }

        // 백그라운드 수신 루프 (SerialPort.DataReceived 역할)
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
                    DataReceived?.Invoke(this, text);           // 모든 구독자에게 broadcast
                }
            }
            catch (Exception ex)
            {
                if (_running) OnLogOccurred($"TCP 수신 오류: {ex.Message}");
            }
        }

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);

        public void Dispose()
        {
            Disconnect();
            _rxThread?.Join(1000);
        }
    }
}

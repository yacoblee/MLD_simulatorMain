using System.Net.Sockets;
using System.Text;

namespace camProject.Comm
{

    // 포팅 시 주의:
    //   - using System.Net.Sockets; (net8 기본 포함, 별도 패키지 불필요)
    //   - ReceiveLoop 안에서 DataReceived?.Invoke(this, msg)를 두 번 호출하지 않도록 주의
    //     (이전 세션에서 발견된 버그 — 같은 프레임이 중복 처리되어 상태머신이 꼬이는 원인이었음)
    public sealed class TcpComm : IDisposable
    {

        public static TcpComm Instance { get; } = new TcpComm();

        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _rxThread;

        private volatile bool _connected;
        public bool IsConnected => _connected;
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private string _rxBuffer = "";

        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;
        public event EventHandler DisconnectedEvent;

        private TcpComm() { }

        public async Task<bool> Connect(string ip, int port, int timeOutCnt, int retryCnt)
        {
            try
            {
                Disconnect();
                var client = new TcpClient();
                await client.ConnectAsync(ip, port);

                _stream = client.GetStream();
                _client = client;
                _connected = true;

                _rxThread = new Thread(ReceiveLoop) { IsBackground = true, Name = "TCP-Client-RX" };


                _rxThread.Start();
                Connected?.Invoke(this, EventArgs.Empty);
                OnLogOccurred($"TCP 연결 성공 IP : {ip}");
                
                return true;
            }
            catch (Exception e)
            {
                OnLogOccurred($"TCP 연결 오류: {e.Message}");
                _connected = false;
                return false;
            }
        }



        private void ReceiveLoop()
        {
            byte[] buffer = new byte[4096];
            try
            {
                while (_connected)
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
                if (_connected) OnLogOccurred($"TCP 수신 오류: {ex.Message}");
            }
            finally
            {
                DisconnectedEvent?.Invoke(this, EventArgs.Empty);
            }
        }


        public void SendFramed(string payload)
        {
            if (!_connected) return;
            byte[] body = Encoding.ASCII.GetBytes(payload);
            byte[] framed = new byte[body.Length + 2];

            framed[0] = STX;
            Array.Copy(body, 0 , framed , 1 , body.Length);
            framed[framed.Length - 1] = ETX;
            _stream.Write(framed, 0, framed.Length);
            
        }
        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);

        public void Disconnect() {
            OnLogOccurred($"TCP 연결 종료");
            _connected = false;
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
 
        }

        public void Dispose() {
            Disconnect();
            _client?.Dispose();
            _rxThread?.Join(1000);
        }   

    }
}

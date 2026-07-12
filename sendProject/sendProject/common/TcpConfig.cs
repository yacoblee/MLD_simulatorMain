using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace sendProject.common
{
    public class TcpConfig : IDisposable
    {

        public static TcpConfig Instance { get; } = new TcpConfig();
        private TcpClient _client;
        private TcpListener _listener;

        private volatile bool _serverRunning;
        public event EventHandler Connected;
        public event EventHandler DisconnectedEvent;
        public event EventHandler<string> OnLogPrint;
        public event EventHandler<string> DataReceived;
        private NetworkStream _stream;

        private Thread _rxThread;
        private Thread _serverThread;

        public bool IsServerRunning => _serverRunning;


        public TcpConfig() { }



        public bool ServerConnect (string host, int port)
        {
            try {
                IPAddress ipAddress = host == "any" ? IPAddress.Any : IPAddress.Parse(host);

                _listener = new TcpListener(ipAddress, port);
                _listener.Start();

                _serverRunning = true;


                _serverThread = new Thread(ServerAccept) { IsBackground = true, Name ="Server Tx-Rx" };

                _serverThread.Start();
                LogPrint($"server start {ipAddress} : {port} \r time: {DateTime.Now: yyyy-mm-dd:HH:m:s}");

                return true;
            } catch (Exception ex){
 
                LogPrint($"서버 시작 실패: {ex.Message.ToString()}");
                return false;
            }
        }


        public void TcpDisconnect()
        {
            if (_listener != null)
            {
                _serverRunning = false;
                _listener.Stop();
                _serverThread?.Join();
                _listener = null;
            }
        }

        private void ServerAccept()
        {
            try
            {
                while (_serverRunning)
                {
                    TcpClient tcpClient = _listener.AcceptTcpClient();
                    _client = tcpClient;
                    _stream = tcpClient.GetStream();

                    _serverRunning = true;


                    Connected?.Invoke(this, EventArgs.Empty);
                    LogPrint($"client connected: {tcpClient.Client.RemoteEndPoint}");
                    _rxThread = new Thread(ReceiveLoop) { IsBackground = true, Name = "Server-RX" };
                    _rxThread.Start();

                }
            }
            catch { 
            
            
            }
        }

        

        private void ReceiveLoop()
        {
            byte[] buffer = new byte[2048];
            var sb = new StringBuilder();          // 프레임 조립용 (STX~ETX 사이 누적)

            try
            {
                while (_serverRunning && _stream != null)
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0) break;      // 상대가 연결 종료

                    for (int i = 0; i < bytesRead; i++)
                    {
                        byte b = buffer[i];

                        if (b == 0x02)              // STX → 새 프레임 시작
                        {
                            sb.Clear();
                        }
                        else if (b == 0x03)         // ETX → 프레임 완성
                        {
                            string payload = sb.ToString();
                            DataReceived?.Invoke(this, payload);   // 완성된 데이터 전달
                            LogPrint($"수신: {payload}");
                            sb.Clear();
                        }
                        else
                        {
                            sb.Append((char)b);     // 본문 바이트 누적 (ASCII)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_serverRunning) LogPrint($"수신 오류: {ex.Message}");
            }
            finally
            {
                DisconnectedEvent?.Invoke(this, EventArgs.Empty);  // 끊김 알림
            }
        }



        public void SendData(string data, bool valid)
        {
            if (_stream != null && _stream.CanWrite)
            {
                byte[] payload = Encoding.ASCII.GetBytes(data);
                byte[] framedData = null;
                if(valid)
                    framedData = new byte[payload.Length + 2];
                else 
                     framedData = new byte[payload.Length + 1];

                framedData[0] = 0x02; // STX
                Array.Copy(payload, 0, framedData, 1, payload.Length);
                
                if(valid)
                    framedData[framedData.Length - 1] = 0x03; // ETX
                try
                {
                    _stream.Write(framedData, 0, framedData.Length);
                    LogPrint($"송신: {data}");
                }
                catch (Exception ex)
                {
                    LogPrint($"송신 오류: {ex.Message}");
                }
            }
            else
            {
                LogPrint("송신 실패: 스트림이 유효하지 않음");
            }
        }

        private void LogPrint(string msg) => OnLogPrint?.Invoke(this, msg);

        public void Dispose()
        {
            _serverRunning = false;
            try { _listener?.Stop(); } catch { }
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _serverThread?.Join(300);       
            _listener = null;
            _serverThread = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace test
{
    public class TcpConnect
    {
        public TcpListener ServerListener { get; private set; }
        public TcpClient Client { get; private set; }
        public TcpClient ServerClient { get; private set; }
        public bool IsServerRunning { get; private set; } = false;
        public bool IsClientRunning { get; private set; } = false;


        public event Action<string> OnLogMessage; // 실시간 전달 용도

        public async Task StartServerAsync(string ip, int port)
        {
            try
            {
                System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse(ip);
                ServerListener = new TcpListener(ipAddress, port);
                ServerListener.Start();
                IsServerRunning = true;

                OnLogMessage?.Invoke($"서버가 [{ip}:{port}] 에서 대기 중입니다...\r\n");

                while (IsServerRunning)
                {
                    ServerClient = await ServerListener.AcceptTcpClientAsync();
                    OnLogMessage?.Invoke($"클라이언트({ServerClient.Client.RemoteEndPoint})가 접속했습니다!\r\n");

                }
            }
            catch (Exception ex)
            {
                if (IsServerRunning) OnLogMessage?.Invoke($"서버 에러: {ex.Message}\r\n");
            }
        }

        public async Task ConnectAsClientAsync(string ip, int port, int timeoutMs, int retrySec)
        {
            IsClientRunning = true;

            while (IsClientRunning)
            {
                try
                {
                    OnLogMessage?.Invoke($"서버({ip}:{port}) 접속 시도 중...\r\n");
                    Client = new TcpClient();

                    var connectTask = Client.ConnectAsync(ip, port);

                    if (await Task.WhenAny(connectTask, Task.Delay(timeoutMs)) == connectTask)
                    {
                        OnLogMessage?.Invoke("서버와 연결되었습니다!\r\n");

                        break;
                    }
                    else
                    {
                        throw new TimeoutException("연결 시간 초과!");
                    }
                }
                catch (Exception ex)
                {
                    OnLogMessage?.Invoke($"연결 실패 ({ex.Message}). {retrySec}초 후 재접속합니다...\r\n");
                    Client?.Close();
                    await Task.Delay(retrySec * 1000);
                }
            }
        }

        public void DisconnectAll()
        {
            try
            {
                if (IsServerRunning)
                {
                    IsServerRunning = false;
                    ServerListener?.Stop();
                    OnLogMessage?.Invoke("서버가 안전하게 종료되었습니다.\r\n");
                }

                if (IsClientRunning)
                {
                    IsClientRunning = false;
                    Client?.Close();
                    OnLogMessage?.Invoke("클라이언트 연결이 종료되었습니다.\r\n");
                }
            }
            catch (Exception ex)
            {
                OnLogMessage?.Invoke($"종료 중 에러 발생: {ex.Message}\r\n");
            }
        }

        public async Task SendDataAsync(string message, Encoding encoding)
        {
            try
            {
                byte[] data = encoding.GetBytes(message);
                NetworkStream stream = null;

                if (IsClientRunning)
                {
                    stream = Client.GetStream();

                }
                else if(IsServerRunning)
                {
                    stream = ServerClient.GetStream();
                } else 
                {
                    OnLogMessage?.Invoke("데이터 전송 실패: 클라이언트가 연결되어 있지 않습니다.\r\n");
                }

                if (stream != null)
                {
                    await stream.WriteAsync(data, 0, data.Length);
                    OnLogMessage?.Invoke($" 전송: {message}\r\n");
                } else {
                    OnLogMessage?.Invoke("연결된 대상이 없어 데이터를 보낼 수 없습니다.\r\n");
                }

            }
            catch (Exception ex)
            {
                OnLogMessage?.Invoke($"데이터 전송 중 에러 발생: {ex.Message}\r\n");
            }
        }

    }
}
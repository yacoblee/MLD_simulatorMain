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
        // 통신 알맹이들을 외부에서 읽을 수만 있게 속성(Property)으로 선언
        public TcpListener ServerListener { get; private set; }
        public TcpClient Client { get; private set; }
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
                    TcpClient connectedClient = await ServerListener.AcceptTcpClientAsync();
                    OnLogMessage?.Invoke($"클라이언트({connectedClient.Client.RemoteEndPoint})가 접속했습니다!\r\n");

                    // TODO: 접속된 클라이언트와 통신하는 수신 루프 연결
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

                        
                        // await ReceiveDataLoopAsync(); 
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
    }
}
using copyApp.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace copyApp.Model
{
    public class TcpModel
    {
        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;

        private System.Timers.Timer _retryTimer;    // 주기적인 재연결을 시도할 백그라운드 타이머
        private int _maxRetryCount = 0;             // 최대 허용 재시도 횟수
        private int _currentRetryCount = 0;          // 현재 재시도 누적 횟수

        // 연결에 필요한 정보를 타이머 핸들러가 알 수 있도록 임시 보관할 변수
        private string _targetIp;
        private int _targetPort;


        private string _serverIp;
        private int _serverPort;

        public bool IsConnected { get; private set; }
        public bool ServerConnected { get; private set; }

        public TcpModel()
        {
            TcpComm.Instance.DataReceived += OnData;
        }
        

        private void OnData(object sender, string text)
        {
            DataReceived?.Invoke(this, text);
            LogOccurred?.Invoke(this, $"[수신 데이터] {text}");

        } 

        public void ServerConn(string ipAddress, int port)
        {
            _serverIp = ipAddress;
            _serverPort = port;

            ServerConnected = TcpComm.Instance.ServerConnect(ipAddress, port);

            if (ServerConnected)
            {
                OnLogOccurred($"Connected to {ipAddress}:{port} Success ");
                StopRetryTimer(); // 연결 성공 시 타이머 정리
            }
            OnLogOccurred($"Connected to {ipAddress}:{port}");


        }


        public async Task Connect(string ipAddress, int port, int timeOutCnt, int retryCnt)
        {
            _targetIp = ipAddress;
            _targetPort = port;
            _maxRetryCount = retryCnt;
            _currentRetryCount = 0;


            try
            {
                IsConnected = await TcpComm.Instance.Connect(ipAddress, port);

                if (IsConnected)
                {
                    OnLogOccurred($"Connected to {ipAddress}:{port}");
                    StopRetryTimer(); // 연결 성공 시 타이머 정리
                }
                else
                {
                    StartRetryTimer(timeOutCnt);
                }

                OnLogOccurred($"Connected to {ipAddress}:{port}");
         
            }
            catch (Exception ex)
            {
                IsConnected = false;
                OnLogOccurred($"Connection failed: {ex.Message}");
 
            }
        }

        public bool Disconnect()
        {
            try
            {
                if(ServerConnected)
                {
                    TcpComm.Instance.AllDisconnect();
                    ServerConnected = false;

                    OnLogOccurred("Disconnected from TCP server.");
                    return ServerConnected;
                }
                else
                {
                    TcpComm.Instance.Disconnect();
                    IsConnected = false;
                    OnLogOccurred("Disconnected from TCP client.");
                    return IsConnected;

                }
            
            }
            catch (Exception ex)
            {
                OnLogOccurred($"Disconnection failed: {ex.Message}");
                return false;
            }
        }

        public void Detach()
        {
            IsConnected = false;
            ServerConnected = false;
            TcpComm.Instance.DataReceived -= OnData;
        }

        private void OnLogOccurred (string message) => LogOccurred?.Invoke(this, message);

        private void StartRetryTimer(int intervalMs)
        {

            if (_currentRetryCount >= _maxRetryCount)
            {
                OnLogOccurred($"[Retry] 최대 재시도 횟수({_maxRetryCount}회)를 초과하여 연결을 포기합니다.");
                return;
            }

            if (_retryTimer == null)
            {
                _retryTimer = new System.Timers.Timer();
                _retryTimer.Elapsed +=  OnRetryTimerElapsed; // 타이머 이벤트 연결
            }

            _retryTimer.Interval = intervalMs; // 넘겨받은 주기 설정 (밀리초 단위)
            _retryTimer.AutoReset = false;     // 한 번 켜지고 꺼진 후, 다음 연결 시도 결과에 따라 재점등하기 위함
            _retryTimer.Start();

            _currentRetryCount++;
            OnLogOccurred($"[Retry] 연결 실패. {intervalMs / 1000.0}초 후 재연결을 시도합니다... ({_currentRetryCount}/{_maxRetryCount})");
        }

        private void StopRetryTimer()
        {
            if (_retryTimer != null)
            {
                _retryTimer.Stop();
                _retryTimer.Dispose();
                _retryTimer = null;
            }
            _currentRetryCount = 0;
        }
        private async void OnRetryTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                OnLogOccurred($"[Retry] 재연결 시도 중... ({_targetIp}:{_targetPort})");
                IsConnected = await TcpComm.Instance.Connect(_targetIp, _targetPort);

                if (IsConnected)
                {
                    OnLogOccurred("[Retry] 재연결 성공!");
                    StopRetryTimer();
                }
                else
                {
                    // 또 실패하면 카운트 체크 후 다시 타이머 가동
                    StartRetryTimer((int)_retryTimer.Interval);
                }
            }
            catch (Exception ex)
            {
                OnLogOccurred($"[Retry] 재연결 실패: {ex.Message}");
                StartRetryTimer((int)_retryTimer.Interval);
            }
        }


    }
}

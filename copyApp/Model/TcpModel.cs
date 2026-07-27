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

        private System.Timers.Timer _retryTimer;    
        private int _maxRetryCount = 0;            
        private int _currentRetryCount = 0;      

        private string _targetIp;
        private int _targetPort;


        private string _serverIp;
        private int _serverPort;
        private int[] _sv; 
        private int[] _pv;

        public void UpdatePv(int[] pv) => _pv = pv;
        public void UpdateSv(int[] sv) => _sv = sv;

        public bool IsConnected { get; private set; }
        public bool ServerConnected { get; private set; }

        private enum ProcessState
        {
            Idle,
            WaitingT1Response,
            WaitingT2Response,
            WaitingT3Response 
        }

        private ProcessState _currentState = ProcessState.Idle;


        private string _barcode;
        private string _recipe;
        private string _eqpId;
        private string _side;
        private string _tempSv;
        private string _tempPv;

        private const string DELIM = "/";

        public TcpModel()
        {
            TcpComm.Instance.DataReceived += OnData;
            TcpComm.Instance.LogOccurred += (s, m) => OnLogOccurred(m);
        }


        private void OnData(object sender, string text)
        {
            DataReceived?.Invoke(this, text);
            LogOccurred?.Invoke(this, $"[수신 데이터] {text}");


            string[] tokens = text.Split('/');
            string cmdType = tokens[0];
            string result = tokens[tokens.Length - 1].Trim();

            if (result == "NG")
            {
                OnLogOccurred($"{cmdType} 단계에서 에러가 발생하여 프로세스를 중단합니다.");
                _currentState = ProcessState.Idle;
                return;
            }

   
            switch (_currentState)
            {
                case ProcessState.WaitingT1Response:
                    if (cmdType == "T1" && result == "OK")
                    {
                        _currentState = ProcessState.WaitingT2Response;
                        OnLogOccurred("T1 단계 완료. T2 단계로 진행합니다.");
                        
                        StartT2Process();
                    }
                    break;
                case ProcessState.WaitingT2Response:
                    if (cmdType == "T2")
                    {
                        _currentState = ProcessState.WaitingT3Response;
                        OnLogOccurred("T2 단계 완료. T3 단계로 진행합니다.");
                    }
                    break; 
                case ProcessState.WaitingT3Response:
                    if (cmdType == "T3")
                    {
                        _currentState = ProcessState.Idle;
                        OnLogOccurred("T3 단계 완료. 전체 프로세스가 성공적으로 완료되었습니다.");
                    }
                    break;
            }
            
        }

        public void ServerConn(string ipAddress, int port)
        {
            _serverIp = ipAddress;
            _serverPort = port;

            ServerConnected = TcpComm.Instance.ServerConnect(ipAddress, port);

            if (ServerConnected)
            {
                OnLogOccurred($"Connected to {ipAddress}:{port} Success ");
                StopRetryTimer();
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
                    StopRetryTimer();
                }
                else
                {
                    StartRetryTimer(timeOutCnt);
                }

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
                if (ServerConnected)
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

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);

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
                _retryTimer.Elapsed += OnRetryTimerElapsed; // 타이머 이벤트 연결
            }

            _retryTimer.Interval = intervalMs;
            _retryTimer.AutoReset = false;
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
                    StartRetryTimer((int)_retryTimer.Interval);
                }
            }
            catch (Exception ex)
            {
                OnLogOccurred($"[Retry] 재연결 실패: {ex.Message}");
                StartRetryTimer((int)_retryTimer.Interval);
            }
        }

        public void Send(string message)
        {
            if (IsConnected || ServerConnected)
            {
                TcpComm.Instance.SendFramed(message);
                OnLogOccurred($"송신 데이터 ---->  {message}");
            }
            else
            {
                OnLogOccurred("연결되지 않은 상태에서 데이터를 전송하려고 시도했습니다.");
            }

        }



        public void StartT1Process(string barcode, string recipe, string eqpId, string side)
        {
            _barcode = barcode;
            _recipe = recipe;
            _eqpId = eqpId;
            _side = side;

            _currentState = ProcessState.WaitingT1Response;

            string payload = string.Join(DELIM, "T1", _barcode, _recipe, _eqpId, _side);
            Send(payload);

            OnLogOccurred("T1 전송 완료. 서버 응답 대기 중...");
        }
        public int TempCount { get; set; } = 4;

        public void StartT2Process()
        {
            string svStr = string.Join(",", _sv);        
            string pvStr = string.Join(",", _pv);
            string sv = $"{pvStr},{svStr}";
            
            TempCount = _pv.Length;

            _currentState = ProcessState.WaitingT2Response;
            string payload = string.Join(DELIM, "T2", _barcode, _recipe, _eqpId, _side, TempCount.ToString("00"), sv);
            Send(payload);
            OnLogOccurred("T2 전송 완료. 서버 응답 대기 중...");
        }

        public void StartT3Process(string tempSv, string tempPv)
        {
            string svStr = string.Join(",", _sv);
            string pvStr = string.Join(",", _pv);
            string sv = $"{pvStr},{svStr}";

            TempCount = _pv.Length;

            _currentState = ProcessState.WaitingT2Response;
            string payload = string.Join(DELIM, "T3", _barcode, _recipe, _eqpId, _side, TempCount.ToString("00"), sv);
            Send(payload);
            OnLogOccurred("T3 전송 완료. 서버 응답 대기 중...");
        }

    }
}

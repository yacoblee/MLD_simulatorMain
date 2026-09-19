using camProject.Comm;
using System.IO.Ports;

namespace camProject.Model
{
    public class TcpModel : IDisposable
    {

        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;
        public event EventHandler<string> ProcessCompleted;

        // 2026-10-28 추가(문제4): MES 단계(T1~T3)가 NG 또는 타임아웃으로 실패했음을 상위에 알림. 인자는 실패 사유
        public event EventHandler<string>? ProcessFailed;

        public event EventHandler Connected; 
        public event EventHandler Disconnected;

        private System.Timers.Timer _retryTimer;
        private const int TimeoutSecond = 5000;


        public int TempCount { get; set; } = 4;
        private int[] _sv;
        private int[] _pv;
        public void UpdatePv(int[] pv) => _pv = pv;
        public void UpdateSv(int[] sv) => _sv = sv;
        public bool IsConnected => TcpComm.Instance.IsConnected;

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
        private const string DELIM = "/";

        public TcpModel()
        {
            TcpComm.Instance.DataReceived += OnData;
            TcpComm.Instance.LogOccurred += (s, m) => LogOccurred?.Invoke(this, m);
            TcpComm.Instance.Connected += (s, e) => Connected?.Invoke(this, e);
            TcpComm.Instance.DisconnectedEvent += (s, e) => Disconnected?.Invoke(this, e);
        }

        private void TimeOutHandler()
        {
            _retryTimer?.Stop();
            _retryTimer?.Dispose();

            _retryTimer = new System.Timers.Timer(TimeoutSecond) { AutoReset = false };
            _retryTimer.Elapsed += (s, e) =>
            {
                // [2026-10-28 원본]
                // OnLogOccurred($"{_currentState} 단계 응답 타임아웃 — 프로세스를 중단합니다.");
                // _currentState = ProcessState.Idle;

                // 2026-10-28 수정(문제4): 상태를 Idle로 되돌리기 전에 어느 단계였는지 보관하고, 실패를 상위(ViewModel)에 알림
                string timedOutState = _currentState.ToString();
                OnLogOccurred($"{timedOutState} 단계 응답 타임아웃 — 프로세스를 중단합니다.");
                _currentState = ProcessState.Idle;
                ProcessFailed?.Invoke(this, $"{timedOutState} 타임아웃");
            };
            _retryTimer.Start();
        }
        private void StopTimeoutWatch()
        {
            _retryTimer?.Stop();
        }

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);
        private void OnData(object sender, string text)
        {
            string[] tokens = text.Split('/');
            
            string cmdType = tokens[0];
            string result = tokens[tokens.Length - 1].Trim();

            if (result == "NG")
            {
                // [2026-10-28 원본]
                // StopTimeoutWatch();
                // _currentState = ProcessState.Idle;
                // return;

                // 2026-10-28 수정(문제4): NG 사유를 로그로 남기고 실패를 상위에 알림 (기존에는 아무 표시 없이 Idle로만 돌아갔음)
                StopTimeoutWatch();
                _currentState = ProcessState.Idle;
                OnLogOccurred($"{cmdType} 단계 NG 응답 — 프로세스를 중단합니다.");
                ProcessFailed?.Invoke(this, $"{cmdType} NG");
                return;
            }

            
            switch (_currentState)
            {
                case ProcessState.WaitingT1Response:
                    if (cmdType == "T1" && result == "OK")
                    {
                        StartT2Process();
                    }
                    break;
                case ProcessState.WaitingT2Response:
                    if (cmdType == "T2" && result == "OK")
                    {
                        StartT3Process();
                    }
                    break;
                case ProcessState.WaitingT3Response:
                    if (cmdType == "T3" && result == "OK")
                    {
                        StopTimeoutWatch();
                        _currentState = ProcessState.Idle;
                        OnLogOccurred($"TCP T3 END");
                        ProcessCompleted?.Invoke(this, _barcode);
                    }
                    break;
            }
        }



        public Task <bool> Connect(string ip, int port, int timeOutCnt, int retryCnt)
            => TcpComm.Instance.Connect( ip, port, timeOutCnt, retryCnt);

        public void DisConnect() => TcpComm.Instance.Disconnect();
        public void Send(string message){
            if (IsConnected)
            {
                TcpComm.Instance.SendFramed(message);
            }
            else {
                OnLogOccurred("통신에 문제가 있습니다");
            }


        }


        public void StartT1Process(string barcode, string recipe, string eqpId, string side, int[] pv, int[] sv)
        {
            if (!IsConnected) { OnLogOccurred("연결되지 않아 T1을 시작할 수 없습니다."); return; }
            if (_currentState != ProcessState.Idle) {
                OnLogOccurred("이미 진행중인 프로세스가 있습니다");
                return;
            }
            _barcode = barcode;
            _recipe = recipe;
            _eqpId = eqpId;
            _side = side;
            _sv = sv;
            _pv = pv;
            _currentState = ProcessState.WaitingT1Response;

            string payload = string.Join(DELIM, "T1", _barcode, _recipe, _eqpId, _side);
            OnLogOccurred($"T1 전송 파라미터 : {payload}");
            Send(payload);
            TimeOutHandler();
            OnLogOccurred("T1 전송 완료. 서버 응답 대기 중...");
        }

        public void StartT2Process()
        {
            string svStr = string.Join(",", _sv);
            string pvStr = string.Join(",", _pv);
            string sv = $"{pvStr},{svStr}";

            TempCount = _pv.Length;

            _currentState = ProcessState.WaitingT2Response;
            string payload = string.Join(DELIM, "T2", _barcode, _recipe, _eqpId, _side, TempCount.ToString("00"), sv);
            Send(payload);
            TimeOutHandler();
        }

        public void StartT3Process()
        {
            string svStr = string.Join(",", _sv);
            string pvStr = string.Join(",", _pv);
            string sv = $"{pvStr},{svStr}";

            TempCount = _pv.Length;

            _currentState = ProcessState.WaitingT3Response;
            string payload = string.Join(DELIM, "T3", _barcode, _recipe, _eqpId, _side, TempCount.ToString("00"), sv);
            Send(payload);
            TimeOutHandler();
        }


        public void Dispose  ()
        {
            TcpComm.Instance.Disconnect();
        }
    }
}

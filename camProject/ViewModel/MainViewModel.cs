using camProject.Comm;
using camProject.Model;
using OpenCvSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace camProject.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Model.MainModel _mainModel = new MainModel();
        Model.TcpModel _tcpModel = new TcpModel();

        private readonly object _logLock = new();
        public ObservableCollection<string> LogEntries { get; set; } = new();
        public ObservableCollection<int> Pv { get; } = new(new[] { 0, 0, 0, 0 });
        public ObservableCollection<int> Sv { get; } = new(new[] { 0, 0, 0, 0 });
        public ObservableCollection<string> RecipeList { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _selectRecipe;

        private string _serialButtonText = "연결";
        private string _tcpButtonText = "연결";
        private string _barcode = "";
        // [2026-10-28 원본] private int _readyPending;   (선언만 있고 어디에서도 쓰이지 않았음)

        // 2026-10-28 추가(문제1~4): 5단계 사이클 상태.
        // 장비 1차 동작 완료(_stage1Done)와 MES 단계 완료(_mesDone)가 모두 끝나야 2차 동작(:SWITCH2:ON?)을 보냄
        private enum Cycle { Idle, Stage1, Stage2 }
        private Cycle _cycle = Cycle.Idle;
        private bool _stage1Done;
        private bool _mesDone;
        private bool _mesStarted;
        private readonly object _cycleLock = new();            // 시리얼/TCP 수신 스레드와 UI 스레드가 동시에 상태를 바꾸므로 보호
        private readonly System.Timers.Timer _cycleTimer;
        private const int CycleTimeoutMs = 60000;              // 장비나 MES 중 하나가 끝내 응답하지 않아도 다음 바코드를 받을 수 있게 하는 제한시간

        private void OnPropertyChanged([CallerMemberName] string? name = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        public string SerialButtonText
        {
            get => _serialButtonText;
            set { _serialButtonText = value; OnPropertyChanged(); }
        }

        public string TcpButtonText
        {
            get => _tcpButtonText;
            set { _tcpButtonText = value; OnPropertyChanged(); }
        }

        // [2026-10-28 원본] 검증과 장비 명령 없이 값만 저장했고, 장비 준비 명령은 온도를 받을 때마다 보내졌음
        // public string ReceiveBarcode(string barcode)
        // {
        //     _barcode = barcode;
        //     return _barcode;
        // }

        // 2026-10-28 수정(문제1): 바코드 확정 시점에 장비 1차 동작(:SWITCH:ON?)을 한 번만 보냄.
        // 사이클이 진행 중이면 거부하고, 쉼표('/')가 들어간 바코드는 장비/MES 프로토콜을 깨뜨리므로 거부함
        public bool ConfirmBarcode(string text)
        {
            string code = (text ?? "").Trim();
            if (code.Length == 0) { LogEntries.Add("바코드를 입력하세요"); return false; }

            foreach (char c in code)
            {
                if (c == ',' || c == '/' || char.IsControl(c))
                {
                    LogEntries.Add("바코드에 사용할 수 없는 문자가 있습니다 (쉼표, 슬래시, 제어문자)");
                    return false;
                }
            }

            if (!_mainModel.IsOpen) { LogEntries.Add("Serial 미연결 - 장비 1차 동작 명령을 보낼 수 없습니다"); return false; }

            lock (_cycleLock)
            {
                if (_cycle != Cycle.Idle) { LogEntries.Add("이전 사이클이 진행 중입니다"); return false; }
                _barcode = code;
                _cycle = Cycle.Stage1;
                _stage1Done = false;
                _mesDone = false;
                _mesStarted = false;
            }

            _cycleTimer.Stop();
            _cycleTimer.Start();
            LogEntries.Add($"바코드 확정: {code} - 장비 1차 동작을 시작합니다");
            _mainModel.SendFramed(string.Join(",", code, ":SWITCH:ON?"));
            return true;
        }

        // 2026-10-28 추가(문제2): 1차 동작 완료와 MES 완료가 모두 끝났을 때만 2차 동작을 보냄 (먼저 끝난 쪽은 기다림)
        private void TrySendStage2()
        {
            string code;
            lock (_cycleLock)
            {
                if (_cycle != Cycle.Stage1 || !_stage1Done || !_mesDone) return;
                _cycle = Cycle.Stage2;
                code = _barcode;
            }

            LogEntries.Add("장비 1차 동작과 MES 단계가 모두 완료되어 2차 동작을 시작합니다");
            _mainModel.SendFramed(string.Join(",", code, ":SWITCH2:ON?"));
        }

        // 2026-10-28 추가(문제2): 장비 완료 보고(01DONE,OK,<단계>) 처리. 순서와 맞지 않는 보고는 무시
        private void OnStageDone(int stage)
        {
            bool cycleEnd = false;
            lock (_cycleLock)
            {
                if (stage == 1 && _cycle == Cycle.Stage1) _stage1Done = true;
                else if (stage == 2 && _cycle == Cycle.Stage2) { _cycle = Cycle.Idle; cycleEnd = true; }
                else return;
            }

            if (cycleEnd)
            {
                _cycleTimer.Stop();
                LogEntries.Add("사이클 완료 - 다음 바코드를 입력할 수 있습니다");
                return;
            }

            LogEntries.Add("장비 1차 동작 완료");
            TrySendStage2();
        }

        // 2026-10-28 추가(문제4): 어느 단계에서든 실패하면 사이클을 Idle로 되돌리고 장비를 정지 상태로 보냄
        private void AbortCycle(string reason)
        {
            string code;
            lock (_cycleLock)
            {
                if (_cycle == Cycle.Idle) return;
                _cycle = Cycle.Idle;
                _stage1Done = false;
                _mesDone = false;
                _mesStarted = false;
                code = _barcode;
            }

            _cycleTimer.Stop();
            LogEntries.Add($"사이클 중단: {reason}");
            if (_mainModel.IsOpen) _mainModel.SendFramed(string.Join(",", code, ":SWITCH2:OFF"));
        }

        // TCP가 끊겨도 MES가 이미 끝났다면(2차 동작 대기 중) 사이클을 계속 진행해야 하므로 MES 진행 중일 때만 중단
        private void AbortIfMesPending(string reason)
        {
            bool pending;
            lock (_cycleLock) { pending = _cycle == Cycle.Stage1 && !_mesDone; }
            if (pending) AbortCycle(reason);
        }
        public string? SelectRecipe
        {
            get => _selectRecipe;
            set
            {
                _selectRecipe = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                {
                    int[] sv = _mainModel.GetSelectedValue(value);
                    for (int i = 0; i <sv.Length && i < Sv.Count; i++) Sv[i] = sv[i];
                }
            }
        }
        public MainViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(LogEntries, new object());

            // 2026-10-28 추가(문제4): 사이클 제한시간. 이벤트가 연결되기 전에 발생하지 않도록 생성자 맨 앞에서 만듦
            _cycleTimer = new System.Timers.Timer(CycleTimeoutMs) { AutoReset = false };
            _cycleTimer.Elapsed += (s, e) => AbortCycle("사이클 제한시간 초과");

            // [2026-10-28 원본] 온도를 받을 때마다(30초 폴링) 장비 준비 명령(:SWITCH:ON?)을 보내서 서보가 계속 재구동되고
            //                   5단계에서 끈 LED1이 다시 켜졌음
            // _mainModel.TempReceived += (s, pv) =>
            //     Application.Current.Dispatcher.BeginInvoke(() =>
            //     {
            //         for (int i = 0; i < pv.Length; i++)
            //         {
            //             Pv[i] = pv[i];
            //         }
            //         if (!_mainModel.IsOpen) { LogEntries.Add("Serial 장비 미연결 - 구동 명령 전송 불가"); return; }
            //         _mainModel.SendFramed(string.Join(",", _barcode, ":SWITCH:ON?"));
            //     });

            // 2026-10-28 수정(문제1): 온도 수신은 PV 화면 갱신만 함. 장비 명령은 ConfirmBarcode에서 한 번만 보냄
            _mainModel.TempReceived += (s, pv) =>
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    for (int i = 0; i < pv.Length; i++)
                    {
                        Pv[i] = pv[i];
                    }
                });
            _mainModel.Disconnected += (s, e) => Application.Current.Dispatcher.Invoke(() => SerialButtonText = "연결");
            _tcpModel.Connected += (s, e) => Application.Current.Dispatcher.Invoke(() => TcpButtonText = "연결해제");
            _tcpModel.Disconnected += (s, e) => Application.Current.Dispatcher.Invoke(() => TcpButtonText = "연결");

            _mainModel.LogOccurred += (s, msg) => LogEntries.Add($"Serial {msg}");
            _tcpModel.LogOccurred += (s, msg) => LogEntries.Add($"TCP {msg}");

            LoadRecipeList();
            if (RecipeList.Count > 0) SelectRecipe = RecipeList[0];


            // [2026-10-28 원본] T3 완료 즉시 2차 동작 명령을 보내서, 1차 동작(6초)이 끝나기 전에 도착해 2차 동작이 따로 보이지 않았음
            // _tcpModel.ProcessCompleted += (s, barcode) =>
            // {
            //     if (!_mainModel.IsOpen) { LogEntries.Add("Serial 장비 미연결 - 구동 명령 전송 불가"); return; }
            //     _mainModel.SendFramed(string.Join(",", barcode, ":SWITCH2:ON?"));
            // };

            // 2026-10-28 수정(문제2): MES 완료만 기록하고, 장비 1차 동작 완료까지 기다렸다가 2차 동작을 보냄
            _tcpModel.ProcessCompleted += (s, bc) =>
            {
                lock (_cycleLock)
                {
                    if (_cycle != Cycle.Stage1) return;
                    _mesDone = true;
                }
                LogEntries.Add($"MES 단계(T1~T3) 완료: {bc}");
                TrySendStage2();
            };

            // 2026-10-28 추가(문제2,4): 장비 완료 보고, MES 실패, 연결 끊김을 사이클 상태에 반영
            _mainModel.StageDone += (s, stage) => OnStageDone(stage);
            _tcpModel.ProcessFailed += (s, reason) => AbortCycle($"MES 실패 ({reason})");
            _mainModel.Disconnected += (s, e) => AbortCycle("Serial 연결 끊김");
            _tcpModel.Disconnected += (s, e) => AbortIfMesPending("TCP 연결 끊김");
        }

         public void LoadRecipeList()
        {
            RecipeList.Clear();
            foreach(var item in _mainModel.GetFileList())
            {
                RecipeList.Add(item);
            }
        }

        public void SaveRecipe(string path)
        {
            _mainModel.SaveSv(Sv.ToArray(), path);
            LoadRecipeList();
        }


        public async Task<bool> ToggleSerialConnect(string port, int baud, Parity parity, int dataBits, StopBits stopBits)
        {
            if (_mainModel.IsOpen)
            {
                _mainModel.DisConnect(); 
                return false;
            }
            bool ok = _mainModel.Connect(port, baud, parity, dataBits, stopBits);

      
                //int[]? first = await _mainModel.WaitFirstTempAsync(3000);
            if (!ok)
            {
                LogEntries.Add("Serial 장비 응답 없음(3초) - 연결을 해제합니다");
                _mainModel.DisConnect();
                return false;
            }
            SerialButtonText = "연결해제";
            _mainModel.PollingTemp();
            return true;

        }

        public async Task<bool> TcpConnect(string ip, int port, int timeOutCnt, int retryCnt)
        {
            if (_tcpModel.IsConnected)
            {
                _tcpModel.DisConnect();
                return true;
            }

            bool ok = await _tcpModel.Connect(ip, port, timeOutCnt, retryCnt);
            if (ok)
            {
                return true;
            } else
            {
                return false;
            }
        }

        // [2026-10-28 원본] 텍스트박스 값을 그대로 T1에 사용해서, 바코드 확정 후 글자를 고치면 장비에 보낸 바코드와 달라질 수 있었음
        // public void TcpSend(string barcode)
        // {
        //     if (_tcpModel.IsConnected)
        //     {
        //         try
        //         {
        //             int[] pv = Pv.ToArray();
        //             int[] sv = Sv.ToArray();
        //             string recipe = _selectRecipe ?? "Default";
        //
        //             _tcpModel.StartT1Process(barcode, recipe, "TEST1", "Front", pv, sv);
        //         } catch (Exception ex)
        //         {
        //             MessageBox.Show($"데이터 전송 중 오류 발생: {ex.Message}", "오류");
        //         }
        //     }
        //     else
        //     {
        //         MessageBox.Show($"TCP 연결 상태가 아닙니다.");
        //     }
        // }

        // 2026-10-28 수정(문제1,2): 호출부 호환을 위해 barcode 매개변수는 남겼지만 사용하지 않고,
        // ConfirmBarcode로 확정된 _barcode를 사용함. 사이클당 MES 단계는 한 번만 시작. 오류는 팝업 대신 로그로 표시
        public void TcpSend(string barcode)
        {
            string code;
            lock (_cycleLock)
            {
                if (_cycle != Cycle.Stage1) { LogEntries.Add("바코드를 먼저 확정(입력 버튼)하세요"); return; }
                if (_mesStarted) { LogEntries.Add("이 사이클의 MES 단계는 이미 시작되었습니다"); return; }
                code = _barcode;
            }

            if (!_tcpModel.IsConnected) { LogEntries.Add("TCP 연결 상태가 아닙니다."); return; }

            try
            {
                int[] pv = Pv.ToArray();
                int[] sv = Sv.ToArray();
                string recipe = _selectRecipe ?? "Default";

                lock (_cycleLock) { _mesStarted = true; }
                _tcpModel.StartT1Process(code, recipe, "TEST1", "Front", pv, sv);
            }
            catch (Exception ex)
            {
                LogEntries.Add($"데이터 전송 중 오류 발생: {ex.Message}");
                AbortCycle("MES 전송 오류");
            }
        }


    }
}

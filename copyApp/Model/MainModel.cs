using copyApp.Conf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copyApp.Model
{
    public class MainModel
    {
        private System.Timers.Timer _tempTimer;
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;
        public event EventHandler<string> DataReceived;   // 수신 raw 데이터 브로드캐스트

        public ProtocolFIle Config;
        private System.Timers.Timer _startTimer;
        public event EventHandler<int[]> TempReceived;
        public event EventHandler<string> AckReceived;

        private readonly ManualResetEventSlim _rxSignal = new ManualResetEventSlim(false);

        // 프레이밍: STX + payload + CR LF (sendProject 규약)
        private const byte STX = 0x02;
        private string _rxBuffer = "";
        public bool IsConnected { get; private set; } = false;




        public MainModel(ProtocolFIle config)
        {
            Config = config;
            SerialComm.Instance.DataReceived += OnData;
            SerialComm.Instance.LogOccurred += (s, m) => OnLogOccurred(m);
            SerialComm.Instance.Connected += (s, e) => Connected?.Invoke(this, e);
        }

        private void OnData(object sender, string text)
        {
            _rxBuffer += text;

            if (_rxBuffer.Contains("TEMP"))
            {
                while (_rxBuffer.Contains("\n"))
                {
                    int idx = _rxBuffer.IndexOf("\n");
                    string msg = _rxBuffer.Substring(0, idx);
                    _rxBuffer = _rxBuffer.Substring(idx + 1);
                    msg = msg.Replace("\r", "").Replace("\u0002", "").Trim();
                    
                    if (string.IsNullOrWhiteSpace(msg)) continue;
                    TryParseTemp(msg);

                }

            }

            DataReceived?.Invoke(this, text);                     // View로 중계 (또는 파싱)
        }


        public void PollingTemp()
        {
            if (_tempTimer == null)
            {
                _tempTimer = new System.Timers.Timer(30000);
                _tempTimer.Elapsed += (s, e) =>
                {
                    if (!SerialComm.Instance.IsOpen) return;
                    SerialComm.Instance.CheckTemp();
                };
            }
            _tempTimer.Start();
            if(SerialComm.Instance.IsOpen) SerialComm.Instance.CheckTemp();
        }




        public void Save()
        {
            Config.PortName = Config.PortName;
            Config.BaudRateIndex = Config.BaudRateIndex;
            Config.ParityIndex = Config.ParityIndex;
            Config.BitIndex = Config.BitIndex;
            Config.LengthIndex = Config.LengthIndex;    
            Config.StopBitsIndex = Config.StopBitsIndex;
            Config.ProtocolIndex = Config.ProtocolIndex;
            Config.Save();

            OnLogOccurred("설정이 저장되었습니다.");
        }


        public Boolean Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, string protocolName )
        {
            try
            {

                if (SerialComm.Instance.IsOpen)
                {
                    SerialComm.Instance.Disconnect();

                    return false;
                }
                else
                {

                    IsConnected = SerialComm.Instance.Connect(portName, baudRate, parity, dataBits, stopBits); 
                
                    if(!IsConnected) { OnLogOccurred("포트 연결 실패"); return false; }
                    //SerialComm.Instance.CheckTemp();

                    OnLogOccurred(" 실제 연결 확인됨 (상대 응답 수신)");
                    
                    return true;
                   
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                OnLogOccurred($"연결 실패: {ex.Message}");
                return false;
            }
        }
        public async Task<int[]> WaitFirstTempAsync(int timeoutMs)
        {
            var tcs = new TaskCompletionSource<int[]>();
            void handler(object s, int[] t) { TempReceived -= handler; tcs.TrySetResult(t); }
            TempReceived += handler;

            if (SerialComm.Instance.IsOpen) SerialComm.Instance.CheckTemp();

            var done = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs));
            TempReceived -= handler;
            return done == tcs.Task ? tcs.Task.Result : null;
        }

        private void TryParseTemp(string msg)
        {
            try
            {
                string[] tokens = msg.Split(',');
                if (tokens.Length < 6) return;

                int[] pv  = new int[tokens.Length - 2]; // CR, LF 제외
                
                if (tokens[1].Trim() == "OK")
                {
                    for (int i = 0; i < pv.Length; i++)
                    {
                        pv[i] = Convert.ToInt32(tokens[i+2].Trim());
                    }
                    TempReceived?.Invoke(this, pv);
                }
            }
            catch (Exception ex)
            {
                OnLogOccurred($"온도값 파싱 오류: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            SerialComm.Instance.Disconnect();
            IsConnected = false;
        }

        public void Detach()
        {
            SerialComm.Instance.DataReceived -= OnData;
        }

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);
 





    }
}

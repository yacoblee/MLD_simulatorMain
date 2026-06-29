using copyApp.Conf;
using System;
using System.Collections.Generic;
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
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;
        public event EventHandler<string> DataReceived;   // 수신 raw 데이터 브로드캐스트


        public ProtocolFIle Config;
        private System.Timers.Timer _startTimer;

        // 프레이밍: STX + payload + CR LF (sendProject 규약)
        private const byte STX = 0x02;
        private readonly ManualResetEventSlim _rxSignal = new ManualResetEventSlim(false);

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
            DataReceived?.Invoke(this, text);                     // View로 중계 (또는 파싱)
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

                    bool result = SerialComm.Instance.Connect(portName, baudRate, parity, dataBits, stopBits); 
                
                    if(!result) { OnLogOccurred("포트 연결 실패"); return false; }
                    IsConnected = SerialComm.Instance.VerifyConnection();
                

                    // 포트 오픈 ≠ 실제 통신. probe로 상대 응답 확인
                    if (IsConnected)
                        OnLogOccurred(" 실제 연결 확인됨 (상대 응답 수신)");
                    else
                        OnLogOccurred(" 포트는 열렸으나 응답 없음 (상대 미연결/포트 불일치)");

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

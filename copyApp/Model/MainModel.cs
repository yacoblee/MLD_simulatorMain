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
        private SerialPort _serial;
        private System.Timers.Timer _startTimer;

        // 프레이밍: STX + payload + CR LF (sendProject 규약)
        private const byte STX = 0x02;
        private readonly ManualResetEventSlim _rxSignal = new ManualResetEventSlim(false);

        public bool IsStarted { get; private set; } = false;
        public bool IsConnected { get; private set; } = false;



        public MainModel(ProtocolFIle config)
        {
            this.Config = config;
            _serial = new SerialPort();
            _serial.DataReceived += OnSerialDataReceived;   // 한 번만 구독
        }

        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string text = _serial.ReadExisting();
                _rxSignal.Set();                            // 응답 도착 신호 (연결 확인용)
                DataReceived?.Invoke(this, text);           // View로 전달
            }
            catch (Exception ex)
            {
                OnLogOccurred($"수신 오류: {ex.Message}");
            }
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
                if (_serial.IsOpen) _serial.Close();

                _serial.PortName = portName;
                _serial.BaudRate = baudRate;
                _serial.Parity = parity;
                _serial.DataBits = dataBits;
                _serial.StopBits = stopBits;
                
                _serial.Open();
                IsConnected = _serial.IsOpen;
                Connected?.Invoke(this, EventArgs.Empty);
                OnLogOccurred($"포트 열림: {portName}, {baudRate}, {parity}, {dataBits}");

                // 포트 오픈 ≠ 실제 통신. probe로 상대 응답 확인
                if (VerifyConnection())
                    OnLogOccurred("✅ 실제 연결 확인됨 (상대 응답 수신)");
                else
                    OnLogOccurred("⚠ 포트는 열렸으나 응답 없음 (상대 미연결/포트 불일치)");

                return true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                OnLogOccurred($"연결 실패: {ex.Message}");
                return false;
            }
        }

        // 실제 통신 확인: SYNC probe 전송 후 timeoutMs 내 응답 도착 여부
        public bool VerifyConnection(int timeoutMs = 1000)
        {
            if (!_serial.IsOpen) return false;
            try
            {
                _rxSignal.Reset();
                SendFramed("01SYNC");                 // 비파괴적 probe
                return _rxSignal.Wait(timeoutMs);     // 응답 오면 true
            }
            catch (Exception ex)
            {
                OnLogOccurred($"연결 확인 실패: {ex.Message}");
                return false;
            }
        }

        // STX + payload + CR LF 로 프레이밍하여 전송
        public void SendFramed(string payload)
        {
            if (!_serial.IsOpen) return;
            byte[] body = Encoding.ASCII.GetBytes(payload);
            byte[] frame = new byte[body.Length + 3];
            frame[0] = STX;
            Array.Copy(body, 0, frame, 1, body.Length);
            frame[frame.Length - 2] = 0x0D;           // CR
            frame[frame.Length - 1] = 0x0A;           // LF
            _serial.Write(frame, 0, frame.Length);
        }

        public void Disconnect()
        {
            if (_serial.IsOpen)
            {
                _serial.Close();
                IsConnected = false;
                OnLogOccurred("연결 해제됨");
            }
        }   


        private void OnLogOccurred(string message)
        {
            LogOccurred?.Invoke(this, message);
        }





    }
}

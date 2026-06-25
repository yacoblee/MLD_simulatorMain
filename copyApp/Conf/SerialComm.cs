using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace copyApp.Conf
{
    /// <summary>
    /// 시리얼 통신 전담 모듈. 포트 소유/송수신/프레이밍을 담당하고,
    /// 수신 결과를 이벤트로 broadcast 한다. (UI/Model을 모름)
    /// </summary>
    public class SerialComm : IDisposable
    {
        public static SerialComm Instance { get; } = new SerialComm(); 

        private SerialPort _serial = new SerialPort();
        private readonly ManualResetEventSlim _rxSignal = new ManualResetEventSlim(false);
        private const byte STX = 0x02;

        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;
        public bool IsOpen => _serial.IsOpen;

        private SerialComm()
        {
            _serial.DataReceived += OnSerialDataReceived;
        }


        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string text = _serial.ReadExisting();
                _rxSignal.Set();
                DataReceived?.Invoke(this, text);   // 모든 구독자에게 broadcast
            }
            catch (Exception ex) { OnLogOccurred($"수신 오류: {ex.Message}"); }
        }

        public bool Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
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

                if (_serial.IsOpen)
                {
                    Connected?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                else
                {   
                    OnLogOccurred("포트 열기 실패");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogOccurred?.Invoke(this, $"연결 오류: {ex.Message}");
                return false;
            }
           
        }

        public void Disconnect()
        {
            if (_serial.IsOpen) { 
                _serial.Close(); 
                OnLogOccurred("포트 닫힘");
            }
        }   

        public void SendFramed(string payload)
        {
            if (!_serial.IsOpen) return;
            byte[] body = Encoding.ASCII.GetBytes(payload);
            byte[] framed = new byte[body.Length + 3];
            framed[0] = STX;
            Array.Copy(body, 0, framed, 1, body.Length);
            framed[framed.Length - 2] = 0x0D; // CR
            framed[framed.Length - 1] = 0x0A; // LF 
            _serial.Write(framed, 0, framed.Length);
        }

        public bool VerifyConnection(int timeoutMs = 1000)  
        {
            if (!_serial.IsOpen) return false;
            _rxSignal.Reset();
            SendFramed("01SYNC");
            return _rxSignal.Wait(timeoutMs);
        }

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);   
 

        public void Dispose()
        {
            _serial.DataReceived -= OnSerialDataReceived;
            if (_serial.IsOpen) _serial.Close();
            _serial.Dispose();
        }
    }
}

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
        private readonly SerialPort _serial = new SerialPort();
        private readonly ManualResetEventSlim _rxSignal = new ManualResetEventSlim(false);

        // 프레이밍: STX + payload + CR LF (sendProject 규약)
        private const byte STX = 0x02;

        public event EventHandler<string> DataReceived;   // 수신 raw 데이터 broadcast
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;

        public bool IsOpen => _serial.IsOpen;

        public SerialComm()
        {
            _serial.DataReceived += OnSerialDataReceived;
        }

        // 설정 + 오픈 + 실제 연결 확인
        public bool Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        // STX + payload + CR LF 로 프레이밍하여 전송
        public void SendFramed(string payload)
        {
            throw new NotImplementedException();
        }

        // probe 전송 후 timeoutMs 내 응답 도착 여부
        public bool VerifyConnection(int timeoutMs = 1000)
        {
            throw new NotImplementedException();
        }

        private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnLogOccurred(string message)
        {
            LogOccurred?.Invoke(this, message);
        }

        public void Dispose()
        {
            _serial.DataReceived -= OnSerialDataReceived;
            if (_serial.IsOpen) _serial.Close();
            _serial.Dispose();
        }
    }
}

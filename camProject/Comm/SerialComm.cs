using System.IO.Ports;
using System.Text;

namespace camProject.Comm
{
    // 이식 대상: copyApp/Conf/SerialComm.cs — UI 비의존 계층이라 로직 거의 1:1 포팅 가능
    //
    // 필요한 멤버:
    //   - public static SerialComm Instance { get; }   (싱글톤, private 생성자)
    //   - public bool IsOpen { get; }
    //   - event EventHandler<string> DataReceived
    //   - event EventHandler<string> LogOccurred
    //   - event EventHandler Connected
    //   - bool Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    //   - void Disconnect()
    //   - void SendFramed(string payload)        // STX + payload + CR LF
    //   - void CheckTemp()                       // SendFramed("01TEMP")
    //   - bool VerifyConnection(int timeoutMs = 1000)
    //   - void Dispose()  (IDisposable)
    //
    // 포팅 시 주의:
    //   - using System.IO.Ports; 필요 → camProject.csproj에 System.IO.Ports 패키지 이미 추가되어 있음
    //   - 그 외 프레임워크 차이 없음 — 원본 로직 그대로 옮겨도 됨
    public class SerialComm : IDisposable
    {
        public static SerialComm Instance { get; } = new SerialComm();

        private SerialPort _serial = new SerialPort();
        private readonly ManualResetEventSlim _rxSignal = new ManualResetEventSlim(false);
        private const byte STX = 0x02;
        private string _rxBuffer = "";
        private const int MaxRetry = 3;
        private int _rtyCnt = MaxRetry;

        public event EventHandler? Disconnected;
        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;
        public event EventHandler Connected;
        public bool IsOpen => _serial.IsOpen;
        private CancellationTokenSource? _healthCts;
        private TaskCompletionSource<string>? _healthTcs;
        private Task? _health;


        private SerialComm() {
            _serial.DataReceived += OnSerialDataReceived;
        }

        public void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                int bylen = _serial.BytesToRead;
                byte[] data = new byte[bylen];
                _serial.Read(data, 0, bylen);

                _rxBuffer = Encoding.UTF8.GetString(data);
                
                _rxSignal.Set();
                while (_rxBuffer.Contains("\n"))
                {
                    int idx = _rxBuffer.IndexOf("\n");
                    string frame = _rxBuffer.Substring(0, idx);
                    _rxBuffer = _rxBuffer.Substring(idx + 1);

                    string clean = frame.Replace("\u0002", "").Replace("\r", "").Trim();
                    if (string.IsNullOrWhiteSpace(clean)) continue;

                    _healthTcs?.TrySetResult(clean);
                    DataReceived?.Invoke(this, clean);
                }
            } catch (Exception ex) { 
                OnLogOccurred($"수신 오류: {ex.Message}"); 
            }
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

                if(_serial.IsOpen)
                {
                    Connected?.Invoke(this, EventArgs.Empty);
                }
                HealthCheck();

                return true;
             
            } catch (Exception ex) {
                OnLogOccurred($"연결 오류: {ex.Message}");

                Disconnect();
                return false;
            }
        }
        public void sendFramed(string payload)
        {
            if (!_serial.IsOpen) return;

            byte[] box = Encoding.ASCII.GetBytes(payload);
            byte[] framed = new byte[box.Length + 3];

            framed[0] = STX;
            Array.Copy(box, 0, framed, 1, box.Length);
            framed[framed.Length - 2] = 0x0D; // CR
            framed[framed.Length - 1] = 0x0A; // LF 
            _serial.Write(framed, 0, framed.Length);

        }
        private async Task<bool> sendAsyncPackage(string payload)
        {
            if (_serial == null || !_serial.IsOpen) return false;

            try
            {
                var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                _healthTcs = tcs;

                byte[] box = Encoding.ASCII.GetBytes(payload);
                byte[] pingPacket = new byte[box.Length + 3];

                pingPacket[0] = STX;
                Array.Copy(box, 0, pingPacket, 1, box.Length);             
                pingPacket[pingPacket.Length - 2] = 0x0D; // CR
                pingPacket[pingPacket.Length - 1] = 0x0A; // LF 

                await _serial.BaseStream.WriteAsync(pingPacket, 0, pingPacket.Length);
                await _serial.BaseStream.FlushAsync();

                var completed = await Task.WhenAny(tcs.Task, Task.Delay(2000));
                _healthTcs = null;

                if (completed != tcs.Task) return false;
                string[] parts = tcs.Task.Result.Split(',');
                return parts.Length >= 2 && parts[1].Trim() == "OK";
            }
            catch (Exception ex)
            {
                OnLogOccurred($"장비 체크 데이터 전송 실패{ex.ToString}");
                return false;
            }
        }

        //public void Disconnect()
        //{
        //    _healthCts?.Cancel();
        //    _healthCts?.Dispose();
        //    _healthCts = null;

        //    _serial.Close();

        //}
        public void Disconnect()
        {
            _healthCts?.Cancel();
            _healthCts?.Dispose();
            _healthCts = null;

            bool wasOpen = _serial.IsOpen;
            if (wasOpen) _serial.Close();
            if (wasOpen) Disconnected?.Invoke(this, EventArgs.Empty);   // 추가
        }
        public void CheckTemp()
        {
            if (!_serial.IsOpen) return;
            _rxSignal.Reset();
            sendFramed("01TEMP");
        }

        private void HealthCheck ()
        {

            _healthCts?.Cancel();
            _healthCts?.Dispose();

            _healthCts = new CancellationTokenSource();
            var token = _healthCts.Token;

            _health = Task.Run( async () =>
            {
                using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

                try
                {
                    while (await timer.WaitForNextTickAsync(token))
                    {
                        if (!_serial.IsOpen) break;

                        bool ok = await sendAsyncPackage("CHECK");

                        if (ok) { _rtyCnt = MaxRetry; continue; }

                        _rtyCnt--;
                        OnLogOccurred($"헬스체크 실패 — 재시도 현황 {_rtyCnt}/{MaxRetry}");

                        if (_rtyCnt <= 0)
                        {
                            OnLogOccurred("장비 연결 끊김 확인 — 연결 해제합니다.");
                            Disconnect();
                            break;
                        }


                    }
                }
                catch (Exception ex)
                {
                    OnLogOccurred($"장비 체크 통신 에러 {ex.ToString()}");
                }
            }, token);
        }




        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);

        public void Dispose()
        {
            Disconnect();
            _serial.Dispose();
        }

    }
}

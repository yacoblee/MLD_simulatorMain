using System.IO.Ports;
using System.Text;

namespace VersionUp.common
{

    public sealed class SerialManager : IDisposable
    {
        public static SerialManager Instance { get; } = new();

        private readonly SerialPort _serial = new();
        private string _rxBuffer = "";

        /// <summary>Broadcasts raw received text to all subscribers.</summary>
        public event Action<string>? OnDataReceived;

        public bool IsOpen => _serial.IsOpen;

        private SerialManager()
        {
            _serial.DataReceived += OnRx;
        }

        // --- core serial port configuration centralized here ---
        public void Open(string portName, int baudRate,
            Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            if (_serial.IsOpen) return;
            _serial.PortName = portName;
            _serial.BaudRate = baudRate;
            _serial.Parity = parity;
            _serial.DataBits = dataBits;
            _serial.StopBits = stopBits;
            _serial.Encoding = Encoding.ASCII;
            _serial.Open();
        }

        public void Send(string s)
        {
            if (_serial.IsOpen) _serial.Write(s);
        }

        public void Close()
        {
            if (_serial.IsOpen) _serial.Close();
        }

        private void OnRx(object? sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!_serial.IsOpen) return;

                int len = _serial.BytesToRead;
                if (len <= 0) return;

                byte[] buf = new byte[len];
                _serial.Read(buf, 0, buf.Length);
                string text = Encoding.ASCII.GetString(buf);

                OnDataReceived?.Invoke(text);   // fan-out raw data; subscribers parse
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DataReceived: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _serial.DataReceived -= OnRx;
            Close();
            _serial.Dispose();
        }
    }
}

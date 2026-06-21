using jdh.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jdh.Model
{
    public class SerialModel
    {
        public event EventHandler<string> LogOccurred;
        public event EventHandler<bool> AutoConnectStarted;
        public event EventHandler<bool> Connected;


        public SerialConfig Config;
        private SerialPort _serial;

        // 타이머 시작/중지시
        public bool IsStarted { get; private set; } = false;
        // 연결 성공/실패시
        public bool IsConnected { get; private set; } = false;

        // 자동 연결 타이머
        private System.Timers.Timer _startTimer;

        public SerialModel(SerialConfig config)
        {
            Config = config;
            _serial = new SerialPort();
            _startTimer = new System.Timers.Timer(3000);
            _startTimer.Elapsed += _startTimer_Elapsed;
        }

        // 데이터 저장
        public void Save()
        {
            Config.PortName = Config.PortName;
            Config.BaudRateIndex = Config.BaudRateIndex;
            Config.Save();
            OnLogOccurred($"설정이 저장되었습니다.");
        }

        public void Start()
        {
            _serial.PortName = Config.PortName;
            _serial.BaudRate = int.Parse(ConvertBaudRate(Config.BaudRateIndex));
            SerialAutoConnectStart();
            _startTimer.Start();
            IsStarted = true;
            OnStart(IsStarted);
            OnLogOccurred($"자동 연결이 시작되었습니다.");
        }
        public void Stop()
        {
            _startTimer.Stop();
            IsStarted = false;
            OnStart(IsStarted);
            OnLogOccurred($"자동 연결이 중지되었습니다.");
        }

        // 자동 연결 시도
        private void _startTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SerialAutoConnectStart();
        }

        private void SerialAutoConnectStart()
        {
            if (!_serial.IsOpen)
            {
                try
                {
                    OnLogOccurred($"연결을 시도합니다. PortName={Config.PortName}, BaudRate={ConvertBaudRate(Config.BaudRateIndex)}, Parity={ConvertParity(Config.ParityIndex)}");
                    _serial.Open();
                    if (_serial.IsOpen)
                    {
                        _startTimer.Stop();
                        IsConnected = true;
                        OnConnected(IsConnected);
                        OnLogOccurred($"연결되었습니다. PortName={Config.PortName}, BaudRate={ConvertBaudRate(Config.BaudRateIndex)}, Parity={ConvertParity(Config.ParityIndex)}");
                    }
                    else
                    {
                        IsConnected = false;
                        OnConnected(IsConnected);
                        OnLogOccurred($"연결에 실패하였습니다. PortName={Config.PortName}, BaudRate={ConvertBaudRate(Config.BaudRateIndex)}, Parity={ConvertParity(Config.ParityIndex)}");
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        private void OnLogOccurred(string message)
        {
            LogOccurred?.Invoke(this, message);
        }
        private void OnConnected(bool isConnected)
        {
            Connected?.Invoke(this, isConnected);
        }
        private void OnStart(bool isStarted)
        {
            AutoConnectStarted?.Invoke(this, isStarted);
        }
        public string ConvertBaudRate(int baudRateIndex)
        {
            switch (baudRateIndex)
            {
                case 0: return "9600";
                case 1: return "19200";
                case 2: return "38400";
                case 3: return "57600";
                case 4: return "115200";
                default: return "9600";
            }
        }
        public string ConvertParity(int parityIndex)
        {
            switch (parityIndex)
            {
                case 0: return "None";
                case 1: return "Odd";
                case 2: return "Even";
                case 3: return "Mark";
                case 4: return "Space";
                default: return "None";
            }
        }
    }
}

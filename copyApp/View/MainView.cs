using copyApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Forms;

namespace copyApp
{
    public partial class MainView : Form
    {

        private MainModel _model;

        private readonly BindingList<int> _speeds =
            new BindingList<int> { 4800, 9600, 19200, 38400, 57600, 76800, 115200 };
        private readonly BindingList<int> _dataBits = new BindingList<int> { 7, 8, 9 };
        private readonly BindingList<Parity> _parity = new BindingList<Parity> { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
        private readonly BindingList<StopBits> _stopBits = new BindingList<StopBits> { StopBits.One, StopBits.Two, StopBits.OnePointFive };
        private readonly BindingList<string> _protocols = new BindingList<string> { "PC-LINK STD", "PC-LINK+SUM", "MODBUS ASCII", "MODBUS RTU" };
        private readonly BindingList<string> _ports = new BindingList<string>();


        public int SelectedBaud => (int)cmbSpeed.SelectedItem;
        public Parity SelectedParity => (Parity)cmbBit.SelectedItem;
        public int SelectedDataBits => (int)cmbLength.SelectedItem;
        public StopBits SelectedStopBits => (StopBits)cmbStopBit.SelectedItem;
        public string SelectedProtocol => cmbProtocol.SelectedItem as string;
        public string SelectedPort => cmbPortName.SelectedItem as string;

        private Boolean con = false;


        public MainView(MainModel model)
        {
            _model = model;
            _model.LogOccurred += _model_LogOccurred;
            _model.DataReceived += (s, data) => WriteLog($"[수신] {data}");

            InitializeComponent();
            BindControls();
            RestoreFromConfig();        // 저장된 설정을 콤보박스에 복원
        }



        private void BindControls()
        {
            cmbSpeed.DataSource = _speeds;
            cmbBit.DataSource = Enum.GetValues(typeof(Parity));
            cmbLength.DataSource = _dataBits;
            cmbStopBit.DataSource = _stopBits;
            cmbProtocol.DataSource = _protocols;

            foreach (var p in SerialPort.GetPortNames()) _ports.Add(p);
            cmbPortName.DataSource = _ports;
        }


        private void RestoreFromConfig()
        {
            var c = _model.Config;
            cmbSpeed.SelectedIndex    = c.BaudRateIndex;   // 저장: _speeds.IndexOf(...)
            cmbBit.SelectedIndex      = c.ParityIndex;     // 저장: _parity.IndexOf(...)
            cmbLength.SelectedIndex   = c.BitIndex;        // 저장: _dataBits.IndexOf(...)
            cmbStopBit.SelectedIndex  = c.StopBitsIndex;   // 저장: _stopBits.IndexOf(...)
            cmbProtocol.SelectedIndex = c.ProtocolIndex;   // 저장: _protocols.IndexOf(...)
            if (_ports.Contains(c.PortName))               // 포트는 문자열로 저장
                cmbPortName.SelectedItem = c.PortName;
        }


        private void _model_LogOccurred(object sender, string log)
        {
            WriteLog(log);
        }

        public void WriteLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { WriteLog(message); }));
                return;
            }
            if (IsDisposed) { return; }
            textLog.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]: {message}\r\n");

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {

            _model.Config.PortName = SelectedPort;
            _model.Config.BitIndex = _dataBits.IndexOf(SelectedDataBits);   
            _model.Config.BaudRateIndex = _speeds.IndexOf(SelectedBaud);    
            _model.Config.ParityIndex = _parity.IndexOf(SelectedParity);
            _model.Config.StopBitsIndex = _stopBits.IndexOf(SelectedStopBits);
            _model.Config.ProtocolIndex = _protocols.IndexOf(SelectedProtocol); 

            _model.Save();
        }

        private void connBtn_Click(object sender, EventArgs e)
        {

            if(con)
            {
                _model.Disconnect();
                connBtn.Text = "연결";
            }
            else
            {
                con= _model.Connect(SelectedPort, SelectedBaud, SelectedParity, SelectedDataBits, SelectedStopBits, SelectedProtocol);
                connBtn.Text = "연결해제";
            }

        }
    }
}

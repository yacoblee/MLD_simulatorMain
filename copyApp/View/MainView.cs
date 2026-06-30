using copyApp.Model;
using copyApp.View;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copyApp
{
    public partial class MainView : Form
    {

        private MainModel _model;
        private TcpModel _tcpModel;

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

        private int timeValue = 0;
        private int retryValue = 0;

        public MainView(MainModel model, TcpModel tcpModel)
        {
            _model = model;
            _tcpModel = tcpModel;

            _model.LogOccurred += _model_LogOccurred;
            _model.DataReceived += (s, data) => WriteLog($"[수신] {data}");


            _tcpModel.LogOccurred += _model_LogOccurred;
            _tcpModel.DataReceived += (s, data) => WriteLog($"[TCP 수신] {data}");


            InitializeComponent();
            BindControls();
            RestoreFromConfig();
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

            serialBtn.CheckedChanged += Connected_RadioBtn;
            tcpBtn.CheckedChanged += Connected_RadioBtn;

            UpdateMainUI();
        }


        private void RestoreFromConfig()
        {
            var c = _model.Config;
            cmbSpeed.SelectedIndex    = c.BaudRateIndex;    
            cmbBit.SelectedIndex      = c.ParityIndex;     
            cmbLength.SelectedIndex   = c.BitIndex;         
            cmbStopBit.SelectedIndex  = c.StopBitsIndex;    
            cmbProtocol.SelectedIndex = c.ProtocolIndex;  
            if (_ports.Contains(c.PortName))     
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
            var ok = _model.Connect(SelectedPort, SelectedBaud, SelectedParity, SelectedDataBits, SelectedStopBits, SelectedProtocol);
            connBtn.Text = ok ? "연결해제" : "연결";
        }


       
        private async void TcpConnBtn_Click(object sender, EventArgs e)
        {
            if (pasiveRadio.Checked) // 장비 역할
            {
                if (!_tcpModel.IsConnected)
                {
                    if (!int.TryParse(timeTxt.Text, out timeValue) || !int.TryParse(retryTxt.Text, out retryValue))
                    {
                        MessageBox.Show("시간과 재시도 횟수는 올바른 숫자 형식이어야 합니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ;
                    }

                    if (timeValue <= 0 || retryValue < 0)
                    {
                        MessageBox.Show("시간은 0보다 커야 하며, 재시도 횟수는 0 이상이어야 합니다.", "입력 범위 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ;
                    }

                    await _tcpModel.Connect(ipTxt.Text, int.Parse(portTxt.Text),  timeValue, retryValue);
                }
                else
                {
                    _tcpModel.Disconnect();
                }

            } else if (activeRadio.Checked) {

                if (!_tcpModel.ServerConnected) {
                    _tcpModel.ServerConn(ipTxt.Text, int.Parse(portTxt.Text));
                } else { 
                    _tcpModel.Disconnect();
                }
            }


            tcpConBtn.Text = (_tcpModel.IsConnected || _tcpModel.ServerConnected) ? "연결해제" : "연결";
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _model.Detach();
            _tcpModel.Detach();
            base.OnFormClosed(e);
        }



        private void Connected_RadioBtn(Object sender, EventArgs e)
        {
            if(sender is RadioButton rb && rb.Checked)
            {
                UpdateMainUI();
            }
        }


        private void UpdateMainUI()
        {
            bool isTcp = tcpBtn.Checked;


            if (isTcp)
            {
                tcpPanel.Show();
                serialPanel.Hide();
            }
            else
            {
                tcpPanel.Hide();
                serialPanel.Show();
            }
        }

        private void barBtn_Click(object sender, EventArgs e)
        {
            using (codePopup popup = new codePopup())
            {
                if (popup.ShowDialog() == DialogResult.OK)
                    barTxt.Text = popup.Code;
            }
        }
    }
}

using copyApp.Comm;
using copyApp.Model;
using copyApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace copyApp
{
    public partial class MainView : Form
    {

        private MainModel _model;
        private TcpModel _tcpModel;
        private ConfigFile _cnf;
        private readonly BindingList<int> _speeds =
            new BindingList<int> { 4800, 9600, 19200, 38400, 57600, 76800, 115200 };
        private readonly BindingList<int> _dataBits = new BindingList<int> { 7, 8, 9 };
        private readonly BindingList<Parity> _parity = new BindingList<Parity> { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
        private readonly BindingList<StopBits> _stopBits = new BindingList<StopBits> { StopBits.One, StopBits.Two, StopBits.OnePointFive };
        private readonly BindingList<string> _protocols = new BindingList<string> { "PC-LINK STD", "PC-LINK+SUM", "MODBUS ASCII", "MODBUS RTU" };
        private readonly BindingList<string> _ports = new BindingList<string>();



        private const byte STX = 0x02;
        private const byte ETX = 0x03;  
        private const string RES_OK = "OK";
        private const string RES_NO = "NG";
        private string productID = "pdx-0012";



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
            Theme.Apply(this);

            _model = model;
            _tcpModel = tcpModel;

            _model.LogOccurred += _model_LogOccurred;
            _model.DataReceived += (s, data) => WriteLog($"[수신] {data}");
            _model.TempReceived += (s, temps) =>
            {
                SetPvRow(temps);
                _tcpModel.UpdatePv(temps);
            };

            _tcpModel.LogOccurred += _model_LogOccurred;
            _tcpModel.DataReceived += (s, data) => WriteLog($"[TCP 수신] {data}");
            _tcpModel.ProcessCompleted += (s, barcode) => _model.SendFramed(string.Join(",", barcode, ":SWITCH2:ON?"));
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
            SetupTempGrid();
            RefreshRegistBox();
            UpdateMainUI();
        }

        private void SetupTempGrid()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = true;

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();   // 0행: PV
            dataGridView1.Rows.Add();   // 1행: SV

            dataGridView1.Rows[0].HeaderCell.Value = "now Temp";
            dataGridView1.Rows[1].HeaderCell.Value = "set Temp";

            dataGridView1.Rows[0].ReadOnly = true;    // PV는 표시만
            dataGridView1.Rows[1].ReadOnly = false;   // SV는 입력 가능

            for (int c = 0; c < dataGridView1.Columns.Count; c++)
            {
                dataGridView1.Rows[0].Cells[c].Value = "-";   // PV 초기값
                dataGridView1.Rows[1].Cells[c].Value = "0";   // SV 초기값
            }
        }

        public void SetPvRow(int[] temps)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() => SetPvRow(temps)));
                return;
            }
            if (IsDisposed) return;

            int n = Math.Min(temps.Length, dataGridView1.Columns.Count);
            for (int c = 0; c < n; c++)
                dataGridView1.Rows[0].Cells[c].Value = temps[c].ToString("0");
        }


        public int[] GetSvValues()
        {
            var sv = new int[dataGridView1.Columns.Count];
            for (int c = 0; c < sv.Length; c++)
                int.TryParse(dataGridView1.Rows[1].Cells[c].Value?.ToString(), out sv[c]);
            return sv;
        }

        private void RefreshRegistBox(string selectFileName = "")
        {
            var fileList = _model.GetFileList();
            registBox.DataSource = null;
            registBox.DataSource = fileList;

            if (!string.IsNullOrEmpty(selectFileName) && fileList.Contains(selectFileName))
            {
                registBox.SelectedItem = selectFileName;
            }
        }

        private void SetSvRow(int[] svValues)
        {
            if (svValues == null) return;
            if (dataGridView1.Rows.Count < 2) return;

            int columnCount = dataGridView1.Columns.Count;
            for (int c = 0; c < columnCount; c++)
            {
                string val = (c < svValues.Length) ? svValues[c].ToString() : "0";
                dataGridView1.Rows[1].Cells[c].Value = val;
            }
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



        private async Task<bool> PreviousConn () 
        {
            int[] temps = await _model.WaitFirstTempAsync(3000);  
            if (temps == null) { MessageBox.Show("온도값 수신 실패(타임아웃)"); return false; }
            return true;
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


        private void tcpSend_Click(object sender, EventArgs e)
        {
            if (_tcpModel.IsConnected)
            {
                try
                {
                    string _barcode = barTxt.Text.Trim();
                    string _recipe = registBox.SelectedItem?.ToString();
                    string _eqpId = productID;
                    string _side = "FRONT";

                    _tcpModel.StartT1Process(_barcode, _recipe, _eqpId, _side);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터 전송 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else
            {
                MessageBox.Show("통신이 연결되지 않은 상태입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

      
        }



        private void registBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFileName = registBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedFileName)) return;

            int[] svValues = _model.GetSelectedValue(selectedFileName);
            _tcpModel.UpdateSv(svValues);
            SetSvRow(svValues);
        }

        private void regiBtn_Click(object sender, EventArgs e)
        {
            int[] sv = GetSvValues();

            using (var dlg = new SaveFileDialog())
            {
                System.IO.Directory.CreateDirectory(MainModel.RecipeDir);
                dlg.InitialDirectory = MainModel.RecipeDir;
                dlg.Filter = "레시피 (*.json)|*.json";
                dlg.DefaultExt = "json";
                dlg.AddExtension = true;

                if (dlg.ShowDialog() != DialogResult.OK) return;

                _model.SaveSv(sv, dlg.FileName);

                RefreshRegistBox(System.IO.Path.GetFileNameWithoutExtension(dlg.FileName));
            }
        }

        private async Task<bool> connBtn_Click(object sender, EventArgs e)
        {
            var ok = _model.Connect(SelectedPort, SelectedBaud, SelectedParity, SelectedDataBits, SelectedStopBits, SelectedProtocol);
            connBtn.Text = ok ? "연결해제" : "연결";

            if (!ok) return false;

            bool ok2 = await PreviousConn();
            if (!ok2)
            {
                _model.Disconnect();
                connBtn.Text = "연결";
            }
            return ok2;
        }

        private void serialsnd_Click(object sender, EventArgs e)
        {
            string barcodTxt = string.Join(",", barTxt.Text, ":SWITCH:ON?");
            _model.SendFramed(barcodTxt);
        }

    }
}

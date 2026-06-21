using jdh.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jdh.View
{
    public partial class SerialView : Form
    {
        private SerialModel _model;

        public SerialView(SerialModel model)
        {
            InitializeComponent();
            _model = model;
            _model.LogOccurred += _model_LogOccurred;
            _model.AutoConnectStarted += _model_AutoConnectStarted;
            _model.Connected += _model_Connected;

            UpdateUi();

            WriteLog("프로그램 실행 완료.");
        }

        private void _model_Connected(object sender, bool isConnected)
        {
            UpdateUiConnect(isConnected);
        }

        private void _model_AutoConnectStarted(object sender, bool isStarted)
        {
            UpdateUiStart(isStarted);
        }

        private void _model_LogOccurred(object sender, string log)
        {
            WriteLog(log);
        }

        public void UpdateUi()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { UpdateUi(); }));
                return;
            }

            if (IsDisposed) { return; }

            cmbPortName.Text = _model.Config.PortName;
            cmbBaudRate.SelectedIndex = _model.Config.BaudRateIndex;
            cmbParity.SelectedIndex = _model.Config.ParityIndex;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _model.Config.PortName = cmbPortName.Text;
            _model.Config.BaudRateIndex = cmbBaudRate.SelectedIndex;
            _model.Config.ParityIndex = cmbParity.SelectedIndex;
            _model.Save();
        }
        public void WriteLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { WriteLog(message); }));
                return;
            }
            if (IsDisposed) { return; }
            txtLog.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]: {message}\r\n");

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!_model.IsStarted)
            {
                _model.Config.PortName = cmbPortName.Text;
                _model.Config.BaudRateIndex = cmbBaudRate.SelectedIndex;
                _model.Config.ParityIndex = cmbParity.SelectedIndex;
                _model.Start();
            }
            else
            {
                _model.Stop();
            }
        }
        private void UpdateUiStart(bool isStarted)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { UpdateUiStart(isStarted); }));
                return;
            }
            if (IsDisposed) { return; }

            cmbPortName.Enabled = !isStarted;
            cmbBaudRate.Enabled = !isStarted;
            cmbParity.Enabled = !isStarted;
            btnStart.Text = isStarted ? "Stop" : "Start";
        }
        private void UpdateUiConnect(bool isConnected)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { UpdateUiConnect(isConnected); }));
                return;
            }
            if (IsDisposed) { return; }

            btnStart.Text = isConnected ? "Disconnected" : "Start";
        }
    }
}

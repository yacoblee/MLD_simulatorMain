using System;
using System.Windows.Forms;

namespace test
{
    public partial class TcpForm : Form
    {
        private readonly TcpConnect _tcp;

        public TcpForm()
        {
            InitializeComponent();

            _tcp = Program.MainTcp;
            _tcp.OnLogMessage += Tcp_OnLogMessage;
        }

        private void Tcp_OnLogMessage(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => logTxt.Text += msg));
            }
            else
            {
                logTxt.Text += msg;
            }
        }

        private async void tcpConn_Click(object sender, EventArgs e)
        {
            try
            {
                string ip = ipBox.Text.Trim();
                int port = int.Parse(portBox.Text.Trim());

                if (tcpConn.Text == "Start Server" || tcpConn.Text == "Connect")
                {
                    if (serBtn.Checked)
                    {
                        // =======================================================
                        // ★ 내 팝업창 함수가 아니라, _tcp 객체 안의 함수를 실행!
                        // =======================================================
                        _ = _tcp.StartServerAsync(ip, port); // Task 잊음 처리
                        tcpConn.Text = "Stop Server";
                    }
                    else if (cliBtn.Checked)
                    {
                        int timeoutMs = (int)timeOutUpDown.Value;
                        int retrySec = (int)reConnUpDown.Value;

                        // ★ _tcp 객체의 함수를 호출
                        await _tcp.ConnectAsClientAsync(ip, port, timeoutMs, retrySec);
                        tcpConn.Text = "Disconnect";
                    }
                }
                else
                {
                    _tcp.DisconnectAll(); // ★ _tcp 객체의 함수 호출
                    tcpConn.Text = serBtn.Checked ? "Start Server" : "Connect";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }
        }

        private void serBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (serBtn.Checked)
            {
                ipBox.Enabled = true;
                tcpConn.Text = "Start Server";
            }
        }

        private void cliBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (cliBtn.Checked)
            {
                ipBox.Enabled = true;
                tcpConn.Text = "Connect";
            }
        }
    }
}
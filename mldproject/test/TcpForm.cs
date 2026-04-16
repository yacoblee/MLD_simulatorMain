using System;
using System.Text;
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
                        _ = _tcp.StartServerAsync(ip, port);
                        tcpConn.Text = "Stop Server";
                    }
                    else if (cliBtn.Checked)
                    {
                        int timeoutMs = (int)timeOutUpDown.Value;
                        int retrySec = (int)reConnUpDown.Value;

                        await _tcp.ConnectAsClientAsync(ip, port, timeoutMs, retrySec);
                        tcpConn.Text = "Disconnect";
                    }
                }
                else
                {
                    _tcp.DisconnectAll();
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

        private async void sendBtn_Click(object sender, EventArgs e)
        {
            string msg = sendTxt.Text;
            Encoding encode = Encoding.UTF8;
            await _tcp.SendDataAsync(msg, encode);
            sendTxt.Clear();
        }
    }
}
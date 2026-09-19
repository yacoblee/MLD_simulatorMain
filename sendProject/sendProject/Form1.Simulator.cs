using sendProject.common;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace sendProject
{
    // 아두이노 역할 시뮬레이터.
    // camProject가 시리얼로 보내는 :SWITCH 명령에 아두이노 스케치와 같게 반응한다.
    //   바코드,:SWITCH:ON?   -> LED1 ON, 서보 6초 구동
    //   바코드,:SWITCH2:ON?  -> LED1 OFF, LED2 ON, 서보 6초 구동
    //   바코드,:SWITCH2:OFF  -> LED2 OFF
    // 서보 구동은 1초마다 15도씩 올라가며 각도를 출력하고(15,30,45,60,75,0) 끝나면 완료 보고를 보낸다.
    public partial class Form1
    {
        private Panel pnlSim;
        private Panel pnlLed1;
        private Panel pnlLed2;
        private Label lblAngle;
        private NumericUpDown numSpeed;
        private CheckBox chkMute;
        private CheckBox chkDebug;
        private CheckBox chkDone;

        private readonly System.Windows.Forms.Timer _sweepTimer = new System.Windows.Forms.Timer();
        private readonly object _serialLock = new object();
        private int _angle;

        private volatile bool _mute;
        private volatile bool _debugLines = true;
        private volatile bool _sendDone = true;


        private void BuildSimulatorPanel()
        {
            pnlSim = new Panel { Dock = DockStyle.Bottom, Height = 130 };

          
            FlowLayoutPanel NewRow(int x, int y)
            {
               
                return new FlowLayoutPanel { Location = new Point(x, y), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false };
            }
            Label MakeLabel(string text)
            {
                return new Label { AutoSize = true, Text = text, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 24, 0) };
            }
            CheckBox MakeCheck(string text, bool isChecked)
            {
                return new CheckBox { AutoSize = true, Text = text, Checked = isChecked, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 24, 0) };
            }

            Label title = new Label { AutoSize = true, Location = new Point(15, 8), Text = "아두이노 시뮬레이터 (가상 포트로 camProject와 시리얼 통신)" };

            pnlLed1 = new Panel { Size = new Size(26, 26), BorderStyle = BorderStyle.FixedSingle, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 8, 0) };
            pnlLed2 = new Panel { Size = new Size(26, 26), BorderStyle = BorderStyle.FixedSingle, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 8, 0) };
            lblAngle = MakeLabel("서보 각도: 0°");

            FlowLayoutPanel row1 = NewRow(15, 38);
            row1.Controls.Add(pnlLed1);
            row1.Controls.Add(MakeLabel("LED1 (3번 핀)"));
            row1.Controls.Add(pnlLed2);
            row1.Controls.Add(MakeLabel("LED2 (7번 핀, MES Ready)"));
            row1.Controls.Add(lblAngle);

            numSpeed = new NumericUpDown { Width = 60, Minimum = 1, Maximum = 20, Value = 1, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 24, 0) };
            chkMute = MakeCheck("응답 중지(장애 주입)", false);
            chkDebug = MakeCheck("디버그 줄 출력", true);
            chkDone = MakeCheck("완료 보고(01DONE)", true);

            chkMute.CheckedChanged += delegate { _mute = chkMute.Checked; };
            chkDebug.CheckedChanged += delegate { _debugLines = chkDebug.Checked; };
            chkDone.CheckedChanged += delegate { _sendDone = chkDone.Checked; };

            FlowLayoutPanel row2 = NewRow(15, 84);
            row2.Controls.Add(MakeLabel("배속"));
            row2.Controls.Add(numSpeed);
            row2.Controls.Add(chkMute);
            row2.Controls.Add(chkDebug);
            row2.Controls.Add(chkDone);

            pnlSim.Controls.Add(title);
            pnlSim.Controls.Add(row1);
            pnlSim.Controls.Add(row2);
            Controls.Add(pnlSim);

            _sweepTimer.Tick += SweepTimer_Tick;
        }

  
        private void ResetSimulatorUi()
        {
            SetLed(pnlLed1, false);
            SetLed(pnlLed2, false);
            lblAngle.Text = "서보 각도: 0°";
        }

        private void SetLed(Panel p, bool on)
        {
            p.BackColor = on ? Color.LimeGreen : Theme.BorderColor;
        }

        // 정상 프레임: STX + 본문 + CR LF
        private void SendSerialFrame(string data)
        {
            if (!_serial.IsOpen) return;

            byte[] body = Encoding.ASCII.GetBytes(data);
            byte[] by = new byte[body.Length + 3];
            by[0] = 0x02;
            Array.Copy(body, 0, by, 1, body.Length);
            by[by.Length - 2] = 0x0D;
            by[by.Length - 1] = 0x0A;

            lock (_serialLock) { _serial.Write(by, 0, by.Length); }
        }

        private void SendSerialLine(string line)
        {
            if (!_serial.IsOpen || !_debugLines) return;

            byte[] by = Encoding.ASCII.GetBytes(line + "\r\n");
            lock (_serialLock) { _serial.Write(by, 0, by.Length); }
        }

        private void LogUi(string text)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(() =>
            {
                if (!IsDisposed) txtBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}\r\n");
            }));
        }

        // 시리얼 수신 스레드에서 호출된다. 컨트롤과 타이머는 BeginInvoke로 UI 스레드에서만 만진다
        private void ProcessSwitch(string msg)
        {
            int comma = msg.IndexOf(',');
            if (comma < 0) return;

            string barcode = msg.Substring(0, comma).Trim();
            string cmd = msg.Substring(comma + 1).Trim();

            if (cmd == ":SWITCH:ON?" && barcode.Length > 0)
            {
                BeginInvoke(new Action(() => { SetLed(pnlLed1, true); StartSweep(); }));
                SendSerialLine("System READY: LED ON");
            }
            else if (cmd == ":SWITCH2:ON?")
            {
                BeginInvoke(new Action(() => { SetLed(pnlLed1, false); SetLed(pnlLed2, true); StartSweep(); }));
                SendSerialLine("System MES Ready: LED ON");
            }
            else if (cmd == ":SWITCH2:OFF")
            {
                BeginInvoke(new Action(() => SetLed(pnlLed2, false)));
                SendSerialLine("System ALL OFF ");
            }
            else if (cmd == ":SWITCH:ON?")
            {
                LogUi("바코드가 없어 :SWITCH:ON?를 무시합니다 (아두이노와 동일)");
            }
            else
            {
                LogUi($"알 수 없는 SWITCH 명령: {cmd}");
            }
        }

        // UI 스레드에서만 호출
        private void StartSweep()
        {
            if (_sweepTimer.Enabled) return;  

            _angle = 0;
            _sweepTimer.Interval = Math.Max(30, 1000 / (int)numSpeed.Value);
            _sweepTimer.Start();
            LogUi($"이동 시작 (배속 x{numSpeed.Value})");
        }

        private void SweepTimer_Tick(object sender, EventArgs e)
        {
            _angle += 15;
            bool done = _angle >= 90;
            if (done) _angle = 0;

            lblAngle.Text = $"이동중 : {_angle}°";
            SendSerialLine(_angle.ToString());

            if (!done) return;

            _sweepTimer.Stop();
            LogUi("이동 완료");
            if (_sendDone) SendSerialFrame($"{_addr:00}DONE,OK");
        }
    }
}

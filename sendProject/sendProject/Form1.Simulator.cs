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
    // 2026-10-29 수정: SMEMA 2신호 핸드셰이크 모사. 두 LED가 모두 ON일 때만 이송(서보 구동)한다.
    //   바코드,:SWITCH:ON?   -> LED2 OFF, LED1 ON(Board Available), 이송 없이 준비 완료(01DONE,OK,1) 보고
    //   바코드,:SWITCH2:ON?  -> LED2 ON(Machine Ready), LED1도 켜져 있으면 이송(서보 6초 구동)
    //   바코드,:SWITCH2:OFF  -> 구동 정지, LED 모두 OFF
    // [2026-10-28 버전] :SWITCH:ON? 은 LED1 ON + 서보 구동, :SWITCH2:ON? 은 LED1 OFF + LED2 ON + 서보 구동
    // 서보 구동은 1초마다 15도씩 올라가며 각도를 출력하고(15,30,45,60,75,0) 끝나면 완료 보고(01DONE,OK,2)를 보낸다.
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

        // 2026-10-28 추가(문제2): 현재 구동 단계(1=1차, 2=2차). 완료 보고(01DONE,OK,<단계>)에 실어 camProject가 순서를 맞추게 함
        private int _stage;

        // 2026-10-29 추가: 두 신호(LED) 상태. UI 스레드에서만 읽고 쓴다
        private bool _led1;
        private bool _led2;

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
            lblAngle = MakeLabel("이동 : 0°");

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
            lblAngle.Text = "이동 : 0°";
        }

        private void SetLed(Panel p, bool on)
        {
            p.BackColor = on ? Color.LimeGreen : Theme.BorderColor;
            if (p == pnlLed1) _led1 = on; else _led2 = on;

            // 2026-10-28 추가(문제3 확인용): LED 상태 변화를 로그에 남겨 사이클마다 LED2가 정리되는지 확인할 수 있게 함
            LogUi($"{(p == pnlLed1 ? "LED1" : "LED2")} {(on ? "ON" : "OFF")}");
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
                // [2026-10-28 원본] BeginInvoke(new Action(() => { SetLed(pnlLed1, true); StartSweep(); }));
                // 2026-10-28 수정(문제3): 새 사이클이 시작되면 이전 사이클에서 켜진 LED2를 끄고, 1차 구동을 처음부터 시작
                // [2026-10-28 수정본] BeginInvoke(new Action(() => { SetLed(pnlLed2, false); SetLed(pnlLed1, true); StartSweep(1); }));
                // 2026-10-29 수정: 1차에서는 이송하지 않는다. 진행 중이던 구동을 멈추고 LED1만 켠 뒤 준비 완료(단계 1)를 보고
                BeginInvoke(new Action(() =>
                {
                    _sweepTimer.Stop();
                    _angle = 0;
                    lblAngle.Text = "이동 : 0°";
                    SetLed(pnlLed2, false);
                    SetLed(pnlLed1, true);
                    if (_sendDone) SendSerialFrame($"{_addr:00}DONE,OK,1");
                }));
                SendSerialLine("System READY: LED ON");
            }
            else if (cmd == ":SWITCH2:ON?")
            {
                // [2026-10-28 원본] BeginInvoke(new Action(() => { SetLed(pnlLed1, false); SetLed(pnlLed2, true); StartSweep(); }));
                // 2026-10-28 수정(문제2): 구동 중이어도 처음부터 다시 구동(재동작)하고, 단계는 2로 표시
                // [2026-10-28 수정본] BeginInvoke(new Action(() => { SetLed(pnlLed1, false); SetLed(pnlLed2, true); StartSweep(2); }));
                // 2026-10-29 수정: LED1을 끄지 않는다. LED2를 켠 뒤 두 신호가 모두 ON일 때만 이송
                BeginInvoke(new Action(() => { SetLed(pnlLed2, true); TryTransfer(); }));
                SendSerialLine("System MES Ready: LED ON");
            }
            else if (cmd == ":SWITCH2:OFF")
            {
                // [2026-10-28 원본] BeginInvoke(new Action(() => SetLed(pnlLed2, false)));
                // 2026-10-28 수정(문제4): 사이클 중단 시 camProject가 보내는 정지 명령. 구동을 멈추고 LED를 모두 끔
                BeginInvoke(new Action(() => { _sweepTimer.Stop(); _angle = 0; lblAngle.Text = "이동중 : 0°"; SetLed(pnlLed1, false); SetLed(pnlLed2, false); }));
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

        // [2026-10-28 원본] 구동 중이면 새 명령을 무시해서, 2차 명령이 1차 구동 중에 오면 2차 동작이 따로 보이지 않았음
        // // UI 스레드에서만 호출
        // private void StartSweep()
        // {
        //     if (_sweepTimer.Enabled) return;
        //
        //     _angle = 0;
        //     _sweepTimer.Interval = Math.Max(30, 1000 / (int)numSpeed.Value);
        //     _sweepTimer.Start();
        //     LogUi($"이동 시작 (배속 x{numSpeed.Value})");
        // }

        // 2026-10-29 추가: 두 신호가 모두 ON일 때만 이송(서보 구동). UI 스레드에서만 호출
        private void TryTransfer()
        {
            if (_led1 && _led2) StartSweep(2);
            else LogUi("두 신호(LED1, LED2)가 모두 ON이 아니라 이송 대기");
        }

        // 2026-10-28 수정(문제2): 구동 중이어도 중단하고 처음부터 다시 시작. stage는 완료 보고에 실림. UI 스레드에서만 호출
        private void StartSweep(int stage)
        {
            _sweepTimer.Stop();
            _stage = stage;
            _angle = 0;
            lblAngle.Text = "이동중 : 0°";
            _sweepTimer.Interval = Math.Max(30, 1000 / (int)numSpeed.Value);
            _sweepTimer.Start();
            LogUi($"이송 시작 - LED1+LED2 ON (배속 x{numSpeed.Value})"); // 2026-10-29 수정: "{stage}차 이동 시작" 에서 변경
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
            LogUi("이송 완료"); // 2026-10-29 수정: "{_stage}차 이동 완료" 에서 변경

            // 2026-10-29 추가: 이송이 끝나면 두 신호를 내린다 (SMEMA: 기판 전달 후 신호 해제)
            SetLed(pnlLed1, false);
            SetLed(pnlLed2, false);

            // [2026-10-28 원본] if (_sendDone) SendSerialFrame($"{_addr:00}DONE,OK");
            // 2026-10-28 수정(문제2): 어느 단계가 끝났는지 알 수 있게 단계 번호를 포함
            if (_sendDone) SendSerialFrame($"{_addr:00}DONE,OK,{_stage}");
        }
    }
}

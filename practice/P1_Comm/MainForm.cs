using System;
using System.Windows.Forms;

namespace P1_Comm
{
    // 연습용 빈 폼. DRILLS.md 의 과제를 보고, 이 파일과 새 클래스를 직접 작성한다.
    // Designer 없이 코드로 컨트롤을 만든다 (sendProject 의 Form1.Simulator.cs 방식).
    public class MainForm : Form
    {
        public Label label1;
        public TextBox textBox1;
        public Button button1;
        public Button button2;
        private bool _act = true;

        public CounterWorker _counterWorker;

        public MainForm()
        {

            _counterWorker = new CounterWorker();

            InitializeComponent();
            _counterWorker.LogOccurred += Write_LogOccured;
            _counterWorker.WatchdogTimeout += Worker_WatchdogTimeout;

        }

        private void InitializeComponent()
        {
            Text = "P1_Comm";
            Width = 900;
            Height = 600;

            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(247, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "실행 로그";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(251, 152);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(290, 156);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(584, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "시작";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(586, 262);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 46);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(965, 541);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LogUi(string text)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(() =>
            {
                if (!IsDisposed) textBox1.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}\r\n");
            }));
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            LogUi(textBox1.Text);
            if (_act == true)
            {
                button1.Text = "실행";
                _counterWorker.Start();

            } else {
                button1.Text = "정지";
                _counterWorker.Stop();
            }

        }

        private void Write_LogOccured(object sender, string message)
        {
            LogUi(message);
        }

        private void Worker_WatchdogTimeout(object sender, EventArgs s)
        {
            LogUi($"[{DateTime.Now:HH:mm:ss}] [ALERT] 장비 응답 지연 감지!\r\n");

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_counterWorker != null)
            {
                _counterWorker.LogOccurred -= Write_LogOccured;
                _counterWorker.WatchdogTimeout -= Worker_WatchdogTimeout;
                _counterWorker.Dispose();
                _counterWorker = null;

            }
        }

    }
}

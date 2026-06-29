namespace copyApp
{
    partial class MainView
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbSpeed = new System.Windows.Forms.ComboBox();
            this.cmbBit = new System.Windows.Forms.ComboBox();
            this.cmbLength = new System.Windows.Forms.ComboBox();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.textLog = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.connBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStopBit = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.tcpSend = new System.Windows.Forms.Button();
            this.serialBtn = new System.Windows.Forms.RadioButton();
            this.tcpBtn = new System.Windows.Forms.RadioButton();
            this.serialPanel = new System.Windows.Forms.Panel();
            this.tcpPanel = new System.Windows.Forms.Panel();
            this.barBtn = new System.Windows.Forms.Button();
            this.pasiveRadio = new System.Windows.Forms.RadioButton();
            this.activeRadio = new System.Windows.Forms.RadioButton();
            this.retryTxt = new System.Windows.Forms.TextBox();
            this.timeTxt = new System.Windows.Forms.TextBox();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.barTxt = new System.Windows.Forms.TextBox();
            this.ipTxt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tcpConBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.serialPanel.SuspendLayout();
            this.tcpPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.FormattingEnabled = true;
            this.cmbSpeed.Location = new System.Drawing.Point(22, 123);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(180, 29);
            this.cmbSpeed.TabIndex = 0;
            // 
            // cmbBit
            // 
            this.cmbBit.FormattingEnabled = true;
            this.cmbBit.Location = new System.Drawing.Point(22, 205);
            this.cmbBit.Name = "cmbBit";
            this.cmbBit.Size = new System.Drawing.Size(180, 29);
            this.cmbBit.TabIndex = 1;
            // 
            // cmbLength
            // 
            this.cmbLength.FormattingEnabled = true;
            this.cmbLength.Location = new System.Drawing.Point(22, 274);
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(180, 29);
            this.cmbLength.TabIndex = 2;
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(22, 47);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(180, 29);
            this.cmbPortName.TabIndex = 3;
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(473, 117);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(967, 604);
            this.textLog.TabIndex = 4;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(6, 477);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(100, 58);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "저장";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // connBtn
            // 
            this.connBtn.Location = new System.Drawing.Point(123, 477);
            this.connBtn.Name = "connBtn";
            this.connBtn.Size = new System.Drawing.Size(100, 58);
            this.connBtn.TabIndex = 5;
            this.connBtn.Text = "연결";
            this.connBtn.UseVisualStyleBackColor = true;
            this.connBtn.Click += new System.EventHandler(this.connBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "포트";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "통신속도";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "페라티비트";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 21);
            this.label4.TabIndex = 6;
            this.label4.Text = "데이터길이";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 21);
            this.label5.TabIndex = 6;
            this.label5.Text = "정지비트";
            // 
            // cmbStopBit
            // 
            this.cmbStopBit.FormattingEnabled = true;
            this.cmbStopBit.Location = new System.Drawing.Point(22, 347);
            this.cmbStopBit.Name = "cmbStopBit";
            this.cmbStopBit.Size = new System.Drawing.Size(180, 29);
            this.cmbStopBit.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 391);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 21);
            this.label6.TabIndex = 8;
            this.label6.Text = "프로토콜";
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Location = new System.Drawing.Point(22, 415);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(180, 29);
            this.cmbProtocol.TabIndex = 7;
            // 
            // tcpSend
            // 
            this.tcpSend.Location = new System.Drawing.Point(129, 786);
            this.tcpSend.Name = "tcpSend";
            this.tcpSend.Size = new System.Drawing.Size(100, 56);
            this.tcpSend.TabIndex = 10;
            this.tcpSend.Text = "전송";
            this.tcpSend.UseVisualStyleBackColor = true;
            // 
            // serialBtn
            // 
            this.serialBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.serialBtn.Location = new System.Drawing.Point(159, 46);
            this.serialBtn.Name = "serialBtn";
            this.serialBtn.Size = new System.Drawing.Size(80, 25);
            this.serialBtn.TabIndex = 9;
            this.serialBtn.Text = "Serial";
            this.serialBtn.UseVisualStyleBackColor = true;
            // 
            // tcpBtn
            // 
            this.tcpBtn.AutoSize = true;
            this.tcpBtn.Checked = true;
            this.tcpBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tcpBtn.Location = new System.Drawing.Point(73, 46);
            this.tcpBtn.Name = "tcpBtn";
            this.tcpBtn.Size = new System.Drawing.Size(69, 25);
            this.tcpBtn.TabIndex = 9;
            this.tcpBtn.TabStop = true;
            this.tcpBtn.Text = "Tcp";
            this.tcpBtn.UseVisualStyleBackColor = true;
            // 
            // serialPanel
            // 
            this.serialPanel.Controls.Add(this.cmbStopBit);
            this.serialPanel.Controls.Add(this.cmbSpeed);
            this.serialPanel.Controls.Add(this.cmbBit);
            this.serialPanel.Controls.Add(this.cmbLength);
            this.serialPanel.Controls.Add(this.label6);
            this.serialPanel.Controls.Add(this.connBtn);
            this.serialPanel.Controls.Add(this.cmbPortName);
            this.serialPanel.Controls.Add(this.cmbProtocol);
            this.serialPanel.Controls.Add(this.saveBtn);
            this.serialPanel.Controls.Add(this.label1);
            this.serialPanel.Controls.Add(this.label5);
            this.serialPanel.Controls.Add(this.label2);
            this.serialPanel.Controls.Add(this.label4);
            this.serialPanel.Controls.Add(this.label3);
            this.serialPanel.Location = new System.Drawing.Point(36, 97);
            this.serialPanel.Name = "serialPanel";
            this.serialPanel.Size = new System.Drawing.Size(241, 560);
            this.serialPanel.TabIndex = 11;
            // 
            // tcpPanel
            // 
            this.tcpPanel.Controls.Add(this.barBtn);
            this.tcpPanel.Controls.Add(this.pasiveRadio);
            this.tcpPanel.Controls.Add(this.activeRadio);
            this.tcpPanel.Controls.Add(this.retryTxt);
            this.tcpPanel.Controls.Add(this.timeTxt);
            this.tcpPanel.Controls.Add(this.portTxt);
            this.tcpPanel.Controls.Add(this.barTxt);
            this.tcpPanel.Controls.Add(this.ipTxt);
            this.tcpPanel.Controls.Add(this.tcpSend);
            this.tcpPanel.Controls.Add(this.label11);
            this.tcpPanel.Controls.Add(this.label8);
            this.tcpPanel.Controls.Add(this.tcpConBtn);
            this.tcpPanel.Controls.Add(this.label9);
            this.tcpPanel.Controls.Add(this.label7);
            this.tcpPanel.Controls.Add(this.label10);
            this.tcpPanel.Location = new System.Drawing.Point(33, 77);
            this.tcpPanel.Name = "tcpPanel";
            this.tcpPanel.Size = new System.Drawing.Size(241, 870);
            this.tcpPanel.TabIndex = 12;
            // 
            // barBtn
            // 
            this.barBtn.Location = new System.Drawing.Point(146, 113);
            this.barBtn.Name = "barBtn";
            this.barBtn.Size = new System.Drawing.Size(74, 34);
            this.barBtn.TabIndex = 13;
            this.barBtn.Text = "입력";
            this.barBtn.UseVisualStyleBackColor = true;
            this.barBtn.Click += new System.EventHandler(this.barBtn_Click);
            // 
            // pasiveRadio
            // 
            this.pasiveRadio.AutoSize = true;
            this.pasiveRadio.Checked = true;
            this.pasiveRadio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pasiveRadio.Location = new System.Drawing.Point(115, 702);
            this.pasiveRadio.Name = "pasiveRadio";
            this.pasiveRadio.Size = new System.Drawing.Size(100, 25);
            this.pasiveRadio.TabIndex = 12;
            this.pasiveRadio.TabStop = true;
            this.pasiveRadio.Text = "passive";
            this.pasiveRadio.UseVisualStyleBackColor = true;
            // 
            // activeRadio
            // 
            this.activeRadio.AutoSize = true;
            this.activeRadio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.activeRadio.Location = new System.Drawing.Point(12, 702);
            this.activeRadio.Name = "activeRadio";
            this.activeRadio.Size = new System.Drawing.Size(86, 25);
            this.activeRadio.TabIndex = 12;
            this.activeRadio.Text = "active";
            this.activeRadio.UseVisualStyleBackColor = true;
            // 
            // retryTxt
            // 
            this.retryTxt.Location = new System.Drawing.Point(19, 485);
            this.retryTxt.Name = "retryTxt";
            this.retryTxt.Size = new System.Drawing.Size(201, 32);
            this.retryTxt.TabIndex = 11;
            // 
            // timeTxt
            // 
            this.timeTxt.Location = new System.Drawing.Point(19, 381);
            this.timeTxt.Name = "timeTxt";
            this.timeTxt.Size = new System.Drawing.Size(201, 32);
            this.timeTxt.TabIndex = 11;
            // 
            // portTxt
            // 
            this.portTxt.Location = new System.Drawing.Point(19, 286);
            this.portTxt.Name = "portTxt";
            this.portTxt.Size = new System.Drawing.Size(201, 32);
            this.portTxt.TabIndex = 11;
            // 
            // barTxt
            // 
            this.barTxt.Location = new System.Drawing.Point(19, 75);
            this.barTxt.Name = "barTxt";
            this.barTxt.ReadOnly = true;
            this.barTxt.Size = new System.Drawing.Size(201, 32);
            this.barTxt.TabIndex = 11;
            // 
            // ipTxt
            // 
            this.ipTxt.Location = new System.Drawing.Point(19, 192);
            this.ipTxt.Name = "ipTxt";
            this.ipTxt.Size = new System.Drawing.Size(201, 32);
            this.ipTxt.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 448);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 21);
            this.label11.TabIndex = 6;
            this.label11.Text = "Retry Count";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 343);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 21);
            this.label8.TabIndex = 6;
            this.label8.Text = "Time Out";
            // 
            // tcpConBtn
            // 
            this.tcpConBtn.Location = new System.Drawing.Point(12, 785);
            this.tcpConBtn.Name = "tcpConBtn";
            this.tcpConBtn.Size = new System.Drawing.Size(100, 58);
            this.tcpConBtn.TabIndex = 5;
            this.tcpConBtn.Text = "연결";
            this.tcpConBtn.UseVisualStyleBackColor = true;
            this.tcpConBtn.Click += new System.EventHandler(this.tcpConnBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 248);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 21);
            this.label9.TabIndex = 6;
            this.label9.Text = "포트";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 21);
            this.label7.TabIndex = 6;
            this.label7.Text = "IP";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 21);
            this.label10.TabIndex = 6;
            this.label10.Text = "바코드";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1774, 1023);
            this.Controls.Add(this.tcpPanel);
            this.Controls.Add(this.serialPanel);
            this.Controls.Add(this.tcpBtn);
            this.Controls.Add(this.serialBtn);
            this.Controls.Add(this.textLog);
            this.Name = "MainView";
            this.Text = "Form1";
            this.serialPanel.ResumeLayout(false);
            this.serialPanel.PerformLayout();
            this.tcpPanel.ResumeLayout(false);
            this.tcpPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSpeed;
        private System.Windows.Forms.ComboBox cmbBit;
        private System.Windows.Forms.ComboBox cmbLength;
        private System.Windows.Forms.ComboBox cmbPortName;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button connBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStopBit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.Button tcpSend;
        private System.Windows.Forms.RadioButton serialBtn;
        private System.Windows.Forms.RadioButton tcpBtn;
        private System.Windows.Forms.Panel serialPanel;
        private System.Windows.Forms.Panel tcpPanel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button tcpConBtn;
        private System.Windows.Forms.TextBox portTxt;
        private System.Windows.Forms.TextBox ipTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton pasiveRadio;
        private System.Windows.Forms.RadioButton activeRadio;
        private System.Windows.Forms.TextBox timeTxt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox retryTxt;
        private System.Windows.Forms.Button barBtn;
        private System.Windows.Forms.TextBox barTxt;
    }
}


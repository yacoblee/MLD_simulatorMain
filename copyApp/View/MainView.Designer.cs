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
            this.SuspendLayout();
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.FormattingEnabled = true;
            this.cmbSpeed.Location = new System.Drawing.Point(965, 161);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(180, 29);
            this.cmbSpeed.TabIndex = 0;
            // 
            // cmbBit
            // 
            this.cmbBit.FormattingEnabled = true;
            this.cmbBit.Location = new System.Drawing.Point(965, 243);
            this.cmbBit.Name = "cmbBit";
            this.cmbBit.Size = new System.Drawing.Size(180, 29);
            this.cmbBit.TabIndex = 1;
            // 
            // cmbLength
            // 
            this.cmbLength.FormattingEnabled = true;
            this.cmbLength.Location = new System.Drawing.Point(965, 312);
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(180, 29);
            this.cmbLength.TabIndex = 2;
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(965, 85);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(180, 29);
            this.cmbPortName.TabIndex = 3;
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(85, 65);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(709, 411);
            this.textLog.TabIndex = 4;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(955, 553);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(100, 58);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "저장";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // connBtn
            // 
            this.connBtn.Location = new System.Drawing.Point(1072, 553);
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
            this.label1.Location = new System.Drawing.Point(961, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "포트";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(961, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "통신속도";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(961, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "페라티비트";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(961, 288);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 21);
            this.label4.TabIndex = 6;
            this.label4.Text = "데이터길이";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(961, 361);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 21);
            this.label5.TabIndex = 6;
            this.label5.Text = "정지비트";
            // 
            // cmbStopBit
            // 
            this.cmbStopBit.FormattingEnabled = true;
            this.cmbStopBit.Location = new System.Drawing.Point(965, 385);
            this.cmbStopBit.Name = "cmbStopBit";
            this.cmbStopBit.Size = new System.Drawing.Size(180, 29);
            this.cmbStopBit.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(961, 444);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 21);
            this.label6.TabIndex = 8;
            this.label6.Text = "프로토콜";
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Location = new System.Drawing.Point(965, 468);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(180, 29);
            this.cmbProtocol.TabIndex = 7;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 783);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbProtocol);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.cmbPortName);
            this.Controls.Add(this.cmbStopBit);
            this.Controls.Add(this.cmbLength);
            this.Controls.Add(this.cmbBit);
            this.Controls.Add(this.cmbSpeed);
            this.Name = "MainView";
            this.Text = "Form1";
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
    }
}


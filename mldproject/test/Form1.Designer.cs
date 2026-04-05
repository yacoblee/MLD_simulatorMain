namespace test
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonWho = new System.Windows.Forms.Button();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.TxtLog = new System.Windows.Forms.TextBox();
            this.winPop = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btTimer = new System.Windows.Forms.Button();
            this.cmbSpeed = new System.Windows.Forms.ComboBox();
            this.cmbBit = new System.Windows.Forms.ComboBox();
            this.cmbLength = new System.Windows.Forms.ComboBox();
            this.cmbStopBit = new System.Windows.Forms.ComboBox();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbNet = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tcpConn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonWho
            // 
            this.buttonWho.Location = new System.Drawing.Point(1463, 166);
            this.buttonWho.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonWho.Name = "buttonWho";
            this.buttonWho.Size = new System.Drawing.Size(157, 57);
            this.buttonWho.TabIndex = 0;
            this.buttonWho.Text = "who";
            this.buttonWho.UseVisualStyleBackColor = true;
            this.buttonWho.Click += new System.EventHandler(this.Buttonwho_Click);
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(52, 266);
            this.cmbPortName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(155, 29);
            this.cmbPortName.TabIndex = 2;
            this.cmbPortName.DropDown += new System.EventHandler(this.cmbPortName_DropDown);
            this.cmbPortName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPortName_KeyDown);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(1283, 238);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(158, 57);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // TxtLog
            // 
            this.TxtLog.Location = new System.Drawing.Point(34, 15);
            this.TxtLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtLog.Size = new System.Drawing.Size(1221, 189);
            this.TxtLog.TabIndex = 4;
            this.TxtLog.TextChanged += new System.EventHandler(this.TxtLog_TextChanged);
            // 
            // winPop
            // 
            this.winPop.Location = new System.Drawing.Point(1284, 166);
            this.winPop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.winPop.Name = "winPop";
            this.winPop.Size = new System.Drawing.Size(157, 57);
            this.winPop.TabIndex = 102;
            this.winPop.Text = "Setting";
            this.winPop.UseVisualStyleBackColor = true;
            this.winPop.Click += new System.EventHandler(this.Popup_Click);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea9.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea9.AxisX.MajorTickMark.Enabled = false;
            chartArea9.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea9.AxisY.Interval = 10D;
            chartArea9.AxisY.Maximum = 100D;
            chartArea9.AxisY.Minimum = 0D;
            chartArea9.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea9.Name = "ChartArea1";
            chartArea9.Position.Auto = false;
            chartArea9.Position.Height = 50F;
            chartArea9.Position.Width = 50F;
            chartArea10.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea10.AxisX.MinorGrid.Enabled = true;
            chartArea10.AxisX.MinorTickMark.Enabled = true;
            chartArea10.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea10.AxisY.Interval = 10D;
            chartArea10.AxisY.LabelStyle.Format = "0000.00";
            chartArea10.AxisY.LabelStyle.Interval = 0D;
            chartArea10.AxisY.Maximum = 100D;
            chartArea10.AxisY.Minimum = 0D;
            chartArea10.Name = "ChartArea2";
            chartArea10.Position.Auto = false;
            chartArea10.Position.Height = 50F;
            chartArea10.Position.Width = 50F;
            chartArea10.Position.X = 50F;
            chartArea11.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea11.AxisX.MinorGrid.Enabled = true;
            chartArea11.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea11.AxisY.Interval = 10D;
            chartArea11.AxisY.Maximum = 100D;
            chartArea11.AxisY.Minimum = 0D;
            chartArea11.Name = "ChartArea3";
            chartArea11.Position.Auto = false;
            chartArea11.Position.Height = 50F;
            chartArea11.Position.Width = 50F;
            chartArea11.Position.Y = 50F;
            chartArea12.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea12.AxisX.MinorGrid.Enabled = true;
            chartArea12.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea12.AxisY.Interval = 10D;
            chartArea12.AxisY.LabelStyle.Format = "0000.00";
            chartArea12.AxisY.LabelStyle.Interval = 10D;
            chartArea12.AxisY.MajorTickMark.Enabled = false;
            chartArea12.AxisY.Maximum = 100D;
            chartArea12.AxisY.Minimum = 0D;
            chartArea12.Name = "ChartArea4";
            chartArea12.Position.Auto = false;
            chartArea12.Position.Height = 50F;
            chartArea12.Position.Width = 50F;
            chartArea12.Position.X = 50F;
            chartArea12.Position.Y = 50F;
            this.chart1.ChartAreas.Add(chartArea9);
            this.chart1.ChartAreas.Add(chartArea10);
            this.chart1.ChartAreas.Add(chartArea11);
            this.chart1.ChartAreas.Add(chartArea12);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(52, 325);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1552, 613);
            this.chart1.TabIndex = 103;
            this.chart1.Text = "chart1";
            // 
            // btTimer
            // 
            this.btTimer.Location = new System.Drawing.Point(1463, 238);
            this.btTimer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btTimer.Name = "btTimer";
            this.btTimer.Size = new System.Drawing.Size(191, 57);
            this.btTimer.TabIndex = 104;
            this.btTimer.Text = "그래프 통신 시작";
            this.btTimer.UseVisualStyleBackColor = true;
            this.btTimer.Click += new System.EventHandler(this.btTimer_Click);
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.FormattingEnabled = true;
            this.cmbSpeed.Location = new System.Drawing.Point(253, 266);
            this.cmbSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(155, 29);
            this.cmbSpeed.TabIndex = 105;
            // 
            // cmbBit
            // 
            this.cmbBit.FormattingEnabled = true;
            this.cmbBit.Location = new System.Drawing.Point(444, 266);
            this.cmbBit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbBit.Name = "cmbBit";
            this.cmbBit.Size = new System.Drawing.Size(155, 29);
            this.cmbBit.TabIndex = 106;
            // 
            // cmbLength
            // 
            this.cmbLength.FormattingEnabled = true;
            this.cmbLength.Location = new System.Drawing.Point(632, 266);
            this.cmbLength.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbLength.Name = "cmbLength";
            this.cmbLength.Size = new System.Drawing.Size(155, 29);
            this.cmbLength.TabIndex = 107;
            // 
            // cmbStopBit
            // 
            this.cmbStopBit.FormattingEnabled = true;
            this.cmbStopBit.Location = new System.Drawing.Point(822, 266);
            this.cmbStopBit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbStopBit.Name = "cmbStopBit";
            this.cmbStopBit.Size = new System.Drawing.Size(155, 29);
            this.cmbStopBit.TabIndex = 107;
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Location = new System.Drawing.Point(1009, 266);
            this.cmbProtocol.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(220, 29);
            this.cmbProtocol.TabIndex = 107;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(79, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 108;
            this.label1.Text = "포트";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(275, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 108;
            this.label2.Text = "통신속도";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(459, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 23);
            this.label3.TabIndex = 108;
            this.label3.Text = "패리티비트";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(645, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 25);
            this.label4.TabIndex = 108;
            this.label4.Text = "데이터길이";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(844, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 108;
            this.label5.Text = "정지비트";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(1059, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 108;
            this.label6.Text = "프로토콜";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbNet
            // 
            this.cmbNet.FormattingEnabled = true;
            this.cmbNet.Location = new System.Drawing.Point(1283, 52);
            this.cmbNet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbNet.Name = "cmbNet";
            this.cmbNet.Size = new System.Drawing.Size(155, 29);
            this.cmbNet.TabIndex = 107;
            this.cmbNet.SelectedIndexChanged += new System.EventHandler(this.cmbNet_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(1299, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 23);
            this.label7.TabIndex = 108;
            this.label7.Text = "회사";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tcpConn
            // 
            this.tcpConn.Location = new System.Drawing.Point(1463, 37);
            this.tcpConn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tcpConn.Name = "tcpConn";
            this.tcpConn.Size = new System.Drawing.Size(158, 57);
            this.tcpConn.TabIndex = 3;
            this.tcpConn.Text = "TCP Connect";
            this.tcpConn.UseVisualStyleBackColor = true;
            this.tcpConn.Click += new System.EventHandler(this.tcpConn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1697, 1039);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbNet);
            this.Controls.Add(this.cmbProtocol);
            this.Controls.Add(this.cmbStopBit);
            this.Controls.Add(this.cmbLength);
            this.Controls.Add(this.cmbBit);
            this.Controls.Add(this.cmbSpeed);
            this.Controls.Add(this.btTimer);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.winPop);
            this.Controls.Add(this.TxtLog);
            this.Controls.Add(this.tcpConn);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.cmbPortName);
            this.Controls.Add(this.buttonWho);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonWho;
        private System.Windows.Forms.ComboBox cmbPortName;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox TxtLog;
        private System.Windows.Forms.Button winPop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btTimer;
        private System.Windows.Forms.ComboBox cmbSpeed;
        private System.Windows.Forms.ComboBox cmbBit;
        private System.Windows.Forms.ComboBox cmbLength;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbNet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStopBit;
        private System.Windows.Forms.Button tcpConn;
    }
}


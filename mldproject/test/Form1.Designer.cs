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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonWho = new System.Windows.Forms.Button();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.TxtLog = new System.Windows.Forms.TextBox();
            this.winPop = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btTimer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonWho
            // 
            this.buttonWho.Location = new System.Drawing.Point(804, 77);
            this.buttonWho.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWho.Name = "buttonWho";
            this.buttonWho.Size = new System.Drawing.Size(100, 33);
            this.buttonWho.TabIndex = 0;
            this.buttonWho.Text = "who";
            this.buttonWho.UseVisualStyleBackColor = true;
            this.buttonWho.Click += new System.EventHandler(this.Buttonwho_Click);
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(804, 49);
            this.cmbPortName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(100, 20);
            this.cmbPortName.TabIndex = 2;
            this.cmbPortName.Text = "COM1";
            this.cmbPortName.DropDown += new System.EventHandler(this.cmbPortName_DropDown);
            this.cmbPortName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPortName_KeyDown);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(930, 41);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(101, 33);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // TxtLog
            // 
            this.TxtLog.Location = new System.Drawing.Point(22, 9);
            this.TxtLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtLog.Size = new System.Drawing.Size(778, 110);
            this.TxtLog.TabIndex = 4;
            this.TxtLog.TextChanged += new System.EventHandler(this.TxtLog_TextChanged);
            // 
            // winPop
            // 
            this.winPop.Location = new System.Drawing.Point(931, 77);
            this.winPop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.winPop.Name = "winPop";
            this.winPop.Size = new System.Drawing.Size(100, 56);
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
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.Interval = 10D;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 50F;
            chartArea1.Position.Width = 50F;
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorTickMark.Enabled = true;
            chartArea2.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisY.Interval = 10D;
            chartArea2.AxisY.LabelStyle.Format = "0000.00";
            chartArea2.AxisY.LabelStyle.Interval = 0D;
            chartArea2.AxisY.Maximum = 100D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartArea2";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 50F;
            chartArea2.Position.Width = 50F;
            chartArea2.Position.X = 50F;
            chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisX.MinorGrid.Enabled = true;
            chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisY.Interval = 10D;
            chartArea3.AxisY.Maximum = 100D;
            chartArea3.AxisY.Minimum = 0D;
            chartArea3.Name = "ChartArea3";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 50F;
            chartArea3.Position.Width = 50F;
            chartArea3.Position.Y = 50F;
            chartArea4.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea4.AxisX.MinorGrid.Enabled = true;
            chartArea4.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea4.AxisY.Interval = 10D;
            chartArea4.AxisY.LabelStyle.Format = "0000.00";
            chartArea4.AxisY.LabelStyle.Interval = 10D;
            chartArea4.AxisY.MajorTickMark.Enabled = false;
            chartArea4.AxisY.Maximum = 100D;
            chartArea4.AxisY.Minimum = 0D;
            chartArea4.Name = "ChartArea4";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 50F;
            chartArea4.Position.Width = 50F;
            chartArea4.Position.X = 50F;
            chartArea4.Position.Y = 50F;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.ChartAreas.Add(chartArea4);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(33, 186);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(987, 493);
            this.chart1.TabIndex = 103;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // btTimer
            // 
            this.btTimer.Location = new System.Drawing.Point(33, 136);
            this.btTimer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTimer.Name = "btTimer";
            this.btTimer.Size = new System.Drawing.Size(122, 33);
            this.btTimer.TabIndex = 104;
            this.btTimer.Text = "그래프 통신 시작";
            this.btTimer.UseVisualStyleBackColor = true;
            this.btTimer.Click += new System.EventHandler(this.btTimer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 710);
            this.Controls.Add(this.btTimer);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.winPop);
            this.Controls.Add(this.TxtLog);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.cmbPortName);
            this.Controls.Add(this.buttonWho);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
    }
}


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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.buttonWho.Location = new System.Drawing.Point(919, 96);
            this.buttonWho.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWho.Name = "buttonWho";
            this.buttonWho.Size = new System.Drawing.Size(114, 41);
            this.buttonWho.TabIndex = 0;
            this.buttonWho.Text = "who";
            this.buttonWho.UseVisualStyleBackColor = true;
            this.buttonWho.Click += new System.EventHandler(this.Buttonwho_Click);
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(919, 61);
            this.cmbPortName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(114, 23);
            this.cmbPortName.TabIndex = 2;
            this.cmbPortName.DropDown += new System.EventHandler(this.cmbPortName_DropDown);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(1063, 51);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(115, 41);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // TxtLog
            // 
            this.TxtLog.Location = new System.Drawing.Point(25, 11);
            this.TxtLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtLog.Multiline = true;
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtLog.Size = new System.Drawing.Size(888, 137);
            this.TxtLog.TabIndex = 4;
            this.TxtLog.TextChanged += new System.EventHandler(this.TxtLog_TextChanged);
            // 
            // winPop
            // 
            this.winPop.Location = new System.Drawing.Point(1064, 96);
            this.winPop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.winPop.Name = "winPop";
            this.winPop.Size = new System.Drawing.Size(114, 70);
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
            chartArea5.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea5.AxisX.MajorTickMark.Enabled = false;
            chartArea5.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea5.AxisY.Interval = 10D;
            chartArea5.AxisY.Maximum = 100D;
            chartArea5.AxisY.Minimum = 0D;
            chartArea5.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.Name = "ChartArea1";
            chartArea5.Position.Auto = false;
            chartArea5.Position.Height = 50F;
            chartArea5.Position.Width = 50F;
            chartArea6.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea6.AxisX.MinorGrid.Enabled = true;
            chartArea6.AxisX.MinorTickMark.Enabled = true;
            chartArea6.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea6.AxisY.Interval = 10D;
            chartArea6.AxisY.LabelStyle.Format = "0000.00";
            chartArea6.AxisY.LabelStyle.Interval = 0D;
            chartArea6.AxisY.Maximum = 100D;
            chartArea6.AxisY.Minimum = 0D;
            chartArea6.Name = "ChartArea2";
            chartArea6.Position.Auto = false;
            chartArea6.Position.Height = 50F;
            chartArea6.Position.Width = 50F;
            chartArea6.Position.X = 50F;
            chartArea7.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea7.AxisX.MinorGrid.Enabled = true;
            chartArea7.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea7.AxisY.Interval = 10D;
            chartArea7.AxisY.Maximum = 100D;
            chartArea7.AxisY.Minimum = 0D;
            chartArea7.Name = "ChartArea3";
            chartArea7.Position.Auto = false;
            chartArea7.Position.Height = 50F;
            chartArea7.Position.Width = 50F;
            chartArea7.Position.Y = 50F;
            chartArea8.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea8.AxisX.MinorGrid.Enabled = true;
            chartArea8.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea8.AxisY.Interval = 10D;
            chartArea8.AxisY.LabelStyle.Format = "0000.00";
            chartArea8.AxisY.LabelStyle.Interval = 10D;
            chartArea8.AxisY.MajorTickMark.Enabled = false;
            chartArea8.AxisY.Maximum = 100D;
            chartArea8.AxisY.Minimum = 0D;
            chartArea8.Name = "ChartArea4";
            chartArea8.Position.Auto = false;
            chartArea8.Position.Height = 50F;
            chartArea8.Position.Width = 50F;
            chartArea8.Position.X = 50F;
            chartArea8.Position.Y = 50F;
            this.chart1.ChartAreas.Add(chartArea5);
            this.chart1.ChartAreas.Add(chartArea6);
            this.chart1.ChartAreas.Add(chartArea7);
            this.chart1.ChartAreas.Add(chartArea8);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(38, 232);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1128, 616);
            this.chart1.TabIndex = 103;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // btTimer
            // 
            this.btTimer.Location = new System.Drawing.Point(38, 170);
            this.btTimer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTimer.Name = "btTimer";
            this.btTimer.Size = new System.Drawing.Size(139, 41);
            this.btTimer.TabIndex = 104;
            this.btTimer.Text = "그래프 통신 시작";
            this.btTimer.UseVisualStyleBackColor = true;
            this.btTimer.Click += new System.EventHandler(this.btTimer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 888);
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


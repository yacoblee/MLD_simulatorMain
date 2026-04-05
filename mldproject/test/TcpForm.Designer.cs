namespace test
{
    partial class TcpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tcpConn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.serBtn = new System.Windows.Forms.RadioButton();
            this.cliBtn = new System.Windows.Forms.RadioButton();
            this.logTxt = new System.Windows.Forms.TextBox();
            this.sendTxt = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.timeOutUpDown = new System.Windows.Forms.NumericUpDown();
            this.reConnUpDown = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeOutUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reConnUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.BurlyWood;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.85052F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.14948F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.portBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ipBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.reConnUpDown, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.timeOutUpDown, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 37);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(776, 263);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tcpConn
            // 
            this.tcpConn.Font = new System.Drawing.Font("굴림", 10F);
            this.tcpConn.Location = new System.Drawing.Point(442, 24);
            this.tcpConn.Name = "tcpConn";
            this.tcpConn.Size = new System.Drawing.Size(178, 78);
            this.tcpConn.TabIndex = 1;
            this.tcpConn.Text = "Connect";
            this.tcpConn.UseVisualStyleBackColor = true;
            this.tcpConn.Click += new System.EventHandler(this.tcpConn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("굴림", 10F);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("굴림", 10F);
            this.label2.Location = new System.Drawing.Point(3, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(311, 65);
            this.label2.TabIndex = 0;
            this.label2.Text = "Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("굴림", 10F);
            this.label3.Location = new System.Drawing.Point(3, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(311, 65);
            this.label3.TabIndex = 0;
            this.label3.Text = "Time Out(ms)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("굴림", 10F);
            this.label4.Location = new System.Drawing.Point(3, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(311, 68);
            this.label4.TabIndex = 0;
            this.label4.Text = "Reconn Interval(sec)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(320, 3);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(453, 32);
            this.ipBox.TabIndex = 1;
            // 
            // portBox
            // 
            this.portBox.Font = new System.Drawing.Font("굴림", 10F);
            this.portBox.Location = new System.Drawing.Point(320, 68);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(453, 34);
            this.portBox.TabIndex = 1;
            // 
            // sendBtn
            // 
            this.sendBtn.Font = new System.Drawing.Font("굴림", 10F);
            this.sendBtn.Location = new System.Drawing.Point(442, 23);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(178, 78);
            this.sendBtn.TabIndex = 1;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.tcpConn_Click);
            // 
            // serBtn
            // 
            this.serBtn.AutoSize = true;
            this.serBtn.Font = new System.Drawing.Font("굴림", 10F);
            this.serBtn.Location = new System.Drawing.Point(55, 52);
            this.serBtn.Name = "serBtn";
            this.serBtn.Size = new System.Drawing.Size(102, 28);
            this.serBtn.TabIndex = 3;
            this.serBtn.TabStop = true;
            this.serBtn.Text = "Server";
            this.serBtn.UseVisualStyleBackColor = true;
            this.serBtn.CheckedChanged += new System.EventHandler(this.serBtn_CheckedChanged);
            // 
            // cliBtn
            // 
            this.cliBtn.AutoSize = true;
            this.cliBtn.Font = new System.Drawing.Font("굴림", 10F);
            this.cliBtn.Location = new System.Drawing.Point(224, 52);
            this.cliBtn.Name = "cliBtn";
            this.cliBtn.Size = new System.Drawing.Size(91, 28);
            this.cliBtn.TabIndex = 3;
            this.cliBtn.TabStop = true;
            this.cliBtn.Text = "Client";
            this.cliBtn.UseVisualStyleBackColor = true;
            this.cliBtn.CheckedChanged += new System.EventHandler(this.cliBtn_CheckedChanged);
            // 
            // logTxt
            // 
            this.logTxt.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.logTxt.Location = new System.Drawing.Point(28, 648);
            this.logTxt.Multiline = true;
            this.logTxt.Name = "logTxt";
            this.logTxt.ReadOnly = true;
            this.logTxt.Size = new System.Drawing.Size(757, 260);
            this.logTxt.TabIndex = 2;
            // 
            // sendTxt
            // 
            this.sendTxt.Location = new System.Drawing.Point(55, 50);
            this.sendTxt.Name = "sendTxt";
            this.sendTxt.Size = new System.Drawing.Size(309, 32);
            this.sendTxt.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.BurlyWood;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.serBtn);
            this.panel1.Controls.Add(this.tcpConn);
            this.panel1.Controls.Add(this.cliBtn);
            this.panel1.Location = new System.Drawing.Point(101, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 116);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.BurlyWood;
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.sendBtn);
            this.panel2.Controls.Add(this.sendTxt);
            this.panel2.Location = new System.Drawing.Point(101, 486);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(642, 118);
            this.panel2.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 10F);
            this.label7.Location = new System.Drawing.Point(18, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 24);
            this.label7.TabIndex = 4;
            this.label7.Text = "TCP Option";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 10F);
            this.label8.Location = new System.Drawing.Point(3, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 24);
            this.label8.TabIndex = 4;
            this.label8.Text = "Send Data";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 10F);
            this.label9.Location = new System.Drawing.Point(24, 621);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 24);
            this.label9.TabIndex = 4;
            this.label9.Text = "Log";
            // 
            // timeOutUpDown
            // 
            this.timeOutUpDown.Font = new System.Drawing.Font("굴림", 10F);
            this.timeOutUpDown.Location = new System.Drawing.Point(320, 133);
            this.timeOutUpDown.Name = "timeOutUpDown";
            this.timeOutUpDown.Size = new System.Drawing.Size(453, 34);
            this.timeOutUpDown.TabIndex = 2;
            this.timeOutUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // reConnUpDown
            // 
            this.reConnUpDown.Font = new System.Drawing.Font("굴림", 10F);
            this.reConnUpDown.Location = new System.Drawing.Point(320, 198);
            this.reConnUpDown.Name = "reConnUpDown";
            this.reConnUpDown.Size = new System.Drawing.Size(453, 34);
            this.reConnUpDown.TabIndex = 2;
            this.reConnUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TcpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(834, 1007);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.logTxt);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TcpForm";
            this.Text = "TcpForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeOutUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reConnUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button tcpConn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.RadioButton serBtn;
        private System.Windows.Forms.RadioButton cliBtn;
        private System.Windows.Forms.TextBox logTxt;
        private System.Windows.Forms.TextBox sendTxt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown reConnUpDown;
        private System.Windows.Forms.NumericUpDown timeOutUpDown;
    }
}
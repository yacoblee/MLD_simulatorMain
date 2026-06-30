namespace sendProject
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
            this.portList = new System.Windows.Forms.ComboBox();
            this.conBtn = new System.Windows.Forms.Button();
            this.txtBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ipTxt = new System.Windows.Forms.TextBox();
            this.portTxt = new System.Windows.Forms.TextBox();
            this.tcpBtn = new System.Windows.Forms.Button();
            this.sendBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // portList
            // 
            this.portList.FormattingEnabled = true;
            this.portList.Location = new System.Drawing.Point(55, 454);
            this.portList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.portList.Name = "portList";
            this.portList.Size = new System.Drawing.Size(106, 20);
            this.portList.TabIndex = 0;
            this.portList.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.portList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.portList_KeyDown);
            // 
            // conBtn
            // 
            this.conBtn.Location = new System.Drawing.Point(176, 435);
            this.conBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.conBtn.Name = "conBtn";
            this.conBtn.Size = new System.Drawing.Size(106, 40);
            this.conBtn.TabIndex = 1;
            this.conBtn.Text = "Connect";
            this.conBtn.UseVisualStyleBackColor = true;
            this.conBtn.Click += new System.EventHandler(this.conBtn_Click);
            // 
            // txtBox
            // 
            this.txtBox.Location = new System.Drawing.Point(291, 108);
            this.txtBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBox.Multiline = true;
            this.txtBox.Name = "txtBox";
            this.txtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBox.Size = new System.Drawing.Size(327, 245);
            this.txtBox.TabIndex = 2;
            this.txtBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label5.Location = new System.Drawing.Point(55, 435);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Protocol";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ipTxt
            // 
            this.ipTxt.Location = new System.Drawing.Point(41, 151);
            this.ipTxt.Margin = new System.Windows.Forms.Padding(2);
            this.ipTxt.Name = "ipTxt";
            this.ipTxt.Size = new System.Drawing.Size(126, 21);
            this.ipTxt.TabIndex = 5;
            // 
            // portTxt
            // 
            this.portTxt.Location = new System.Drawing.Point(41, 194);
            this.portTxt.Margin = new System.Windows.Forms.Padding(2);
            this.portTxt.Name = "portTxt";
            this.portTxt.Size = new System.Drawing.Size(126, 21);
            this.portTxt.TabIndex = 5;
            // 
            // tcpBtn
            // 
            this.tcpBtn.Location = new System.Drawing.Point(113, 234);
            this.tcpBtn.Margin = new System.Windows.Forms.Padding(2);
            this.tcpBtn.Name = "tcpBtn";
            this.tcpBtn.Size = new System.Drawing.Size(62, 28);
            this.tcpBtn.TabIndex = 6;
            this.tcpBtn.Text = "연결";
            this.tcpBtn.UseVisualStyleBackColor = true;
            this.tcpBtn.Click += new System.EventHandler(this.tcpBtn_Click);
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(41, 234);
            this.sendBtn.Margin = new System.Windows.Forms.Padding(2);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(62, 28);
            this.sendBtn.TabIndex = 6;
            this.sendBtn.Text = "전송";
            this.sendBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 137);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 180);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "PORT";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 520);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.tcpBtn);
            this.Controls.Add(this.portTxt);
            this.Controls.Add(this.ipTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.conBtn);
            this.Controls.Add(this.portList);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox portList;
        private System.Windows.Forms.Button conBtn;
        private System.Windows.Forms.TextBox txtBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ipTxt;
        private System.Windows.Forms.TextBox portTxt;
        private System.Windows.Forms.Button tcpBtn;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}


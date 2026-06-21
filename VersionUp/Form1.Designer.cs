namespace VersionUp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbPortName = new ComboBox();
            cmbSpeed = new ComboBox();
            cmbLength = new ComboBox();
            cmbBit = new ComboBox();
            cmbStopBit = new ComboBox();
            cmbProtocol = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            button1 = new Button();
            button2 = new Button();
            TextLog = new TextBox();
            SuspendLayout();
            // 
            // cmbPortName
            // 
            cmbPortName.FormattingEnabled = true;
            cmbPortName.Location = new Point(1147, 44);
            cmbPortName.Name = "cmbPortName";
            cmbPortName.Size = new Size(174, 38);
            cmbPortName.TabIndex = 2;
            // 
            // cmbSpeed
            // 
            cmbSpeed.FormattingEnabled = true;
            cmbSpeed.Location = new Point(1147, 112);
            cmbSpeed.Name = "cmbSpeed";
            cmbSpeed.Size = new Size(174, 38);
            cmbSpeed.TabIndex = 0;
            // 
            // cmbLength
            // 
            cmbLength.FormattingEnabled = true;
            cmbLength.Location = new Point(1147, 267);
            cmbLength.Name = "cmbLength";
            cmbLength.Size = new Size(174, 38);
            cmbLength.TabIndex = 0;
            // 
            // cmbBit
            // 
            cmbBit.FormattingEnabled = true;
            cmbBit.Location = new Point(1147, 184);
            cmbBit.Name = "cmbBit";
            cmbBit.Size = new Size(174, 38);
            cmbBit.TabIndex = 0;
            // 
            // cmbStopBit
            // 
            cmbStopBit.FormattingEnabled = true;
            cmbStopBit.Location = new Point(1147, 346);
            cmbStopBit.Name = "cmbStopBit";
            cmbStopBit.Size = new Size(174, 38);
            cmbStopBit.TabIndex = 0;
            // 
            // cmbProtocol
            // 
            cmbProtocol.FormattingEnabled = true;
            cmbProtocol.Location = new Point(1147, 427);
            cmbProtocol.Name = "cmbProtocol";
            cmbProtocol.Size = new Size(174, 38);
            cmbProtocol.TabIndex = 0;
            // 
            // label1
            // 
            label1.Location = new Point(1012, 49);
            label1.Name = "label1";
            label1.Size = new Size(115, 33);
            label1.TabIndex = 3;
            label1.Text = "포트";
            // 
            // label2
            // 
            label2.Location = new Point(1012, 117);
            label2.Name = "label2";
            label2.Size = new Size(115, 33);
            label2.TabIndex = 3;
            label2.Text = "통신속도";
            // 
            // label3
            // 
            label3.Location = new Point(1012, 189);
            label3.Name = "label3";
            label3.Size = new Size(129, 33);
            label3.TabIndex = 3;
            label3.Text = "페라티비트";
            // 
            // label4
            // 
            label4.Location = new Point(1012, 269);
            label4.Name = "label4";
            label4.Size = new Size(129, 33);
            label4.TabIndex = 3;
            label4.Text = "데이터길이";
            // 
            // label5
            // 
            label5.Location = new Point(1012, 351);
            label5.Name = "label5";
            label5.Size = new Size(115, 33);
            label5.TabIndex = 3;
            label5.Text = "정지비트";
            // 
            // label6
            // 
            label6.Location = new Point(1012, 432);
            label6.Name = "label6";
            label6.Size = new Size(115, 33);
            label6.TabIndex = 3;
            label6.Text = "프로토콜";
            // 
            // button1
            // 
            button1.Location = new Point(1174, 524);
            button1.Name = "button1";
            button1.Size = new Size(147, 56);
            button1.TabIndex = 4;
            button1.Text = "설정 저장";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(1012, 524);
            button2.Name = "button2";
            button2.Size = new Size(147, 56);
            button2.TabIndex = 4;
            button2.Text = "실행";
            button2.UseVisualStyleBackColor = true;
            // 
            // TextLog
            // 
            TextLog.Location = new Point(40, 44);
            TextLog.Multiline = true;
            TextLog.Name = "TextLog";
            TextLog.ScrollBars = ScrollBars.Horizontal;
            TextLog.Size = new Size(906, 340);
            TextLog.TabIndex = 5;
            TextLog.TextChanged += TextLog_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1360, 943);
            Controls.Add(TextLog);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cmbProtocol);
            Controls.Add(cmbStopBit);
            Controls.Add(cmbBit);
            Controls.Add(cmbLength);
            Controls.Add(cmbSpeed);
            Controls.Add(cmbPortName);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbPortName;
        private ComboBox cmbSpeed;
        private ComboBox cmbLength;
        private ComboBox cmbBit;
        private ComboBox cmbStopBit;
        private ComboBox cmbProtocol;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button button1;
        private Button button2;
        private TextBox TextLog;
    }
}

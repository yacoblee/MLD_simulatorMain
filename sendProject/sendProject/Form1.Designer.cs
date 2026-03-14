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
            this.SuspendLayout();
            // 
            // portList
            // 
            this.portList.FormattingEnabled = true;
            this.portList.Location = new System.Drawing.Point(402, 89);
            this.portList.Name = "portList";
            this.portList.Size = new System.Drawing.Size(121, 23);
            this.portList.TabIndex = 0;
            this.portList.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.portList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.portList_KeyDown);
            // 
            // conBtn
            // 
            this.conBtn.Location = new System.Drawing.Point(402, 144);
            this.conBtn.Name = "conBtn";
            this.conBtn.Size = new System.Drawing.Size(121, 50);
            this.conBtn.TabIndex = 1;
            this.conBtn.Text = "Connect";
            this.conBtn.UseVisualStyleBackColor = true;
            this.conBtn.Click += new System.EventHandler(this.conBtn_Click);
            // 
            // txtBox
            // 
            this.txtBox.Location = new System.Drawing.Point(12, 89);
            this.txtBox.Multiline = true;
            this.txtBox.Name = "txtBox";
            this.txtBox.ReadOnly = true;
            this.txtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBox.Size = new System.Drawing.Size(384, 363);
            this.txtBox.TabIndex = 2;
            this.txtBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 515);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.conBtn);
            this.Controls.Add(this.portList);
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
    }
}


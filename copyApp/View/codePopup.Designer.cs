namespace copyApp.View
{
    partial class codePopup
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.popupLb = new System.Windows.Forms.Label();
            this.barBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("굴림", 14F);
            this.textBox1.Location = new System.Drawing.Point(60, 122);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(272, 45);
            this.textBox1.TabIndex = 0;
            // 
            // popupLb
            // 
            this.popupLb.AutoSize = true;
            this.popupLb.Font = new System.Drawing.Font("굴림", 12F);
            this.popupLb.Location = new System.Drawing.Point(184, 50);
            this.popupLb.Name = "popupLb";
            this.popupLb.Size = new System.Drawing.Size(161, 28);
            this.popupLb.TabIndex = 1;
            this.popupLb.Text = "바코드 입력";
            // 
            // barBtn
            // 
            this.barBtn.Font = new System.Drawing.Font("굴림", 10F);
            this.barBtn.Location = new System.Drawing.Point(361, 122);
            this.barBtn.Name = "barBtn";
            this.barBtn.Size = new System.Drawing.Size(99, 45);
            this.barBtn.TabIndex = 2;
            this.barBtn.Text = "입력";
            this.barBtn.UseVisualStyleBackColor = true;
            this.barBtn.Click += new System.EventHandler(this.barBtn_Click);
            // 
            // codePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.barBtn);
            this.Controls.Add(this.popupLb);
            this.Controls.Add(this.textBox1);
            this.Name = "codePopup";
            this.Size = new System.Drawing.Size(519, 221);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label popupLb;
        private System.Windows.Forms.Button barBtn;
    }
}

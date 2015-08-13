namespace cal
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cal = new System.Windows.Forms.Button();
            this.inputA = new System.Windows.Forms.TextBox();
            this.result = new System.Windows.Forms.TextBox();
            this.inputB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cal
            // 
            this.cal.Location = new System.Drawing.Point(159, 107);
            this.cal.Name = "cal";
            this.cal.Size = new System.Drawing.Size(75, 23);
            this.cal.TabIndex = 0;
            this.cal.Text = "计算";
            this.cal.UseVisualStyleBackColor = true;
            this.cal.Click += new System.EventHandler(this.cal_Click);
            // 
            // inputA
            // 
            this.inputA.Location = new System.Drawing.Point(12, 57);
            this.inputA.Name = "inputA";
            this.inputA.Size = new System.Drawing.Size(100, 21);
            this.inputA.TabIndex = 1;
            // 
            // result
            // 
            this.result.Location = new System.Drawing.Point(12, 107);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(100, 21);
            this.result.TabIndex = 2;
            // 
            // inputB
            // 
            this.inputB.Location = new System.Drawing.Point(159, 57);
            this.inputB.Name = "inputB";
            this.inputB.Size = new System.Drawing.Size(100, 21);
            this.inputB.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "+";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputB);
            this.Controls.Add(this.result);
            this.Controls.Add(this.inputA);
            this.Controls.Add(this.cal);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cal;
        private System.Windows.Forms.TextBox inputA;
        private System.Windows.Forms.TextBox result;
        private System.Windows.Forms.TextBox inputB;
        private System.Windows.Forms.Label label1;
    }
}


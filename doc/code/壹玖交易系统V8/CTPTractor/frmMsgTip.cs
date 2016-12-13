using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class frmMsgTip : System.Windows.Forms.Form
	{
		private IContainer components = null;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.GroupBox groupBox1;

		private System.Windows.Forms.Button button1;

		public frmMsgTip()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("华文楷体", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new System.Drawing.Point(3, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(291, 113);
			this.label1.TabIndex = 0;
			this.label1.Text = "关于期货投资者在使用\"壹玖交易系统V8\"等软件时，均存在如网络传输延迟或网络丢包等可能性的不确定情况，可能造成订单未被执行而导致的投资损失、争议及后果，福州壹玖金融均免责，不承担任何责任。";
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(9, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(297, 133);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.button1.Location = new System.Drawing.Point(118, 141);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "我知道了";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(318, 167);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "frmMsgTip";
			this.Text = "免责声明";
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}

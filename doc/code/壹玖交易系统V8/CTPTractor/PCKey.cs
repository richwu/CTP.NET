using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class PCKey : System.Windows.Forms.Form
	{
		private IContainer components = null;

		public System.Windows.Forms.TextBox txt_Key;

		private System.Windows.Forms.GroupBox groupBox1;

		public PCKey()
		{
			this.InitializeComponent();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PCKey));
			this.txt_Key = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.txt_Key.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txt_Key.Location = new System.Drawing.Point(3, 17);
			this.txt_Key.Name = "txt_Key";
			this.txt_Key.Size = new System.Drawing.Size(348, 21);
			this.txt_Key.TabIndex = 1;
			this.groupBox1.Controls.Add(this.txt_Key);
			this.groupBox1.Location = new System.Drawing.Point(1, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(354, 44);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "请将以下KEY发送给软件提供商";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(358, 53);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "PCKey";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "注册码";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}

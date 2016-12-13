using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class frmConfirm : System.Windows.Forms.Form
	{
		public delegate void selectcomfirm(string account, string date);

		public frmConfirm.selectcomfirm sc;

		private IContainer components = null;

		private System.Windows.Forms.ComboBox comboBox1;

		private System.Windows.Forms.DateTimePicker dateTimePicker1;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.TextBox textBox1;

		public frmConfirm()
		{
			this.InitializeComponent();
		}

		public void confirmInit(System.Collections.Generic.List<string> account)
		{
			this.comboBox1.Items.Clear();
			this.comboBox1.Items.AddRange(account.ToArray());
		}

		public new void Select()
		{
			if (!string.IsNullOrWhiteSpace(this.comboBox1.Text))
			{
				this.sc(this.comboBox1.Text, this.dateTimePicker1.Value.ToString("yyyyMMdd"));
			}
		}

		public void SetText(string settlement)
		{
			this.textBox1.Text = "";
			this.textBox1.Text = settlement;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Select();
		}

		private void frmConfirm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
		}

		private void frmConfirm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			base.Hide();
			e.Cancel = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmConfirm));
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(62, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(149, 20);
			this.comboBox1.TabIndex = 0;
			this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
			this.dateTimePicker1.Location = new System.Drawing.Point(288, 12);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(142, 21);
			this.dateTimePicker1.TabIndex = 1;
			this.button1.Location = new System.Drawing.Point(596, 7);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(90, 35);
			this.button1.TabIndex = 2;
			this.button1.Text = "查询";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(23, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "账户:";
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(238, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "日期:";
			this.textBox1.BackColor = System.Drawing.SystemColors.HighlightText;
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.textBox1.Location = new System.Drawing.Point(0, 48);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(709, 430);
			this.textBox1.TabIndex = 5;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(709, 478);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.dateTimePicker1);
			base.Controls.Add(this.comboBox1);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmConfirm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "历史结算单查询";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConfirm_FormClosing);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfirm_FormClosed);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}

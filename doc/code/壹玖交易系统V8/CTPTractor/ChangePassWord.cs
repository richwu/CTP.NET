using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class ChangePassWord : System.Windows.Forms.Form
	{
		private GetID gid = new GetID();

		private SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();

		private string dbpath = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\Msg.db";

		private IContainer components = null;

		private System.Windows.Forms.TextBox txt_oldpw;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.TextBox txt_newpw;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button btn_pwconfirm;

		private System.Windows.Forms.Button btn_pwclear;

		private System.Windows.Forms.TextBox txt_confirmpw;

		private System.Windows.Forms.Label label3;

		public ChangePassWord()
		{
			this.InitializeComponent();
			this.connstr.DataSource = this.dbpath;
			this.connstr.Password = this.gid.getRNum();
			this.txt_oldpw.Focus();
		}

		private void btn_pwclear_Click(object sender, System.EventArgs e)
		{
			this.txt_oldpw.Text = "";
			this.txt_newpw.Text = "";
			this.txt_confirmpw.Text = "";
		}

		private void btn_pwconfirm_Click(object sender, System.EventArgs e)
		{
			if (!(this.txt_oldpw.Text.Trim() == ""))
			{
				if (!(this.txt_newpw.Text.Trim() == ""))
				{
					if (!(this.txt_confirmpw.Text.Trim() == ""))
					{
						if (this.txt_newpw.Text.Trim() != this.txt_confirmpw.Text.Trim())
						{
							System.Windows.Forms.MessageBox.Show("输入的两次密码不同");
						}
						else if (this.DOSQLite(this.gid.getPW(this.txt_oldpw.Text.Trim()), 0))
						{
							this.DOSQLite(this.gid.getPW(this.txt_newpw.Text.Trim()), 1);
							if (System.Windows.Forms.MessageBox.Show("密码修改成功") == System.Windows.Forms.DialogResult.OK)
							{
								base.Close();
							}
						}
						else
						{
							System.Windows.Forms.MessageBox.Show("原始密码错误");
						}
					}
				}
			}
		}

		private bool DOSQLite(string pw, int type)
		{
			bool result;
			try
			{
				using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connstr.ToString()))
				{
					sQLiteConnection.Open();
					using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection))
					{
						if (type == 0)
						{
							sQLiteCommand.CommandText = "SELECT * FROM 'admin'";
							SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
							sQLiteDataReader.Read();
							string @string = sQLiteDataReader.GetString(2);
							result = (pw == @string);
						}
						else
						{
							sQLiteCommand.CommandText = "update  admin set password=@password where name='Administrator'; ";
							sQLiteCommand.Parameters.Add("@password", System.Data.DbType.String);
							sQLiteCommand.Parameters["@password"].Value = pw;
							sQLiteCommand.ExecuteScalar();
							result = (pw == pw);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
				result = false;
			}
			return result;
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
			this.txt_oldpw = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txt_newpw = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_pwconfirm = new System.Windows.Forms.Button();
			this.btn_pwclear = new System.Windows.Forms.Button();
			this.txt_confirmpw = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			base.SuspendLayout();
			this.txt_oldpw.Location = new System.Drawing.Point(79, 12);
			this.txt_oldpw.Name = "txt_oldpw";
			this.txt_oldpw.PasswordChar = '*';
			this.txt_oldpw.Size = new System.Drawing.Size(134, 21);
			this.txt_oldpw.TabIndex = 0;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "原始密码:";
			this.txt_newpw.Location = new System.Drawing.Point(79, 39);
			this.txt_newpw.Name = "txt_newpw";
			this.txt_newpw.PasswordChar = '*';
			this.txt_newpw.Size = new System.Drawing.Size(134, 21);
			this.txt_newpw.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 12);
			this.label1.TabIndex = 9;
			this.label1.Text = "新密码:";
			this.btn_pwconfirm.Location = new System.Drawing.Point(165, 98);
			this.btn_pwconfirm.Name = "btn_pwconfirm";
			this.btn_pwconfirm.Size = new System.Drawing.Size(49, 23);
			this.btn_pwconfirm.TabIndex = 3;
			this.btn_pwconfirm.Text = "确认";
			this.btn_pwconfirm.UseVisualStyleBackColor = true;
			this.btn_pwconfirm.Click += new System.EventHandler(this.btn_pwconfirm_Click);
			this.btn_pwclear.Location = new System.Drawing.Point(99, 98);
			this.btn_pwclear.Name = "btn_pwclear";
			this.btn_pwclear.Size = new System.Drawing.Size(49, 23);
			this.btn_pwclear.TabIndex = 4;
			this.btn_pwclear.Text = "重置";
			this.btn_pwclear.UseVisualStyleBackColor = true;
			this.btn_pwclear.Click += new System.EventHandler(this.btn_pwclear_Click);
			this.txt_confirmpw.Location = new System.Drawing.Point(79, 66);
			this.txt_confirmpw.Name = "txt_confirmpw";
			this.txt_confirmpw.PasswordChar = '*';
			this.txt_confirmpw.Size = new System.Drawing.Size(134, 21);
			this.txt_confirmpw.TabIndex = 2;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 12);
			this.label3.TabIndex = 13;
			this.label3.Text = "确认密码:";
			base.AcceptButton = this.btn_pwconfirm;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(226, 124);
			base.Controls.Add(this.txt_confirmpw);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btn_pwclear);
			base.Controls.Add(this.btn_pwconfirm);
			base.Controls.Add(this.txt_newpw);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.txt_oldpw);
			base.Controls.Add(this.label2);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "ChangePassWord";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "密码修改";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}

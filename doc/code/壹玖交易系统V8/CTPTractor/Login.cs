using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class Login : System.Windows.Forms.Form
	{
		private GetID gid = new GetID();

		private SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();

		private string dbpath = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\Msg.db";

		private IContainer components = null;

		private System.Windows.Forms.Button btnLogin;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.TextBox txt_pw;

		private System.Windows.Forms.TextBox textBox2;

		private System.Windows.Forms.Button btnReg;

		public Login()
		{
			this.InitializeComponent();
			this.connstr.DataSource = this.dbpath;
			this.connstr.Password = this.gid.getRNum();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (!(this.txt_pw.Text.Trim() == ""))
			{
				string text = this.SelectSQLite();
				string pW = this.gid.getPW(this.txt_pw.Text.Trim());
				if (text == "")
				{
					System.Windows.Forms.MessageBox.Show("读取帐户异常？", "询问", System.Windows.Forms.MessageBoxButtons.OK);
				}
				else if (pW == text)
				{
					base.Hide();
					frmMainFrame frmMainFrame = new frmMainFrame();
					frmMainFrame.Show();
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("密码错误!", "失败", System.Windows.Forms.MessageBoxButtons.OK);
				}
			}
		}

		private string SelectSQLite()
		{
			string result;
			try
			{
				using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connstr.ToString()))
				{
					sQLiteConnection.Open();
					using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection))
					{
						sQLiteCommand.CommandText = "SELECT count(*) FROM sqlite_master where type='table' and name='admin'";
						int num = System.Convert.ToInt32(sQLiteCommand.ExecuteScalar());
						if (num == 0)
						{
							sQLiteCommand.CommandText = "CREATE TABLE admin(id integer NOT NULL PRIMARY KEY AUTOINCREMENT,name  text,password text);";
							sQLiteCommand.ExecuteScalar();
						}
						else
						{
							sQLiteCommand.CommandText = "SELECT * FROM 'admin'";
							SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
							if (sQLiteDataReader.Read())
							{
								string text = sQLiteDataReader.GetString(2);
								result = text;
								return result;
							}
						}
					}
					using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection))
					{
						string text = this.gid.getPW("hthrgv");
						sQLiteCommand.CommandText = "INSERT INTO admin (name,password) VALUES (@name,@password); ";
						sQLiteCommand.Parameters.Add("@name", System.Data.DbType.String);
						sQLiteCommand.Parameters["@name"].Value = "Administrator";
						sQLiteCommand.Parameters.Add("@password", System.Data.DbType.String);
						sQLiteCommand.Parameters["@password"].Value = text;
						sQLiteCommand.ExecuteScalar();
						result = text;
					}
				}
			}
			catch (System.Exception ex)
			{
				if (System.Windows.Forms.MessageBox.Show(ex.Message) == System.Windows.Forms.DialogResult.OK)
				{
					System.Environment.Exit(System.Environment.ExitCode);
				}
				result = "";
			}
			return result;
		}

		private void Login_Load(object sender, System.EventArgs e)
		{
			UserKey userKey = new UserKey();
			if (!userKey.Key(0)) 
			{
				this.btnReg.Visible = true;
				this.btnLogin.Enabled = false;
			}
			else
			{
				base.ActiveControl = this.txt_pw;
				this.txt_pw.Focus();
			}
		}

		private void btnReg_Click(object sender, System.EventArgs e)
		{
			HardIDKey hardIDKey = new HardIDKey();
			new PCKey
			{
				txt_Key = 
				{
					Text = hardIDKey.GetCPUInfo() + hardIDKey.GetWebInfo() + hardIDKey.GetHardInfo()
				}
			}.Show();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Login));
			this.btnLogin = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txt_pw = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.btnReg = new System.Windows.Forms.Button();
			base.SuspendLayout();
			this.btnLogin.Location = new System.Drawing.Point(167, 76);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(62, 23);
			this.btnLogin.TabIndex = 1;
			this.btnLogin.Text = "登录";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "帐户:";
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "密码:";
			this.txt_pw.Location = new System.Drawing.Point(53, 47);
			this.txt_pw.Name = "txt_pw";
			this.txt_pw.PasswordChar = '*';
			this.txt_pw.Size = new System.Drawing.Size(176, 21);
			this.txt_pw.TabIndex = 6;
			this.textBox2.BackColor = System.Drawing.Color.White;
			this.textBox2.ForeColor = System.Drawing.SystemColors.InfoText;
			this.textBox2.Location = new System.Drawing.Point(53, 15);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(176, 21);
			this.textBox2.TabIndex = 7;
			this.textBox2.Text = "Administrator";
			this.btnReg.Location = new System.Drawing.Point(77, 76);
			this.btnReg.Name = "btnReg";
			this.btnReg.Size = new System.Drawing.Size(59, 23);
			this.btnReg.TabIndex = 8;
			this.btnReg.Text = "注册";
			this.btnReg.UseVisualStyleBackColor = true;
			this.btnReg.Visible = false;
			this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
			base.AcceptButton = this.btnLogin;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(241, 106);
			base.Controls.Add(this.btnReg);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.txt_pw);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnLogin);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.Name = "Login";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "壹玖交易系统V8";
			base.Load += new System.EventHandler(this.Login_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}

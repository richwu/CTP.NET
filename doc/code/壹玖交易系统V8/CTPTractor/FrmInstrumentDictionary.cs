using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CTPTractor
{
	public class FrmInstrumentDictionary : System.Windows.Forms.Form
	{
		public delegate void SelectedHandler(object sender, System.EventArgs e);

		private float fLengh = 138f;

		private System.Windows.Forms.Control ctrl;

		private System.Windows.Forms.Form frmParent;

		private string strSelectText;

		private int nSelectRow;

		private System.IntPtr activeForm = System.IntPtr.Zero;

		private IContainer components = null;

		private System.Windows.Forms.RichTextBox rtbList;

		public event FrmInstrumentDictionary.SelectedHandler SelectedEvent;

		public float FLengh
		{
			set
			{
				this.fLengh = value;
			}
		}

		public string StrSelectText
		{
			get
			{
				return this.strSelectText;
			}
		}

		public System.Collections.Generic.List<string> m_Ls
		{
			get;
			set;
		}

		public FrmInstrumentDictionary()
		{
			try
			{
				this.InitializeComponent();
				this.activeForm = FrmInstrumentDictionary.GetActiveWindow();
				base.Size = new System.Drawing.Size(System.Convert.ToInt32(this.fLengh), 170);
			}
			catch (System.Exception var_0_57)
			{
			}
		}

		public void Binding(System.Windows.Forms.Control _ctrl)
		{
			try
			{
				this.ctrl = _ctrl;
				this.frmParent = ((_ctrl.FindForm().ParentForm == null) ? _ctrl.FindForm() : _ctrl.FindForm().ParentForm);
				if (this.frmParent != null)
				{
					this.frmParent.Move += new System.EventHandler(this.frmParent_Move);
					this.frmParent.Resize += new System.EventHandler(this.frmParent_Resize);
					this.frmParent.Activated += new System.EventHandler(this.frmParent_Activated);
					this.ctrl.TextChanged += new System.EventHandler(this.ctrl_TextChanged);
					this.ctrl.Leave += new System.EventHandler(this.ctrl_Leave);
					this.ctrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctrl_KeyDown);
					this.ctrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ctrl_KeyPress);
					System.Drawing.Point bindingCtrlPoit = this.GetBindingCtrlPoit(this.ctrl);
					base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
					base.SetDesktopBounds(bindingCtrlPoit.X, bindingCtrlPoit.Y, base.Size.Width, base.Size.Width);
				}
			}
			catch (System.Exception var_1_133)
			{
			}
		}

		public void AddList(System.Collections.Generic.List<string> _ls)
		{
			try
			{
				this.rtbList.Text = "";
				foreach (string current in _ls)
				{
					float stringLength = this.GetStringLength(current);
					this.fLengh = ((this.fLengh < stringLength) ? stringLength : this.fLengh);
				}
				foreach (string current in _ls)
				{
					float stringLength = this.GetStringLength(current);
					System.Windows.Forms.RichTextBox expr_88 = this.rtbList;
					expr_88.Text = expr_88.Text + this.StrAddBlank(current, this.fLengh - stringLength) + "\n";
				}
			}
			catch (System.Exception var_2_D0)
			{
			}
		}

		private string StrAddBlank(string _str, float _fNum)
		{
			try
			{
				string str = " ";
				if (_fNum == 0f)
				{
					_fNum += 3.999f;
				}
				while (_fNum >= 0f)
				{
					_fNum -= 3.999f;
					_str += str;
				}
			}
			catch (System.Exception var_1_4A)
			{
			}
			return _str;
		}

		private float GetStringLength(string _str)
		{
			float result = 0f;
			try
			{
				result = base.CreateGraphics().MeasureString(_str, this.Font).Width;
			}
			catch
			{
			}
			return result;
		}

		private void rtbList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				this.SetSelect(e, false);
				FrmInstrumentDictionary.SetActiveWindow(this.activeForm);
			}
			catch (System.Exception var_0_1A)
			{
			}
		}

		private void rtbList_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				this.strSelectText = this.SetSelect(e, true).Trim(new char[]
				{
					'\n',
					' '
				});
				if (this.ctrl != null && this.ctrl.Text != null)
				{
					this.ctrl.Text = this.StrSelectText;
					if (this.SelectedEvent != null)
					{
						this.SelectedEvent(null, null);
					}
				}
				base.Hide();
			}
			catch (System.Exception var_0_7E)
			{
			}
		}

		private string SetSelect(System.Windows.Forms.MouseEventArgs e, bool bRet)
		{
			string text = "";
			string result;
			try
			{
				System.Drawing.Point pt = new System.Drawing.Point(e.X, e.Y);
				this.nSelectRow = this.rtbList.GetLineFromCharIndex(this.rtbList.GetCharIndexFromPosition(pt));
				if (this.nSelectRow < this.m_Ls.Count)
				{
					text = this.SetSelectRow(this.nSelectRow);
					if (bRet)
					{
						result = text;
						return result;
					}
				}
			}
			catch (System.Exception var_2_76)
			{
			}
			result = text;
			return result;
		}

		private string SetSelectRow(int nRow)
		{
			int num = this.rtbList.Lines.Length - 2;
			if (nRow < 0)
			{
				nRow = ((num > 0) ? num : 0);
			}
			if (nRow > num)
			{
				nRow = 0;
			}
			this.nSelectRow = nRow;
			int firstCharIndexFromLine = this.rtbList.GetFirstCharIndexFromLine(this.nSelectRow);
			int firstCharIndexFromLine2 = this.rtbList.GetFirstCharIndexFromLine(this.nSelectRow + 1);
			string result;
			if (firstCharIndexFromLine2 > firstCharIndexFromLine)
			{
				this.rtbList.Select(firstCharIndexFromLine, firstCharIndexFromLine2 - firstCharIndexFromLine - 1);
				result = this.rtbList.Text.Substring(firstCharIndexFromLine, firstCharIndexFromLine2 - firstCharIndexFromLine).Trim(new char[]
				{
					'\n',
					' '
				});
			}
			else
			{
				result = "";
			}
			return result;
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern System.IntPtr GetActiveWindow();

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern System.IntPtr SetActiveWindow(System.IntPtr hwnd);

		private void frmParent_Move(object sender, System.EventArgs e)
		{
			base.Location = this.GetBindingCtrlPoit(this.ctrl);
		}

		private void frmParent_Resize(object sender, System.EventArgs e)
		{
			base.Location = this.GetBindingCtrlPoit(this.ctrl);
		}

		private void frmParent_Activated(object sender, System.EventArgs e)
		{
		}

		private System.Drawing.Point GetBindingCtrlPoit(System.Windows.Forms.Control _ctrl)
		{
			System.Drawing.Point result = this.ctrl.PointToScreen(new System.Drawing.Point(0, 0));
			result.X -= 2;
			result.Y += this.ctrl.Height;
			return result;
		}

		private void FrmInstrumentDictionary_Leave(object sender, System.EventArgs e)
		{
			base.Hide();
		}

		private void ctrl_Leave(object sender, System.EventArgs e)
		{
			base.Hide();
		}

		private void ctrl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (this.ctrl != null && this.ctrl.Text != null && base.Visible)
				{
					System.Windows.Forms.Keys keyCode = e.KeyCode;
					if (keyCode != System.Windows.Forms.Keys.Return)
					{
						if (keyCode != System.Windows.Forms.Keys.Escape)
						{
							switch (keyCode)
							{
							case System.Windows.Forms.Keys.Up:
								this.SetSelectRow(this.nSelectRow - 1);
								break;
							case System.Windows.Forms.Keys.Down:
								this.SetSelectRow(this.nSelectRow + 1);
								break;
							}
						}
						else
						{
							base.Hide();
						}
					}
					else
					{
						string text = this.SetSelectRow(this.nSelectRow).Trim();
						this.ctrl.Text = ((text == "") ? this.ctrl.Text : text);
						if (this.SelectedEvent != null)
						{
							this.SelectedEvent(null, null);
						}
						base.Hide();
					}
				}
			}
			catch (System.Exception var_1_E9)
			{
			}
		}

		private void ctrl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void FrmInstrumentDictionary_Activated(object sender, System.EventArgs e)
		{
		}

		private void ctrl_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.ctrl != null && this.ctrl.Text != null)
				{
					if (this.ctrl.Text.ToString() != "")
					{
						System.Collections.Generic.List<string> list = (from w in this.m_Ls
						where w.Contains(this.ctrl.Text.ToLower()) || w.Contains(this.ctrl.Text.ToUpper())
						select w).ToList<string>();
						if (list.Count == 1 && list[0] == this.ctrl.Text)
						{
							if (base.Visible)
							{
								if (this.SelectedEvent != null)
								{
									this.SelectedEvent(null, null);
								}
								base.Hide();
							}
						}
						else
						{
							this.AddList(list);
							if (!base.Visible)
							{
								base.Location = this.GetBindingCtrlPoit(this.ctrl);
								base.Show(this.ctrl);
							}
							this.SetSelectRow(0);
							FrmInstrumentDictionary.SetActiveWindow(this.activeForm);
						}
					}
				}
			}
			catch (System.Exception var_1_11B)
			{
			}
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
			this.rtbList = new System.Windows.Forms.RichTextBox();
			base.SuspendLayout();
			this.rtbList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.rtbList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbList.HideSelection = false;
			this.rtbList.Location = new System.Drawing.Point(0, 0);
			this.rtbList.Name = "rtbList";
			this.rtbList.Size = new System.Drawing.Size(138, 170);
			this.rtbList.TabIndex = 0;
			this.rtbList.Text = "";
			this.rtbList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rtbList_MouseClick);
			this.rtbList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rtbList_MouseMove);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(138, 170);
			base.ControlBox = false;
			base.Controls.Add(this.rtbList);
			this.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmInstrumentDictionary";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "FrmInstrumentDictionary";
			base.Activated += new System.EventHandler(this.FrmInstrumentDictionary_Activated);
			base.Leave += new System.EventHandler(this.FrmInstrumentDictionary_Leave);
			base.ResumeLayout(false);
		}
	}
}

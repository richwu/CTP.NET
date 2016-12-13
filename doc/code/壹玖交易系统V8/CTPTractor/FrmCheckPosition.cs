using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class FrmCheckPosition : System.Windows.Forms.Form
	{
		public delegate void checkpositionform(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> dlchecktrans);

		public FrmCheckPosition.checkpositionform checktrade;

		private IContainer components = null;

		private System.Windows.Forms.TreeView tvTransList;

		private System.Windows.Forms.Button btncancle;

		private System.Windows.Forms.Button btnDoTrade;

		private System.Windows.Forms.GroupBox groupBox1;

		public FrmCheckPosition()
		{
			this.InitializeComponent();
		}

		public void Initialize(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> dlchecktrans)
		{
			foreach (string current in dlchecktrans.Keys)
			{
				if (dlchecktrans[current].Count != 0)
				{
					System.Windows.Forms.TreeNode treeNode = this.tvTransList.Nodes.Add(current, current);
					for (int i = 0; i < dlchecktrans[current].Count; i++)
					{
						string text = string.Concat(new string[]
						{
							dlchecktrans[current][i][0],
							(dlchecktrans[current][i][1] == "Buy") ? "买入" : "卖出",
							(dlchecktrans[current][i][2] == "open") ? "开仓" : ((dlchecktrans[current][i][2] == "close") ? "平仓" : ((dlchecktrans[current][i][2] == "closetoday") ? "平今" : "平昨")),
							dlchecktrans[current][i][4],
							"手"
						});
						System.Windows.Forms.TreeNode treeNode2 = treeNode.Nodes.Add(text, text);
						treeNode2.Tag = dlchecktrans[current][i];
					}
					treeNode.Expand();
				}
			}
		}

		private void btnDoTrade_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> dictionary = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();
				for (int i = 0; i < this.tvTransList.Nodes.Count; i++)
				{
					System.Collections.Generic.List<string[]> value = new System.Collections.Generic.List<string[]>();
					dictionary.Add(this.tvTransList.Nodes[i].Text, value);
					for (int j = 0; j < this.tvTransList.Nodes[i].Nodes.Count; j++)
					{
						if (this.tvTransList.Nodes[i].Nodes[j].Checked)
						{
							dictionary[this.tvTransList.Nodes[i].Text].Add((string[])this.tvTransList.Nodes[i].Nodes[j].Tag);
						}
					}
				}
				this.checktrade(dictionary);
				base.Close();
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString());
			}
		}

		private void tvTransList_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node.Nodes.Count != 0)
			{
				for (int i = 0; i < e.Node.Nodes.Count; i++)
				{
					if (e.Node.Checked)
					{
						e.Node.Nodes[i].Checked = true;
					}
					else
					{
						e.Node.Nodes[i].Checked = false;
					}
				}
			}
			else if (e.Node.Checked)
			{
			}
		}

		private void btncancle_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> dictionary = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();
				for (int i = 0; i < this.tvTransList.Nodes.Count; i++)
				{
					System.Collections.Generic.List<string[]> value = new System.Collections.Generic.List<string[]>();
					dictionary.Add(this.tvTransList.Nodes[i].Text, value);
					for (int j = 0; j < this.tvTransList.Nodes[i].Nodes.Count; j++)
					{
						dictionary[this.tvTransList.Nodes[i].Text].Add((string[])this.tvTransList.Nodes[i].Nodes[j].Tag);
					}
				}
				this.checktrade(dictionary);
				base.Close();
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString());
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmCheckPosition));
			this.tvTransList = new System.Windows.Forms.TreeView();
			this.btncancle = new System.Windows.Forms.Button();
			this.btnDoTrade = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.tvTransList.CheckBoxes = true;
			this.tvTransList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvTransList.Indent = 23;
			this.tvTransList.ItemHeight = 20;
			this.tvTransList.Location = new System.Drawing.Point(3, 17);
			this.tvTransList.Name = "tvTransList";
			this.tvTransList.Size = new System.Drawing.Size(361, 218);
			this.tvTransList.TabIndex = 0;
			this.tvTransList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTransList_AfterCheck);
			this.btncancle.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btncancle.Location = new System.Drawing.Point(291, 249);
			this.btncancle.Name = "btncancle";
			this.btncancle.Size = new System.Drawing.Size(75, 23);
			this.btncancle.TabIndex = 1;
			this.btncancle.Text = "一键校正";
			this.btncancle.UseVisualStyleBackColor = true;
			this.btncancle.Click += new System.EventHandler(this.btncancle_Click);
			this.btnDoTrade.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnDoTrade.Location = new System.Drawing.Point(201, 249);
			this.btnDoTrade.Name = "btnDoTrade";
			this.btnDoTrade.Size = new System.Drawing.Size(75, 23);
			this.btnDoTrade.TabIndex = 2;
			this.btnDoTrade.Text = "执行";
			this.btnDoTrade.UseVisualStyleBackColor = true;
			this.btnDoTrade.Click += new System.EventHandler(this.btnDoTrade_Click);
			this.groupBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.tvTransList);
			this.groupBox1.Location = new System.Drawing.Point(2, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(367, 238);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "选择要操作的指令";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(372, 277);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnDoTrade);
			base.Controls.Add(this.btncancle);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "FrmCheckPosition";
			this.Text = "仓位校对";
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}

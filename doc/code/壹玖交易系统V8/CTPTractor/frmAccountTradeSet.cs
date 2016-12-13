using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	public class frmAccountTradeSet : System.Windows.Forms.Form
	{
		public delegate void closeform();

		public frmAccountTradeSet.closeform cf;

		private IContainer components = null;

		private GridControl xdgAccountTradeSet;

		private GridView gvAccountTradeSet;

		private GridColumn gridColumn1;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private GridColumn gridColumn4;

		private GridColumn gridColumn5;

		private GridColumn gridColumn6;

		private GridColumn gridColumn7;

		private GridColumn gridColumn8;

		private RepositoryItemSpinEdit repositoryItemSpinEdit1;

		private RepositoryItemSpinEdit repositoryItemSpinEdit3;

		private RepositoryItemSpinEdit repositoryItemSpinEdit2;

		private RepositoryItemSpinEdit repositoryItemSpinEdit4;

		private RepositoryItemTextEdit repositoryItemTextEdit1;

		private GridColumn gridColumn10;

		private GridColumn gridColumn11;

		private GridColumn gridColumn9;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GridColumn gridColumn12;

		private GridColumn gridColumn13;

		private GridColumn gridColumn14;

		private GridColumn gridColumn15;

		private GridColumn gridColumn16;

		public frmAccountTradeSet()
		{
			this.InitializeComponent();
		}

		private void gvAccountTradeSet_InitNewRow(object sender, InitNewRowEventArgs e)
		{
			System.Data.DataRow dataRow = this.gvAccountTradeSet.GetDataRow(e.RowHandle);
			dataRow["价格"] = "/";
			dataRow["延时"] = "/";
			dataRow["撤单等待"] = "/";
			dataRow["最大交易量"] = "/";
			dataRow["多头仓差"] = "/";
			dataRow["空头仓差"] = "/";
			dataRow["是否反向"] = "False";
			dataRow["优先"] = "1";
			dataRow["开多让点"] = "0";
			dataRow["平多让点"] = "0";
			dataRow["开空让点"] = "0";
			dataRow["平空让点"] = "0";
		}

		private void gvAccountTradeSet_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete)
			{
				if (System.Windows.Forms.MessageBox.Show("确定删除该行数据?", "询问", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
				{
					GridView gridView = sender as GridView;
					int[] selectedRows = gridView.GetSelectedRows();
					for (int i = selectedRows.Length - 1; i >= 0; i--)
					{
						gridView.DeleteRow(selectedRows[i]);
					}
				}
			}
		}

		private void frmAccountTradeSet_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			this.cf();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmAccountTradeSet));
			this.xdgAccountTradeSet = new GridControl();
			this.gvAccountTradeSet = new GridView();
			this.gridColumn1 = new GridColumn();
			this.gridColumn2 = new GridColumn();
			this.gridColumn3 = new GridColumn();
			this.gridColumn4 = new GridColumn();
			this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
			this.gridColumn5 = new GridColumn();
			this.gridColumn6 = new GridColumn();
			this.gridColumn7 = new GridColumn();
			this.gridColumn8 = new GridColumn();
			this.gridColumn10 = new GridColumn();
			this.gridColumn11 = new GridColumn();
			this.gridColumn9 = new GridColumn();
			this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			this.gridColumn12 = new GridColumn();
			this.gridColumn13 = new GridColumn();
			this.gridColumn15 = new GridColumn();
			this.gridColumn14 = new GridColumn();
			this.gridColumn16 = new GridColumn();
			this.repositoryItemSpinEdit1 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit2 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit3 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit4 = new RepositoryItemSpinEdit();
			((ISupportInitialize)this.xdgAccountTradeSet).BeginInit();
			((ISupportInitialize)this.gvAccountTradeSet).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEdit1).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit2).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit3).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit4).BeginInit();
			base.SuspendLayout();
			this.xdgAccountTradeSet.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xdgAccountTradeSet.Location = new System.Drawing.Point(0, 0);
			this.xdgAccountTradeSet.MainView = this.gvAccountTradeSet;
			this.xdgAccountTradeSet.Name = "xdgAccountTradeSet";
			this.xdgAccountTradeSet.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemSpinEdit1,
				this.repositoryItemSpinEdit2,
				this.repositoryItemSpinEdit3,
				this.repositoryItemSpinEdit4,
				this.repositoryItemTextEdit1,
				this.repositoryItemCheckEdit1
			});
			this.xdgAccountTradeSet.Size = new System.Drawing.Size(826, 408);
			this.xdgAccountTradeSet.TabIndex = 0;
			this.xdgAccountTradeSet.ViewCollection.AddRange(new BaseView[]
			{
				this.gvAccountTradeSet
			});
			this.gvAccountTradeSet.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn1,
				this.gridColumn2,
				this.gridColumn3,
				this.gridColumn4,
				this.gridColumn5,
				this.gridColumn6,
				this.gridColumn7,
				this.gridColumn8,
				this.gridColumn10,
				this.gridColumn11,
				this.gridColumn9,
				this.gridColumn12,
				this.gridColumn13,
				this.gridColumn15,
				this.gridColumn14,
				this.gridColumn16
			});
			this.gvAccountTradeSet.GridControl = this.xdgAccountTradeSet;
			this.gvAccountTradeSet.Name = "gvAccountTradeSet";
			this.gvAccountTradeSet.NewItemRowText = "点击此处 添加新行";
			this.gvAccountTradeSet.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
			this.gvAccountTradeSet.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True;
			this.gvAccountTradeSet.OptionsFind.AllowFindPanel = false;
			this.gvAccountTradeSet.OptionsMenu.EnableColumnMenu = false;
			this.gvAccountTradeSet.OptionsMenu.EnableFooterMenu = false;
			this.gvAccountTradeSet.OptionsMenu.EnableGroupPanelMenu = false;
			this.gvAccountTradeSet.OptionsSelection.MultiSelect = true;
			this.gvAccountTradeSet.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
			this.gvAccountTradeSet.OptionsView.ShowGroupPanel = false;
			this.gvAccountTradeSet.InitNewRow += new InitNewRowEventHandler(this.gvAccountTradeSet_InitNewRow);
			this.gvAccountTradeSet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvAccountTradeSet_KeyDown);
			this.gridColumn1.Caption = "子帐户";
			this.gridColumn1.FieldName = "子帐户";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 0;
			this.gridColumn2.Caption = "品种";
			this.gridColumn2.FieldName = "品种";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.ToolTip = "不能为空,可填品种、合约、*;当为*时,表示跟所有品种;";
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 1;
			this.gridColumn3.Caption = "主帐户";
			this.gridColumn3.FieldName = "主帐户";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 2;
			this.gridColumn4.Caption = "手数倍率";
			this.gridColumn4.ColumnEdit = this.repositoryItemTextEdit1;
			this.gridColumn4.FieldName = "手数倍率";
			this.gridColumn4.Name = "gridColumn4";
			this.gridColumn4.Visible = true;
			this.gridColumn4.VisibleIndex = 3;
			this.repositoryItemTextEdit1.AutoHeight = false;
			this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
			this.gridColumn5.Caption = "价格";
			this.gridColumn5.FieldName = "价格";
			this.gridColumn5.Name = "gridColumn5";
			this.gridColumn5.ToolTip = "当为 / 时表示涨跌停跟单;为0时表示按主账户报价跟单;为正负数时表示按滑点跟单";
			this.gridColumn5.Visible = true;
			this.gridColumn5.VisibleIndex = 4;
			this.gridColumn6.Caption = "延时";
			this.gridColumn6.FieldName = "延时";
			this.gridColumn6.Name = "gridColumn6";
			this.gridColumn6.ToolTip = "主账户成交后,延时多少秒子账户进行跟单";
			this.gridColumn6.Visible = true;
			this.gridColumn6.VisibleIndex = 5;
			this.gridColumn7.Caption = "撤单等待";
			this.gridColumn7.FieldName = "撤单等待";
			this.gridColumn7.Name = "gridColumn7";
			this.gridColumn7.Visible = true;
			this.gridColumn7.VisibleIndex = 6;
			this.gridColumn8.FieldName = "最大交易量";
			this.gridColumn8.Name = "gridColumn8";
			this.gridColumn8.ToolTip = "每天最多跟多少手数";
			this.gridColumn10.Caption = "多头仓差";
			this.gridColumn10.FieldName = "多头仓差";
			this.gridColumn10.Name = "gridColumn10";
			this.gridColumn10.ToolTip = "校对时运用,当要核算仓位时,不想计算在总仓位持仓数可在此填入相应数值";
			this.gridColumn11.Caption = "空头仓差";
			this.gridColumn11.FieldName = "空头仓差";
			this.gridColumn11.Name = "gridColumn11";
			this.gridColumn11.ToolTip = "校对时运用,当要核算仓位时,不想计算在总仓位持仓数可在此填入相应数值";
			this.gridColumn9.Caption = "是否反向";
			this.gridColumn9.ColumnEdit = this.repositoryItemCheckEdit1;
			this.gridColumn9.FieldName = "是否反向";
			this.gridColumn9.Name = "gridColumn9";
			this.gridColumn9.Visible = true;
			this.gridColumn9.VisibleIndex = 7;
			this.repositoryItemCheckEdit1.AutoHeight = false;
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			this.gridColumn12.Caption = "优先";
			this.gridColumn12.FieldName = "优先";
			this.gridColumn12.Name = "gridColumn12";
			this.gridColumn13.Caption = "开多让点";
			this.gridColumn13.FieldName = "开多让点";
			this.gridColumn13.Name = "gridColumn13";
			this.gridColumn13.Visible = true;
			this.gridColumn13.VisibleIndex = 8;
			this.gridColumn15.Caption = "平多让点";
			this.gridColumn15.FieldName = "平多让点";
			this.gridColumn15.Name = "gridColumn15";
			this.gridColumn15.Visible = true;
			this.gridColumn15.VisibleIndex = 9;
			this.gridColumn14.Caption = "开空让点";
			this.gridColumn14.FieldName = "开空让点";
			this.gridColumn14.Name = "gridColumn14";
			this.gridColumn14.Visible = true;
			this.gridColumn14.VisibleIndex = 10;
			this.gridColumn16.Caption = "平空让点";
			this.gridColumn16.FieldName = "平空让点";
			this.gridColumn16.Name = "gridColumn16";
			this.gridColumn16.Visible = true;
			this.gridColumn16.VisibleIndex = 11;
			this.repositoryItemSpinEdit1.AutoHeight = false;
			this.repositoryItemSpinEdit1.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
			this.repositoryItemSpinEdit2.AutoHeight = false;
			this.repositoryItemSpinEdit2.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
			this.repositoryItemSpinEdit3.AutoHeight = false;
			this.repositoryItemSpinEdit3.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemSpinEdit3.Name = "repositoryItemSpinEdit3";
			this.repositoryItemSpinEdit4.AutoHeight = false;
			this.repositoryItemSpinEdit4.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemSpinEdit4.Name = "repositoryItemSpinEdit4";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(235, 236, 239);
			base.ClientSize = new System.Drawing.Size(826, 408);
			base.Controls.Add(this.xdgAccountTradeSet);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "frmAccountTradeSet";
			this.Text = "帐户下单配置";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAccountTradeSet_FormClosing);
			((ISupportInitialize)this.xdgAccountTradeSet).EndInit();
			((ISupportInitialize)this.gvAccountTradeSet).EndInit();
			((ISupportInitialize)this.repositoryItemTextEdit1).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit2).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit3).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit4).EndInit();
			base.ResumeLayout(false);
		}
	}
}

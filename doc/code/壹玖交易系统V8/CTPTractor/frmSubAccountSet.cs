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
	public class frmSubAccountSet : System.Windows.Forms.Form
	{
		public delegate void closeform();

		public delegate void subaccountParams(string subid, bool listen, bool proof);

		public frmSubAccountSet.closeform cf;

		public frmSubAccountSet.subaccountParams subParams;

		private IContainer components = null;

		private GridControl xdgSubAccountSet;

		private GridView gvSubAccountSet;

		private GridColumn gridColumn1;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private RepositoryItemSpinEdit repositoryItemSpinEdit1;

		private RepositoryItemSpinEdit repositoryItemSpinEdit2;

		private RepositoryItemSpinEdit repositoryItemSpinEdit3;

		private RepositoryItemSpinEdit repositoryItemSpinEdit4;

		private RepositoryItemTextEdit repositoryItemTextEdit1;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private RepositoryItemCheckEdit repositoryItemCheckEdit2;

		public frmSubAccountSet()
		{
			this.InitializeComponent();
		}

		private void frmSubAccountSet_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			this.cf();
		}

		private void gvSubAccountSet_InitNewRow(object sender, InitNewRowEventArgs e)
		{
			System.Data.DataRow dataRow = this.gvSubAccountSet.GetDataRow(e.RowHandle);
			dataRow["子帐户"] = "";
			dataRow["是否监听"] = false;
			dataRow["是否校正"] = false;
		}

		private void gvSubAccountSet_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete)
			{
				if (System.Windows.Forms.MessageBox.Show("确定删除该行数据?", "询问", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
				{
					GridView gridView = sender as GridView;
					int[] selectedRows = gridView.GetSelectedRows();
					for (int i = selectedRows.Length - 1; i >= 0; i--)
					{
						System.Data.DataRow dataRow = gridView.GetDataRow(selectedRows[i]);
						if (dataRow["子帐户"].ToString().Trim() != "")
						{
							this.subParams(dataRow["子帐户"].ToString().Trim(), false, false);
						}
						gridView.DeleteRow(selectedRows[i]);
					}
				}
			}
		}

		private void gvCodeSet_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void gvSubAccountSet_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			if (e.Column.FieldName == "是否监听" || e.Column.FieldName == "是否校正")
			{
				GridView gridView = sender as GridView;
				System.Data.DataRow dataRow = gridView.GetDataRow(e.RowHandle);
				if (dataRow["子帐户"].ToString().Trim() != "")
				{
					this.subParams(dataRow["子帐户"].ToString().Trim(), System.Convert.ToBoolean(dataRow["是否监听"].ToString()), System.Convert.ToBoolean(dataRow["是否校正"].ToString()));
				}
			}
		}

		private void gvSubAccountSet_BeforeLeaveRow(object sender, RowAllowEventArgs e)
		{
			GridView gridView = sender as GridView;
			gridView.Columns["子帐户"].OptionsColumn.AllowEdit = false;
		}

		private void gvSubAccountSet_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			GridView gridView = sender as GridView;
			if (gridView.IsNewItemRow(e.FocusedRowHandle))
			{
				gridView.Columns["子帐户"].OptionsColumn.AllowEdit = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmSubAccountSet));
			this.xdgSubAccountSet = new GridControl();
			this.gvSubAccountSet = new GridView();
			this.gridColumn1 = new GridColumn();
			this.gridColumn2 = new GridColumn();
			this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			this.gridColumn3 = new GridColumn();
			this.repositoryItemCheckEdit2 = new RepositoryItemCheckEdit();
			this.repositoryItemSpinEdit1 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit2 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit3 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit4 = new RepositoryItemSpinEdit();
			this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
			((ISupportInitialize)this.xdgSubAccountSet).BeginInit();
			((ISupportInitialize)this.gvSubAccountSet).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit2).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit2).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit3).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit4).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEdit1).BeginInit();
			base.SuspendLayout();
			this.xdgSubAccountSet.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xdgSubAccountSet.Location = new System.Drawing.Point(0, 0);
			this.xdgSubAccountSet.MainView = this.gvSubAccountSet;
			this.xdgSubAccountSet.Name = "xdgSubAccountSet";
			this.xdgSubAccountSet.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemSpinEdit1,
				this.repositoryItemSpinEdit2,
				this.repositoryItemSpinEdit3,
				this.repositoryItemSpinEdit4,
				this.repositoryItemTextEdit1,
				this.repositoryItemCheckEdit1,
				this.repositoryItemCheckEdit2
			});
			this.xdgSubAccountSet.Size = new System.Drawing.Size(349, 398);
			this.xdgSubAccountSet.TabIndex = 1;
			this.xdgSubAccountSet.ViewCollection.AddRange(new BaseView[]
			{
				this.gvSubAccountSet
			});
			this.gvSubAccountSet.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn1,
				this.gridColumn2,
				this.gridColumn3
			});
			this.gvSubAccountSet.GridControl = this.xdgSubAccountSet;
			this.gvSubAccountSet.Name = "gvSubAccountSet";
			this.gvSubAccountSet.NewItemRowText = "点击此处 添加新行";
			this.gvSubAccountSet.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
			this.gvSubAccountSet.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True;
			this.gvSubAccountSet.OptionsFind.AllowFindPanel = false;
			this.gvSubAccountSet.OptionsMenu.EnableColumnMenu = false;
			this.gvSubAccountSet.OptionsMenu.EnableFooterMenu = false;
			this.gvSubAccountSet.OptionsMenu.EnableGroupPanelMenu = false;
			this.gvSubAccountSet.OptionsSelection.MultiSelect = true;
			this.gvSubAccountSet.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
			this.gvSubAccountSet.OptionsView.ShowGroupPanel = false;
			this.gvSubAccountSet.InitNewRow += new InitNewRowEventHandler(this.gvSubAccountSet_InitNewRow);
			this.gvSubAccountSet.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvSubAccountSet_FocusedRowChanged);
			this.gvSubAccountSet.CellValueChanged += new CellValueChangedEventHandler(this.gvSubAccountSet_CellValueChanged);
			this.gvSubAccountSet.BeforeLeaveRow += new RowAllowEventHandler(this.gvSubAccountSet_BeforeLeaveRow);
			this.gvSubAccountSet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvSubAccountSet_KeyDown);
			this.gridColumn1.Caption = "子帐户";
			this.gridColumn1.FieldName = "子帐户";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.OptionsColumn.AllowEdit = false;
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 0;
			this.gridColumn2.Caption = "是否监听";
			this.gridColumn2.ColumnEdit = this.repositoryItemCheckEdit1;
			this.gridColumn2.FieldName = "是否监听";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 1;
			this.repositoryItemCheckEdit1.AutoHeight = false;
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			this.gridColumn3.Caption = "是否校正";
			this.gridColumn3.ColumnEdit = this.repositoryItemCheckEdit2;
			this.gridColumn3.FieldName = "是否校正";
			this.gridColumn3.Name = "gridColumn3";
			this.repositoryItemCheckEdit2.AutoHeight = false;
			this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
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
			this.repositoryItemTextEdit1.AutoHeight = false;
			this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(349, 398);
			base.Controls.Add(this.xdgSubAccountSet);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "frmSubAccountSet";
			this.Text = "子帐户监听设置";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSubAccountSet_FormClosing);
			((ISupportInitialize)this.xdgSubAccountSet).EndInit();
			((ISupportInitialize)this.gvSubAccountSet).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit2).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit2).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit3).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit4).EndInit();
			((ISupportInitialize)this.repositoryItemTextEdit1).EndInit();
			base.ResumeLayout(false);
		}
	}
}

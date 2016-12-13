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
	public class frmCodeSet : System.Windows.Forms.Form
	{
		public delegate void closeform();

		public frmCodeSet.closeform cf;

		private IContainer components = null;

		private GridControl xdgCodeSet;

		private GridView gvCodeSet;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private RepositoryItemSpinEdit repositoryItemSpinEdit4;

		private RepositoryItemSpinEdit repositoryItemSpinEdit1;

		private RepositoryItemSpinEdit repositoryItemSpinEdit2;

		private RepositoryItemSpinEdit repositoryItemSpinEdit3;

		private GridColumn gridColumn4;

		public frmCodeSet()
		{
			this.InitializeComponent();
		}

		private void gvCodeSet_InitNewRow(object sender, InitNewRowEventArgs e)
		{
			System.Data.DataRow dataRow = this.gvCodeSet.GetDataRow(e.RowHandle);
			dataRow["品种"] = "";
			dataRow["快捷键代码"] = "";
			dataRow["keys"] = "";
		}

		private void gvCodeSet_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void gvCodeSet_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			GridView gridView = sender as GridView;
			string text = "0123456789qwertyuioplkjhgfdsazxcvbnm'\b'";
			if (gridView.FocusedColumn.FieldName == "快捷键代码" && text.IndexOf(e.KeyChar) >= 0 && (gridView.EditingValue == null || gridView.EditingValue.ToString().Length <= 1))
			{
				gridView.EditingValue = "";
				gridView.SetFocusedRowCellValue("keys", (System.Windows.Forms.Keys)e.KeyChar);
				e.Handled = false;
			}
			else if (gridView.FocusedColumn.FieldName == "快捷键代码" && e.KeyChar == '\b')
			{
				e.Handled = false;
			}
			else if (gridView.FocusedColumn.FieldName == "快捷键代码")
			{
				e.Handled = true;
			}
		}

		private void xdgCodeSet_EditorKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			GridControl gridControl = sender as GridControl;
			this.gvCodeSet_KeyPress(gridControl.FocusedView, e);
		}

		private void frmCodeSet_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			this.cf();
		}

		private void gvCodeSet_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmCodeSet));
			this.xdgCodeSet = new GridControl();
			this.gvCodeSet = new GridView();
			this.gridColumn2 = new GridColumn();
			this.gridColumn3 = new GridColumn();
			this.gridColumn4 = new GridColumn();
			this.repositoryItemSpinEdit1 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit2 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit3 = new RepositoryItemSpinEdit();
			this.repositoryItemSpinEdit4 = new RepositoryItemSpinEdit();
			((ISupportInitialize)this.xdgCodeSet).BeginInit();
			((ISupportInitialize)this.gvCodeSet).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit2).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit3).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit4).BeginInit();
			base.SuspendLayout();
			this.xdgCodeSet.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xdgCodeSet.Location = new System.Drawing.Point(0, 0);
			this.xdgCodeSet.MainView = this.gvCodeSet;
			this.xdgCodeSet.Name = "xdgCodeSet";
			this.xdgCodeSet.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemSpinEdit1,
				this.repositoryItemSpinEdit2,
				this.repositoryItemSpinEdit3,
				this.repositoryItemSpinEdit4
			});
			this.xdgCodeSet.Size = new System.Drawing.Size(604, 343);
			this.xdgCodeSet.TabIndex = 1;
			this.xdgCodeSet.ViewCollection.AddRange(new BaseView[]
			{
				this.gvCodeSet
			});
			this.xdgCodeSet.EditorKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.xdgCodeSet_EditorKeyPress);
			this.gvCodeSet.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn2,
				this.gridColumn3,
				this.gridColumn4
			});
			this.gvCodeSet.GridControl = this.xdgCodeSet;
			this.gvCodeSet.Name = "gvCodeSet";
			this.gvCodeSet.NewItemRowText = "点击此处 添加新行";
			this.gvCodeSet.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
			this.gvCodeSet.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True;
			this.gvCodeSet.OptionsFind.AllowFindPanel = false;
			this.gvCodeSet.OptionsMenu.EnableColumnMenu = false;
			this.gvCodeSet.OptionsMenu.EnableFooterMenu = false;
			this.gvCodeSet.OptionsMenu.EnableGroupPanelMenu = false;
			this.gvCodeSet.OptionsSelection.MultiSelect = true;
			this.gvCodeSet.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
			this.gvCodeSet.OptionsView.ShowGroupPanel = false;
			this.gvCodeSet.InitNewRow += new InitNewRowEventHandler(this.gvCodeSet_InitNewRow);
			this.gvCodeSet.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvCodeSet_FocusedRowChanged);
			this.gvCodeSet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvCodeSet_KeyDown);
			this.gvCodeSet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gvCodeSet_KeyPress);
			this.gridColumn2.Caption = "品种";
			this.gridColumn2.FieldName = "品种";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 0;
			this.gridColumn3.Caption = "快捷键代码";
			this.gridColumn3.DisplayFormat.FormatString = "{1}";
			this.gridColumn3.FieldName = "快捷键代码";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 1;
			this.gridColumn4.Caption = "keys";
			this.gridColumn4.FieldName = "keys";
			this.gridColumn4.Name = "gridColumn4";
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
			base.ClientSize = new System.Drawing.Size(604, 343);
			base.Controls.Add(this.xdgCodeSet);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "frmCodeSet";
			this.Text = "合约设置";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCodeSet_FormClosing);
			((ISupportInitialize)this.xdgCodeSet).EndInit();
			((ISupportInitialize)this.gvCodeSet).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit2).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit3).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit4).EndInit();
			base.ResumeLayout(false);
		}
	}
}

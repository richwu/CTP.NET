using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CTPTractor
{
	public class AccountSet : System.Windows.Forms.Form
	{
		private string dbpath = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\Msg.db";

		private SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();

		private System.Data.DataTable dtAccount = new System.Data.DataTable();

		private GetID gid = new GetID();

		private Encrypt ep = new Encrypt();

		private System.Collections.Generic.List<Brokers> list_b = new System.Collections.Generic.List<Brokers>();

		private IContainer components = null;

		private System.Windows.Forms.Button btnUpdate;

		private System.Windows.Forms.GroupBox gbAccountSet;

		private GridControl DGAccount;

		private BandedGridView xAccount;

		private BandedGridColumn account;

		private BandedGridColumn accountname;

		private BandedGridColumn mainaccount;

		private BandedGridColumn password;

		private BandedGridColumn brokers;

		private BandedGridColumn id;

		private RepositoryItemComboBox repositoryItemComboBox1;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button button1;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GridBand gridBand1;

		private BandedGridColumn apitype;

		private RepositoryItemComboBox repositoryItemComboBox2;

		private System.Windows.Forms.Button button2;

		public AccountSet()
		{
			this.InitializeComponent();
			this.connstr.DataSource = this.dbpath;
			this.connstr.Password = this.gid.getRNum();
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (System.Windows.Forms.MessageBox.Show("确定保存你所做的修改？\r\n保存完毕将自动退出软件", "询问", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
				{
					using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connstr.ToString()))
					{
						SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand
						{
							CommandText = "SELECT id as 序号,account as 投资者帐户,accountname as 投资者,mainaccount as 主账户,password as 密码,brokers as 期货公司,type as 类型 FROM user"
						}.CommandText, sQLiteConnection);
						SQLiteCommandBuilder sQLiteCommandBuilder = new SQLiteCommandBuilder(sQLiteDataAdapter);
						System.Data.DataTable changes;
						if ((changes = this.dtAccount.GetChanges()) != null)
						{
							sQLiteDataAdapter.Update(changes);
							this.dtAccount.AcceptChanges();
						}
						this.dtAccount.Clear();
						sQLiteDataAdapter.Fill(this.dtAccount);
						this.DGAccount.DataSource = null;
						this.DGAccount.DataSource = this.dtAccount;
						this.xAccount.Columns["密码"].DisplayFormat.FormatType = FormatType.Numeric;
						this.xAccount.Columns["密码"].DisplayFormat.FormatString = "￥{0:N2}";
					}
					if (System.Windows.Forms.MessageBox.Show("保存完毕") == System.Windows.Forms.DialogResult.OK)
					{
						base.DialogResult = System.Windows.Forms.DialogResult.OK;
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
			}
		}

		private void btnDel_Click(object sender, System.EventArgs e)
		{
		}

		public void SystemSet_value(System.Collections.Generic.List<Brokers> lb)
		{
			this.list_b = lb;
		}

		private void SystemSet_Load(object sender, System.EventArgs e)
		{
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connstr.ToString()))
			{
				string commandText = "SELECT id as 序号,account as 投资者帐户,accountname as 投资者,mainaccount as 主账户,password as 密码,brokers as 期货公司,type as 类型 FROM user";
				using (SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(commandText, sQLiteConnection))
				{
					try
					{
						sQLiteDataAdapter.Fill(this.dtAccount);
						RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
						foreach (Brokers current in this.list_b)
						{
							repositoryItemComboBox.Items.Add(current.BrokerName);
						}
						RepositoryItemComboBox repositoryItemComboBox2 = new RepositoryItemComboBox();
						repositoryItemComboBox2.Items.Add("CTP");
						repositoryItemComboBox2.Items.Add("金牛");
						repositoryItemComboBox2.Items.Add("鑫管家");
						repositoryItemComboBox2.TextEditStyle = TextEditStyles.DisableTextEditor;
						this.DGAccount.DataSource = this.dtAccount;
						this.xAccount.Columns["期货公司"].ColumnEdit = repositoryItemComboBox;
						this.xAccount.Columns["类型"].ColumnEdit = repositoryItemComboBox2;
						RepositoryItemTextEdit repositoryItemTextEdit = new RepositoryItemTextEdit();
						repositoryItemTextEdit.PasswordChar = '*';
						this.xAccount.Columns["密码"].ColumnEdit = repositoryItemTextEdit;
					}
					catch (System.Exception var_7_145)
					{
					}
				}
			}
		}

		private void DGAccount_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 7)
			{
				if (e.Value != null && e.Value.ToString().Length > 0)
				{
					e.Value = new string('*', e.Value.ToString().Length);
				}
			}
		}

		private void xAccount_InitNewRow(object sender, InitNewRowEventArgs e)
		{
			System.Data.DataRow dataRow = this.xAccount.GetDataRow(e.RowHandle);
			dataRow["序号"] = 0;
			dataRow["投资者帐户"] = "";
			dataRow["投资者"] = "";
			dataRow["主账户"] = "False";
			dataRow["密码"] = "";
			dataRow["期货公司"] = "";
			dataRow["类型"] = "";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("确定删除这些数据?", "询问", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			{
				GridView gridView = this.xAccount;
				int[] selectedRows = gridView.GetSelectedRows();
				for (int i = selectedRows.Length - 1; i >= 0; i--)
				{
					gridView.DeleteRow(selectedRows[i]);
				}
			}
		}

		private void xAccount_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Filter = "文本文件(*.xlsx)|*.xlsx";
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				ReadExcel readExcel = new ReadExcel();
				System.Data.DataSet dataSet = readExcel.ReadExcelToDataSet(fileName);
				if (dataSet.Tables.Count > 0)
				{
					this.dtAccount.Rows.Clear();
					for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
					{
						if (!string.IsNullOrWhiteSpace(dataSet.Tables[0].Rows[i]["投资者帐户"].ToString()))
						{
							System.Data.DataRow dataRow = this.dtAccount.NewRow();
							dataRow["投资者帐户"] = dataSet.Tables[0].Rows[i]["投资者帐户"].ToString();
							dataRow["主账户"] = dataSet.Tables[0].Rows[i]["主账户"].ToString();
							dataRow["密码"] = dataSet.Tables[0].Rows[i]["密码"].ToString();
							dataRow["期货公司"] = dataSet.Tables[0].Rows[i]["期货公司"].ToString();
							dataRow["类型"] = dataSet.Tables[0].Rows[i]["类型"].ToString();
							this.dtAccount.Rows.Add(dataRow);
						}
					}
				}
				System.Windows.Forms.MessageBox.Show("导入完成");
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
			StyleFormatCondition styleFormatCondition = new StyleFormatCondition();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AccountSet));
			this.repositoryItemComboBox1 = new RepositoryItemComboBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.gbAccountSet = new System.Windows.Forms.GroupBox();
			this.DGAccount = new GridControl();
			this.xAccount = new BandedGridView();
			this.gridBand1 = new GridBand();
			this.id = new BandedGridColumn();
			this.account = new BandedGridColumn();
			this.accountname = new BandedGridColumn();
			this.password = new BandedGridColumn();
			this.brokers = new BandedGridColumn();
			this.apitype = new BandedGridColumn();
			this.repositoryItemComboBox2 = new RepositoryItemComboBox();
			this.mainaccount = new BandedGridColumn();
			this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			((ISupportInitialize)this.repositoryItemComboBox1).BeginInit();
			this.gbAccountSet.SuspendLayout();
			((ISupportInitialize)this.DGAccount).BeginInit();
			((ISupportInitialize)this.xAccount).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBox2).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			base.SuspendLayout();
			this.repositoryItemComboBox1.AutoHeight = false;
			this.repositoryItemComboBox1.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
			this.btnUpdate.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnUpdate.Location = new System.Drawing.Point(688, 90);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(70, 51);
			this.btnUpdate.TabIndex = 6;
			this.btnUpdate.Text = "保存";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			this.gbAccountSet.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.gbAccountSet.Controls.Add(this.DGAccount);
			this.gbAccountSet.Location = new System.Drawing.Point(8, 3);
			this.gbAccountSet.Name = "gbAccountSet";
			this.gbAccountSet.Size = new System.Drawing.Size(674, 347);
			this.gbAccountSet.TabIndex = 12;
			this.gbAccountSet.TabStop = false;
			this.gbAccountSet.Text = "添加、修改帐户";
			this.DGAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DGAccount.Location = new System.Drawing.Point(3, 17);
			this.DGAccount.MainView = this.xAccount;
			this.DGAccount.Name = "DGAccount";
			this.DGAccount.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEdit1,
				this.repositoryItemComboBox2
			});
			this.DGAccount.Size = new System.Drawing.Size(668, 327);
			this.DGAccount.TabIndex = 17;
			this.DGAccount.ViewCollection.AddRange(new BaseView[]
			{
				this.xAccount
			});
			this.xAccount.Appearance.BandPanel.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.xAccount.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.xAccount.Appearance.BandPanel.BorderColor = System.Drawing.Color.White;
			this.xAccount.Appearance.BandPanel.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.BandPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.xAccount.Appearance.BandPanel.Options.UseBackColor = true;
			this.xAccount.Appearance.BandPanel.Options.UseBorderColor = true;
			this.xAccount.Appearance.BandPanel.Options.UseForeColor = true;
			this.xAccount.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.xAccount.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.xAccount.Appearance.BandPanelBackground.Options.UseBackColor = true;
			this.xAccount.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.xAccount.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.xAccount.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.xAccount.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.xAccount.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.xAccount.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.xAccount.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.xAccount.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.xAccount.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.xAccount.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.xAccount.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.xAccount.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.xAccount.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.xAccount.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.xAccount.Appearance.Empty.Options.UseBackColor = true;
			this.xAccount.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.xAccount.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.xAccount.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.xAccount.Appearance.EvenRow.Options.UseBackColor = true;
			this.xAccount.Appearance.EvenRow.Options.UseBorderColor = true;
			this.xAccount.Appearance.EvenRow.Options.UseForeColor = true;
			this.xAccount.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.xAccount.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.xAccount.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.xAccount.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.xAccount.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.xAccount.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.xAccount.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.xAccount.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.xAccount.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.FilterPanel.Options.UseBackColor = true;
			this.xAccount.Appearance.FilterPanel.Options.UseForeColor = true;
			this.xAccount.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.xAccount.Appearance.FixedLine.Options.UseBackColor = true;
			this.xAccount.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.xAccount.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.FocusedCell.Options.UseBackColor = true;
			this.xAccount.Appearance.FocusedCell.Options.UseForeColor = true;
			this.xAccount.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.xAccount.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.xAccount.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.xAccount.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.xAccount.Appearance.FocusedRow.Options.UseBackColor = true;
			this.xAccount.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.xAccount.Appearance.FocusedRow.Options.UseForeColor = true;
			this.xAccount.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.xAccount.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.xAccount.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.xAccount.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.xAccount.Appearance.FooterPanel.Options.UseBackColor = true;
			this.xAccount.Appearance.FooterPanel.Options.UseForeColor = true;
			this.xAccount.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.xAccount.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.xAccount.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.xAccount.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.xAccount.Appearance.GroupButton.Options.UseBackColor = true;
			this.xAccount.Appearance.GroupButton.Options.UseBorderColor = true;
			this.xAccount.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.xAccount.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.xAccount.Appearance.GroupFooter.Options.UseBackColor = true;
			this.xAccount.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.xAccount.Appearance.GroupFooter.Options.UseForeColor = true;
			this.xAccount.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.xAccount.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.xAccount.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.xAccount.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.xAccount.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.GroupPanel.Options.UseBackColor = true;
			this.xAccount.Appearance.GroupPanel.Options.UseForeColor = true;
			this.xAccount.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.xAccount.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.xAccount.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.xAccount.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.GroupRow.Options.UseBackColor = true;
			this.xAccount.Appearance.GroupRow.Options.UseBorderColor = true;
			this.xAccount.Appearance.GroupRow.Options.UseForeColor = true;
			this.xAccount.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.xAccount.Appearance.HeaderPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.xAccount.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.xAccount.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.xAccount.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.xAccount.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.xAccount.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.xAccount.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.xAccount.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.xAccount.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.xAccount.Appearance.HorzLine.Options.UseBackColor = true;
			this.xAccount.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.xAccount.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.xAccount.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.xAccount.Appearance.OddRow.Options.UseBackColor = true;
			this.xAccount.Appearance.OddRow.Options.UseBorderColor = true;
			this.xAccount.Appearance.OddRow.Options.UseForeColor = true;
			this.xAccount.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.xAccount.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.xAccount.Appearance.Preview.Options.UseBackColor = true;
			this.xAccount.Appearance.Preview.Options.UseFont = true;
			this.xAccount.Appearance.Preview.Options.UseForeColor = true;
			this.xAccount.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.xAccount.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.Row.Options.UseBackColor = true;
			this.xAccount.Appearance.Row.Options.UseForeColor = true;
			this.xAccount.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.xAccount.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.xAccount.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.RowSeparator.Options.UseBackColor = true;
			this.xAccount.Appearance.RowSeparator.Options.UseForeColor = true;
			this.xAccount.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.xAccount.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.xAccount.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.xAccount.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.xAccount.Appearance.SelectedRow.Options.UseBackColor = true;
			this.xAccount.Appearance.SelectedRow.Options.UseForeColor = true;
			this.xAccount.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.xAccount.Appearance.TopNewRow.Options.UseBackColor = true;
			this.xAccount.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.xAccount.Appearance.VertLine.Options.UseBackColor = true;
			this.xAccount.Bands.AddRange(new GridBand[]
			{
				this.gridBand1
			});
			this.xAccount.BorderStyle = BorderStyles.Simple;
			this.xAccount.Columns.AddRange(new BandedGridColumn[]
			{
				this.id,
				this.account,
				this.accountname,
				this.mainaccount,
				this.password,
				this.brokers,
				this.apitype
			});
			this.xAccount.FooterPanelHeight = 5;
			styleFormatCondition.ApplyToRow = true;
			styleFormatCondition.Condition = FormatConditionEnum.Equal;
			styleFormatCondition.Value1 = true;
			this.xAccount.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition
			});
			this.xAccount.GridControl = this.DGAccount;
			this.xAccount.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.xAccount.HorzScrollVisibility = ScrollVisibility.Always;
			this.xAccount.Name = "xAccount";
			this.xAccount.NewItemRowText = "点击此处 添加新行";
			this.xAccount.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
			this.xAccount.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True;
			this.xAccount.OptionsCustomization.AllowFilter = false;
			this.xAccount.OptionsMenu.EnableColumnMenu = false;
			this.xAccount.OptionsMenu.EnableFooterMenu = false;
			this.xAccount.OptionsPrint.AutoWidth = false;
			this.xAccount.OptionsSelection.MultiSelect = true;
			this.xAccount.OptionsView.ColumnAutoWidth = false;
			this.xAccount.OptionsView.EnableAppearanceEvenRow = true;
			this.xAccount.OptionsView.EnableAppearanceOddRow = true;
			this.xAccount.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
			this.xAccount.OptionsView.ShowGroupPanel = false;
			this.xAccount.VertScrollVisibility = ScrollVisibility.Always;
			this.xAccount.RowCellStyle += new RowCellStyleEventHandler(this.xAccount_RowCellStyle);
			this.xAccount.InitNewRow += new InitNewRowEventHandler(this.xAccount_InitNewRow);
			this.gridBand1.Caption = "帐户";
			this.gridBand1.Columns.Add(this.id);
			this.gridBand1.Columns.Add(this.account);
			this.gridBand1.Columns.Add(this.accountname);
			this.gridBand1.Columns.Add(this.password);
			this.gridBand1.Columns.Add(this.brokers);
			this.gridBand1.Columns.Add(this.apitype);
			this.gridBand1.Columns.Add(this.mainaccount);
			this.gridBand1.Name = "gridBand1";
			this.gridBand1.Width = 628;
			this.id.Caption = "序号";
			this.id.FieldName = "序号";
			this.id.GroupFormat.FormatType = FormatType.Numeric;
			this.id.Name = "id";
			this.id.OptionsColumn.AllowEdit = false;
			this.id.Visible = true;
			this.id.Width = 98;
			this.account.Caption = "投资者帐户";
			this.account.FieldName = "投资者帐户";
			this.account.Name = "account";
			this.account.Visible = true;
			this.account.Width = 97;
			this.accountname.Caption = "投资者";
			this.accountname.FieldName = "投资者";
			this.accountname.Name = "accountname";
			this.accountname.Visible = true;
			this.accountname.Width = 76;
			this.password.Caption = "密码";
			this.password.DisplayFormat.FormatString = "*";
			this.password.FieldName = "密码";
			this.password.FilterMode = ColumnFilterMode.DisplayText;
			this.password.Name = "password";
			this.password.Tag = "";
			this.password.Visible = true;
			this.password.Width = 100;
			this.brokers.Caption = "期货公司";
			this.brokers.ColumnEdit = this.repositoryItemComboBox1;
			this.brokers.FieldName = "期货公司";
			this.brokers.GroupFormat.FormatType = FormatType.Numeric;
			this.brokers.Name = "brokers";
			this.brokers.Visible = true;
			this.brokers.Width = 100;
			this.apitype.Caption = "类型";
			this.apitype.ColumnEdit = this.repositoryItemComboBox2;
			this.apitype.FieldName = "类型";
			this.apitype.Name = "apitype";
			this.apitype.Visible = true;
			this.repositoryItemComboBox2.AutoHeight = false;
			this.repositoryItemComboBox2.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton(ButtonPredefines.DropDown)
			});
			this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
			this.mainaccount.AppearanceCell.ForeColor = System.Drawing.Color.Black;
			this.mainaccount.AppearanceCell.Options.UseForeColor = true;
			this.mainaccount.Caption = "主账户";
			this.mainaccount.ColumnEdit = this.repositoryItemCheckEdit1;
			this.mainaccount.FieldName = "主账户";
			this.mainaccount.Name = "mainaccount";
			this.mainaccount.Visible = true;
			this.mainaccount.Width = 82;
			this.repositoryItemCheckEdit1.AutoHeight = false;
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			this.label1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.label1.ForeColor = System.Drawing.Color.Brown;
			this.label1.Location = new System.Drawing.Point(688, 144);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 66);
			this.label1.TabIndex = 13;
			this.label1.Text = "注意:\r\n任何修改都\r\n将重启软件";
			this.button1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.button1.Location = new System.Drawing.Point(688, 29);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(70, 55);
			this.button1.TabIndex = 14;
			this.button1.Text = "删除";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			this.button2.Location = new System.Drawing.Point(688, 188);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 56);
			this.button2.TabIndex = 15;
			this.button2.Text = "EXCEL";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(769, 351);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.gbAccountSet);
			base.Controls.Add(this.btnUpdate);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MinimizeBox = false;
			base.Name = "AccountSet";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "帐户配置";
			base.Load += new System.EventHandler(this.SystemSet_Load);
			((ISupportInitialize)this.repositoryItemComboBox1).EndInit();
			this.gbAccountSet.ResumeLayout(false);
			((ISupportInitialize)this.DGAccount).EndInit();
			((ISupportInitialize)this.xAccount).EndInit();
			((ISupportInitialize)this.repositoryItemComboBox2).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			base.ResumeLayout(false);
		}
	}
}

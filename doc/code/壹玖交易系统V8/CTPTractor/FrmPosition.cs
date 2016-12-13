using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace CTPTractor
{
	public class FrmPosition : System.Windows.Forms.Form
	{
		public delegate void closeform(System.Windows.Forms.Form cf);

		public delegate void clicklistview(int group, System.Data.DataRow dr_focuserow);

		public FrmPosition.closeform backform;

		public FrmPosition.clicklistview clv;

		private int totalbposition = 0;

		private int totalsposition = 0;

		private int yBposition = 0;

		private int ySposition = 0;

		private int tBposition = 0;

		private int tSposition = 0;

		private string xmlPosition = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\XG_Position.xml";

		private string xmlPositionDetail = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\XG_PositionDetail.xml";

		private IContainer components = null;

		private System.Windows.Forms.RadioButton radio_position;

		private System.Windows.Forms.RadioButton radio_positiondetail;

		private GridControl xdgPosition;

		private BandedGridView bgvPosition;

		private BandedGridColumn colInstrument;

		private BandedGridColumn colPosidirection;

		private BandedGridColumn colTotalPosition;

		private BandedGridColumn colYPosition;

		private BandedGridColumn colTPosition;

		private BandedGridColumn colPositionCost;

		private BandedGridColumn colOpenCost;

		private BandedGridColumn colPositionProfit;

		private BandedGridColumn colShortUseMargin;

		private BandedGridColumn colHedgeFlag;

		private BandedGridColumn colTCanPosition;

		private BandedGridColumn colYCanPosition;

		private GridControl xdgPositionDetail;

		private BandedGridView bgvPositionDetail;

		private BandedGridColumn bandedGridColumn8;

		private BandedGridColumn bandedGridColumn2;

		private BandedGridColumn bandedGridColumn3;

		private BandedGridColumn bandedGridColumn4;

		private BandedGridColumn bandedGridColumn5;

		private BandedGridColumn bandedGridColumn6;

		private BandedGridColumn bandedGridColumn7;

		private GridBand gridBand2;

		private BandedGridColumn colLongUseMargin;

		private System.Windows.Forms.Panel panel1;

		private BandedGridColumn bandedGridColumn1;

		private GridBand gridBand1;

		public FrmPosition()
		{
			this.InitializeComponent();
		}

		private void FrmPosition_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			this.xdgPosition.MainView.SaveLayoutToXml(this.xmlPosition);
			this.xdgPositionDetail.MainView.SaveLayoutToXml(this.xmlPositionDetail);
			e.Cancel = true;
			this.backform(this);
		}

		private void CheckChange(System.Windows.Forms.RadioButton rb)
		{
			if (rb.Checked)
			{
				if (rb.Name == "radio_position")
				{
					this.xdgPosition.Visible = true;
					this.xdgPositionDetail.Visible = false;
				}
				else
				{
					this.xdgPosition.Visible = false;
					this.xdgPositionDetail.Visible = true;
				}
			}
		}

		private void FrmPosition_Load(object sender, System.EventArgs e)
		{
			if (!System.IO.File.Exists(this.xmlPosition))
			{
				this.xdgPosition.MainView.SaveLayoutToXml(this.xmlPosition);
			}
			this.xdgPosition.ForceInitialize();
			this.xdgPosition.MainView.RestoreLayoutFromXml(this.xmlPosition);
			if (!System.IO.File.Exists(this.xmlPositionDetail))
			{
				this.xdgPositionDetail.MainView.SaveLayoutToXml(this.xmlPositionDetail);
			}
			this.xdgPositionDetail.ForceInitialize();
			this.xdgPositionDetail.MainView.RestoreLayoutFromXml(this.xmlPositionDetail);
		}

		private void radio_positiondetail_Click(object sender, System.EventArgs e)
		{
			this.CheckChange(this.radio_positiondetail);
		}

		private void radio_position_Click(object sender, System.EventArgs e)
		{
			this.CheckChange(this.radio_position);
		}

		private void bgvPosition_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (this.bgvPosition.GetRowCellValue(e.RowHandle, this.bgvPosition.Columns["买卖"].ToString()).ToString() != "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (this.bgvPosition.GetRowCellValue(e.RowHandle, this.bgvPosition.Columns["买卖"].ToString()).ToString() == "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			if (e.Column.FieldName == "持仓盈亏")
			{
				double num = (double)this.bgvPosition.GetRowCellValue(e.RowHandle, this.bgvPosition.Columns["持仓盈亏"].ToString());
				if (System.Convert.ToDecimal((double.IsNaN(num) || double.IsInfinity(num)) ? 0.0 : num) < 0m)
				{
					e.Appearance.ForeColor = System.Drawing.Color.Green;
				}
				if (System.Convert.ToDecimal((double.IsNaN(num) || double.IsInfinity(num)) ? 0.0 : num) > 0m)
				{
					e.Appearance.ForeColor = System.Drawing.Color.Red;
				}
			}
		}

		private void bgvPosition_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
		{
			int num = System.Convert.ToInt32((e.Item as GridSummaryItem).Tag);
			GridView gridView = sender as GridView;
			if (e.SummaryProcess == CustomSummaryProcess.Start)
			{
				this.totalbposition = 0;
				this.totalsposition = 0;
				this.yBposition = 0;
				this.ySposition = 0;
				this.tBposition = 0;
				this.tSposition = 0;
			}
			if (e.SummaryProcess == CustomSummaryProcess.Calculate)
			{
				string a = (string)gridView.GetRowCellValue(e.RowHandle, "买卖");
				switch (num)
				{
				case 1:
					if (!(a == "买"))
					{
						this.totalsposition += System.Convert.ToInt32("-" + e.FieldValue);
					}
					else
					{
						this.totalbposition += System.Convert.ToInt32(e.FieldValue);
					}
					break;
				case 2:
					if (!(a == "买"))
					{
						this.ySposition += System.Convert.ToInt32("-" + e.FieldValue);
					}
					else
					{
						this.yBposition += System.Convert.ToInt32(e.FieldValue);
					}
					break;
				case 3:
					if (!(a == "买"))
					{
						this.tSposition += System.Convert.ToInt32("-" + e.FieldValue);
					}
					else
					{
						this.tBposition += System.Convert.ToInt32(e.FieldValue);
					}
					break;
				}
			}
			if (e.SummaryProcess == CustomSummaryProcess.Finalize)
			{
				switch (num)
				{
				case 1:
					e.TotalValue = this.totalbposition + "/" + this.totalsposition;
					break;
				case 2:
					e.TotalValue = this.yBposition + "/" + this.ySposition;
					break;
				case 3:
					e.TotalValue = this.tBposition + "/" + this.tSposition;
					break;
				}
			}
		}

		private void bgvPositionDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (e.RowHandle >= 0)
			{
				if (this.bgvPositionDetail.GetRowCellValue(e.RowHandle, this.bgvPositionDetail.Columns["买卖"].ToString()).ToString() != "买" && e.Column.FieldName == "买卖")
				{
					e.Appearance.ForeColor = System.Drawing.Color.Green;
				}
				if (this.bgvPositionDetail.GetRowCellValue(e.RowHandle, this.bgvPositionDetail.Columns["买卖"].ToString()).ToString() == "买" && e.Column.FieldName == "买卖")
				{
					e.Appearance.ForeColor = System.Drawing.Color.Red;
				}
			}
		}

		private void bgvPosition_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
		}

		private void bgvPosition_RowClick(object sender, RowClickEventArgs e)
		{
			System.Data.DataRow dataRow = this.bgvPosition.GetDataRow(this.bgvPosition.FocusedRowHandle);
			if (dataRow != null)
			{
				this.clv(0, dataRow);
			}
		}

		private void bgvPosition_RowStyle(object sender, RowStyleEventArgs e)
		{
		}

		private void bgvPosition_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (this.bgvPosition.GetRow(e.RowHandle) != null)
			{
				string a = this.bgvPosition.GetRowCellValue(e.RowHandle, "校对").ToString();
				if (a == "1")
				{
					e.Appearance.BackColor = System.Drawing.Color.LightPink;
				}
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
			StyleFormatCondition styleFormatCondition2 = new StyleFormatCondition();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmPosition));
			this.radio_position = new System.Windows.Forms.RadioButton();
			this.radio_positiondetail = new System.Windows.Forms.RadioButton();
			this.xdgPosition = new GridControl();
			this.bgvPosition = new BandedGridView();
			this.gridBand1 = new GridBand();
			this.colInstrument = new BandedGridColumn();
			this.colPosidirection = new BandedGridColumn();
			this.colTotalPosition = new BandedGridColumn();
			this.colYPosition = new BandedGridColumn();
			this.colTPosition = new BandedGridColumn();
			this.colYCanPosition = new BandedGridColumn();
			this.colTCanPosition = new BandedGridColumn();
			this.colPositionProfit = new BandedGridColumn();
			this.colShortUseMargin = new BandedGridColumn();
			this.colLongUseMargin = new BandedGridColumn();
			this.colPositionCost = new BandedGridColumn();
			this.colOpenCost = new BandedGridColumn();
			this.colHedgeFlag = new BandedGridColumn();
			this.bandedGridColumn1 = new BandedGridColumn();
			this.xdgPositionDetail = new GridControl();
			this.bgvPositionDetail = new BandedGridView();
			this.gridBand2 = new GridBand();
			this.bandedGridColumn8 = new BandedGridColumn();
			this.bandedGridColumn2 = new BandedGridColumn();
			this.bandedGridColumn3 = new BandedGridColumn();
			this.bandedGridColumn4 = new BandedGridColumn();
			this.bandedGridColumn5 = new BandedGridColumn();
			this.bandedGridColumn6 = new BandedGridColumn();
			this.bandedGridColumn7 = new BandedGridColumn();
			this.panel1 = new System.Windows.Forms.Panel();
			((ISupportInitialize)this.xdgPosition).BeginInit();
			((ISupportInitialize)this.bgvPosition).BeginInit();
			((ISupportInitialize)this.xdgPositionDetail).BeginInit();
			((ISupportInitialize)this.bgvPositionDetail).BeginInit();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.radio_position.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.radio_position.AutoSize = true;
			this.radio_position.Checked = true;
			this.radio_position.Location = new System.Drawing.Point(12, 9);
			this.radio_position.Name = "radio_position";
			this.radio_position.Size = new System.Drawing.Size(47, 16);
			this.radio_position.TabIndex = 3;
			this.radio_position.TabStop = true;
			this.radio_position.Text = "持仓";
			this.radio_position.UseVisualStyleBackColor = true;
			this.radio_position.Click += new System.EventHandler(this.radio_position_Click);
			this.radio_positiondetail.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.radio_positiondetail.AutoSize = true;
			this.radio_positiondetail.Location = new System.Drawing.Point(65, 9);
			this.radio_positiondetail.Name = "radio_positiondetail";
			this.radio_positiondetail.Size = new System.Drawing.Size(71, 16);
			this.radio_positiondetail.TabIndex = 4;
			this.radio_positiondetail.Text = "持仓明细";
			this.radio_positiondetail.UseVisualStyleBackColor = true;
			this.radio_positiondetail.Click += new System.EventHandler(this.radio_positiondetail_Click);
			this.xdgPosition.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.xdgPosition.Location = new System.Drawing.Point(0, 0);
			this.xdgPosition.MainView = this.bgvPosition;
			this.xdgPosition.Name = "xdgPosition";
			this.xdgPosition.Size = new System.Drawing.Size(889, 401);
			this.xdgPosition.TabIndex = 17;
			this.xdgPosition.ViewCollection.AddRange(new BaseView[]
			{
				this.bgvPosition
			});
			this.bgvPosition.Appearance.BandPanel.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPosition.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPosition.Appearance.BandPanel.BorderColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.BandPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.BandPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.BandPanel.Options.UseBackColor = true;
			this.bgvPosition.Appearance.BandPanel.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.BandPanel.Options.UseForeColor = true;
			this.bgvPosition.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPosition.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgvPosition.Appearance.BandPanelBackground.Options.UseBackColor = true;
			this.bgvPosition.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPosition.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPosition.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.bgvPosition.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.bgvPosition.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgvPosition.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgvPosition.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.bgvPosition.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.bgvPosition.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvPosition.Appearance.Empty.Options.UseBackColor = true;
			this.bgvPosition.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.bgvPosition.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.bgvPosition.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvPosition.Appearance.EvenRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.EvenRow.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.EvenRow.Options.UseForeColor = true;
			this.bgvPosition.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPosition.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPosition.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.bgvPosition.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.bgvPosition.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPosition.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.bgvPosition.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.FilterPanel.Options.UseBackColor = true;
			this.bgvPosition.Appearance.FilterPanel.Options.UseForeColor = true;
			this.bgvPosition.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.FixedLine.Options.UseBackColor = true;
			this.bgvPosition.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.bgvPosition.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.FocusedCell.Options.UseBackColor = true;
			this.bgvPosition.Appearance.FocusedCell.Options.UseForeColor = true;
			this.bgvPosition.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPosition.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvPosition.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.FocusedRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.FocusedRow.Options.UseForeColor = true;
			this.bgvPosition.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.bgvPosition.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPosition.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.bgvPosition.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.FooterPanel.Options.UseBackColor = true;
			this.bgvPosition.Appearance.FooterPanel.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.FooterPanel.Options.UseForeColor = true;
			this.bgvPosition.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPosition.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPosition.Appearance.GroupButton.Options.UseBackColor = true;
			this.bgvPosition.Appearance.GroupButton.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.GroupFooter.Options.UseBackColor = true;
			this.bgvPosition.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.GroupFooter.Options.UseForeColor = true;
			this.bgvPosition.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPosition.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.bgvPosition.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.GroupPanel.Options.UseBackColor = true;
			this.bgvPosition.Appearance.GroupPanel.Options.UseForeColor = true;
			this.bgvPosition.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.bgvPosition.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.bgvPosition.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.GroupRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.GroupRow.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.GroupRow.Options.UseForeColor = true;
			this.bgvPosition.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPosition.Appearance.HeaderPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgvPosition.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPosition.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvPosition.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.bgvPosition.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.HorzLine.Options.UseBackColor = true;
			this.bgvPosition.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.bgvPosition.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvPosition.Appearance.OddRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.OddRow.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.OddRow.Options.UseForeColor = true;
			this.bgvPosition.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.bgvPosition.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.bgvPosition.Appearance.Preview.Options.UseBackColor = true;
			this.bgvPosition.Appearance.Preview.Options.UseFont = true;
			this.bgvPosition.Appearance.Preview.Options.UseForeColor = true;
			this.bgvPosition.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPosition.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.Row.Options.UseBackColor = true;
			this.bgvPosition.Appearance.Row.Options.UseForeColor = true;
			this.bgvPosition.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPosition.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.bgvPosition.Appearance.RowSeparator.BorderColor = System.Drawing.Color.Transparent;
			this.bgvPosition.Appearance.RowSeparator.Options.UseBackColor = true;
			this.bgvPosition.Appearance.RowSeparator.Options.UseBorderColor = true;
			this.bgvPosition.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPosition.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvPosition.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPosition.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPosition.Appearance.SelectedRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.SelectedRow.Options.UseForeColor = true;
			this.bgvPosition.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.TopNewRow.Options.UseBackColor = true;
			this.bgvPosition.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.bgvPosition.Appearance.VertLine.Options.UseBackColor = true;
			this.bgvPosition.Bands.AddRange(new GridBand[]
			{
				this.gridBand1
			});
			this.bgvPosition.BorderStyle = BorderStyles.Simple;
			this.bgvPosition.Columns.AddRange(new BandedGridColumn[]
			{
				this.colInstrument,
				this.colPosidirection,
				this.colTotalPosition,
				this.colYPosition,
				this.colTPosition,
				this.colYCanPosition,
				this.colTCanPosition,
				this.colPositionCost,
				this.colOpenCost,
				this.colPositionProfit,
				this.colShortUseMargin,
				this.colLongUseMargin,
				this.colHedgeFlag,
				this.bandedGridColumn1
			});
			styleFormatCondition.ApplyToRow = true;
			styleFormatCondition.Condition = FormatConditionEnum.Equal;
			styleFormatCondition.Value1 = true;
			this.bgvPosition.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition
			});
			this.bgvPosition.GridControl = this.xdgPosition;
			this.bgvPosition.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.bgvPosition.GroupSummary.AddRange(new GridSummaryItem[]
			{
				new GridGroupSummaryItem(SummaryItemType.Custom, "总持仓", this.colTotalPosition, "", "1"),
				new GridGroupSummaryItem(SummaryItemType.Custom, "昨仓", this.colYPosition, "", "2"),
				new GridGroupSummaryItem(SummaryItemType.Custom, "今仓", this.colTPosition, "", "3"),
				new GridGroupSummaryItem(SummaryItemType.Sum, "持仓盈亏", this.colPositionProfit, "", "4"),
				new GridGroupSummaryItem(SummaryItemType.Sum, "空头占用保证金", this.colShortUseMargin, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "多头占用保证金", this.colLongUseMargin, "")
			});
			this.bgvPosition.HorzScrollVisibility = ScrollVisibility.Always;
			this.bgvPosition.Name = "bgvPosition";
			this.bgvPosition.OptionsCustomization.AllowFilter = false;
			this.bgvPosition.OptionsMenu.EnableColumnMenu = false;
			this.bgvPosition.OptionsMenu.EnableFooterMenu = false;
			this.bgvPosition.OptionsPrint.AutoWidth = false;
			this.bgvPosition.OptionsView.ColumnAutoWidth = false;
			this.bgvPosition.OptionsView.EnableAppearanceEvenRow = true;
			this.bgvPosition.OptionsView.EnableAppearanceOddRow = true;
			this.bgvPosition.OptionsView.ShowFooter = true;
			this.bgvPosition.OptionsView.ShowGroupPanel = false;
			this.bgvPosition.VertScrollVisibility = ScrollVisibility.Always;
			this.bgvPosition.RowClick += new RowClickEventHandler(this.bgvPosition_RowClick);
			this.bgvPosition.CustomDrawCell += new RowCellCustomDrawEventHandler(this.bgvPosition_CustomDrawCell);
			this.bgvPosition.RowCellStyle += new RowCellStyleEventHandler(this.bgvPosition_RowCellStyle);
			this.bgvPosition.RowStyle += new RowStyleEventHandler(this.bgvPosition_RowStyle);
			this.bgvPosition.CustomSummaryCalculate += new CustomSummaryEventHandler(this.bgvPosition_CustomSummaryCalculate);
			this.bgvPosition.FocusedRowChanged += new FocusedRowChangedEventHandler(this.bgvPosition_FocusedRowChanged);
			this.gridBand1.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gridBand1.AppearanceHeader.BackColor2 = System.Drawing.Color.Silver;
			this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.gridBand1.AppearanceHeader.Options.UseBackColor = true;
			this.gridBand1.AppearanceHeader.Options.UseFont = true;
			this.gridBand1.Caption = "持仓";
			this.gridBand1.Columns.Add(this.colInstrument);
			this.gridBand1.Columns.Add(this.colPosidirection);
			this.gridBand1.Columns.Add(this.colTotalPosition);
			this.gridBand1.Columns.Add(this.colYPosition);
			this.gridBand1.Columns.Add(this.colTPosition);
			this.gridBand1.Columns.Add(this.colYCanPosition);
			this.gridBand1.Columns.Add(this.colTCanPosition);
			this.gridBand1.Columns.Add(this.colPositionProfit);
			this.gridBand1.Columns.Add(this.colShortUseMargin);
			this.gridBand1.Columns.Add(this.colLongUseMargin);
			this.gridBand1.Columns.Add(this.colPositionCost);
			this.gridBand1.Columns.Add(this.colOpenCost);
			this.gridBand1.Columns.Add(this.colHedgeFlag);
			this.gridBand1.Columns.Add(this.bandedGridColumn1);
			this.gridBand1.Name = "gridBand1";
			this.gridBand1.Width = 1127;
			this.colInstrument.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
			this.colInstrument.AppearanceCell.Options.UseFont = true;
			this.colInstrument.Caption = "合约";
			this.colInstrument.FieldName = "合约";
			this.colInstrument.Name = "colInstrument";
			this.colInstrument.OptionsColumn.AllowEdit = false;
			this.colInstrument.Visible = true;
			this.colInstrument.Width = 107;
			this.colPosidirection.Caption = "买卖";
			this.colPosidirection.FieldName = "买卖";
			this.colPosidirection.Name = "colPosidirection";
			this.colPosidirection.OptionsColumn.AllowEdit = false;
			this.colPosidirection.Visible = true;
			this.colTotalPosition.Caption = "总持仓";
			this.colTotalPosition.DisplayFormat.FormatType = FormatType.Numeric;
			this.colTotalPosition.FieldName = "总持仓";
			this.colTotalPosition.Name = "colTotalPosition";
			this.colTotalPosition.OptionsColumn.AllowEdit = false;
			this.colTotalPosition.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "总持仓", "", "1")
			});
			this.colTotalPosition.Visible = true;
			this.colYPosition.Caption = "昨仓";
			this.colYPosition.FieldName = "昨仓";
			this.colYPosition.GroupFormat.FormatType = FormatType.Numeric;
			this.colYPosition.Name = "colYPosition";
			this.colYPosition.OptionsColumn.AllowEdit = false;
			this.colYPosition.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "昨仓", "", "2")
			});
			this.colYPosition.Visible = true;
			this.colTPosition.Caption = "今仓";
			this.colTPosition.FieldName = "今仓";
			this.colTPosition.GroupFormat.FormatType = FormatType.Numeric;
			this.colTPosition.Name = "colTPosition";
			this.colTPosition.OptionsColumn.AllowEdit = false;
			this.colTPosition.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "今仓", "", "2")
			});
			this.colTPosition.Visible = true;
			this.colYCanPosition.Caption = "可平昨";
			this.colYCanPosition.FieldName = "可平昨";
			this.colYCanPosition.GroupFormat.FormatType = FormatType.Numeric;
			this.colYCanPosition.Name = "colYCanPosition";
			this.colYCanPosition.OptionsColumn.AllowEdit = false;
			this.colYCanPosition.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colYCanPosition.Width = 40;
			this.colTCanPosition.Caption = "可平今";
			this.colTCanPosition.FieldName = "可平今";
			this.colTCanPosition.GroupFormat.FormatType = FormatType.Numeric;
			this.colTCanPosition.Name = "colTCanPosition";
			this.colTCanPosition.OptionsColumn.AllowEdit = false;
			this.colTCanPosition.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colTCanPosition.Width = 40;
			this.colPositionProfit.Caption = "持仓盈亏";
			this.colPositionProfit.FieldName = "持仓盈亏";
			this.colPositionProfit.GroupFormat.FormatType = FormatType.Numeric;
			this.colPositionProfit.Name = "colPositionProfit";
			this.colPositionProfit.OptionsColumn.AllowEdit = false;
			this.colPositionProfit.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colPositionProfit.Visible = true;
			this.colPositionProfit.Width = 107;
			this.colShortUseMargin.Caption = "空头占用保证金";
			this.colShortUseMargin.FieldName = "空头占用保证金";
			this.colShortUseMargin.GroupFormat.FormatType = FormatType.Numeric;
			this.colShortUseMargin.Name = "colShortUseMargin";
			this.colShortUseMargin.OptionsColumn.AllowEdit = false;
			this.colShortUseMargin.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colShortUseMargin.Visible = true;
			this.colShortUseMargin.Width = 119;
			this.colLongUseMargin.Caption = "多头占用保证金";
			this.colLongUseMargin.FieldName = "多头占用保证金";
			this.colLongUseMargin.GroupFormat.FormatType = FormatType.Numeric;
			this.colLongUseMargin.Name = "colLongUseMargin";
			this.colLongUseMargin.OptionsColumn.AllowEdit = false;
			this.colLongUseMargin.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colLongUseMargin.Visible = true;
			this.colLongUseMargin.Width = 113;
			this.colPositionCost.Caption = "持仓均价";
			this.colPositionCost.FieldName = "持仓均价";
			this.colPositionCost.GroupFormat.FormatType = FormatType.Numeric;
			this.colPositionCost.Name = "colPositionCost";
			this.colPositionCost.OptionsColumn.AllowEdit = false;
			this.colPositionCost.Visible = true;
			this.colPositionCost.Width = 138;
			this.colOpenCost.Caption = "开仓均价";
			this.colOpenCost.FieldName = "开仓均价";
			this.colOpenCost.GroupFormat.FormatType = FormatType.Numeric;
			this.colOpenCost.Name = "colOpenCost";
			this.colOpenCost.OptionsColumn.AllowEdit = false;
			this.colOpenCost.Visible = true;
			this.colOpenCost.Width = 144;
			this.colHedgeFlag.AppearanceCell.Options.UseTextOptions = true;
			this.colHedgeFlag.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colHedgeFlag.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
			this.colHedgeFlag.Caption = "投保";
			this.colHedgeFlag.FieldName = "投保";
			this.colHedgeFlag.GroupFormat.FormatType = FormatType.Numeric;
			this.colHedgeFlag.Name = "colHedgeFlag";
			this.colHedgeFlag.OptionsColumn.AllowEdit = false;
			this.colHedgeFlag.Visible = true;
			this.colHedgeFlag.Width = 99;
			this.bandedGridColumn1.Caption = "校对";
			this.bandedGridColumn1.FieldName = "校对";
			this.bandedGridColumn1.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn1.Name = "bandedGridColumn1";
			this.xdgPositionDetail.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.xdgPositionDetail.Location = new System.Drawing.Point(0, 0);
			this.xdgPositionDetail.MainView = this.bgvPositionDetail;
			this.xdgPositionDetail.Name = "xdgPositionDetail";
			this.xdgPositionDetail.Size = new System.Drawing.Size(889, 401);
			this.xdgPositionDetail.TabIndex = 18;
			this.xdgPositionDetail.ViewCollection.AddRange(new BaseView[]
			{
				this.bgvPositionDetail
			});
			this.xdgPositionDetail.Visible = false;
			this.bgvPositionDetail.Appearance.BandPanel.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPositionDetail.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPositionDetail.Appearance.BandPanel.BorderColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.BandPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.BandPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.BandPanel.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.BandPanel.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.BandPanel.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPositionDetail.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.BandPanelBackground.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPositionDetail.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgvPositionDetail.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgvPositionDetail.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvPositionDetail.Appearance.Empty.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.bgvPositionDetail.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.bgvPositionDetail.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvPositionDetail.Appearance.EvenRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.EvenRow.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.EvenRow.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPositionDetail.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPositionDetail.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPositionDetail.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.FilterPanel.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.FilterPanel.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.FixedLine.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.FocusedCell.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.FocusedCell.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPositionDetail.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPositionDetail.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.FocusedRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.FocusedRow.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.bgvPositionDetail.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPositionDetail.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.bgvPositionDetail.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.FooterPanel.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.FooterPanel.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.FooterPanel.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPositionDetail.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvPositionDetail.Appearance.GroupButton.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.GroupButton.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.GroupFooter.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.GroupFooter.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPositionDetail.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.GroupPanel.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.GroupPanel.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.bgvPositionDetail.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.bgvPositionDetail.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.GroupRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.GroupRow.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.GroupRow.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPositionDetail.Appearance.HeaderPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPositionDetail.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvPositionDetail.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.SkyBlue;
			this.bgvPositionDetail.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPositionDetail.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.HorzLine.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.bgvPositionDetail.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvPositionDetail.Appearance.OddRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.OddRow.Options.UseBorderColor = true;
			this.bgvPositionDetail.Appearance.OddRow.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.bgvPositionDetail.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.bgvPositionDetail.Appearance.Preview.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.Preview.Options.UseFont = true;
			this.bgvPositionDetail.Appearance.Preview.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvPositionDetail.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.bgvPositionDetail.Appearance.Row.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.Row.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvPositionDetail.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.RowSeparator.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvPositionDetail.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.SteelBlue;
			this.bgvPositionDetail.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvPositionDetail.Appearance.SelectedRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.SelectedRow.Options.UseForeColor = true;
			this.bgvPositionDetail.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.TopNewRow.Options.UseBackColor = true;
			this.bgvPositionDetail.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.bgvPositionDetail.Appearance.VertLine.Options.UseBackColor = true;
			this.bgvPositionDetail.Bands.AddRange(new GridBand[]
			{
				this.gridBand2
			});
			this.bgvPositionDetail.BorderStyle = BorderStyles.Simple;
			this.bgvPositionDetail.Columns.AddRange(new BandedGridColumn[]
			{
				this.bandedGridColumn8,
				this.bandedGridColumn2,
				this.bandedGridColumn3,
				this.bandedGridColumn4,
				this.bandedGridColumn5,
				this.bandedGridColumn6,
				this.bandedGridColumn7
			});
			styleFormatCondition2.ApplyToRow = true;
			styleFormatCondition2.Condition = FormatConditionEnum.Equal;
			styleFormatCondition2.Value1 = true;
			this.bgvPositionDetail.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition2
			});
			this.bgvPositionDetail.GridControl = this.xdgPositionDetail;
			this.bgvPositionDetail.HorzScrollVisibility = ScrollVisibility.Always;
			this.bgvPositionDetail.Name = "bgvPositionDetail";
			this.bgvPositionDetail.OptionsBehavior.AutoExpandAllGroups = true;
			this.bgvPositionDetail.OptionsCustomization.AllowFilter = false;
			this.bgvPositionDetail.OptionsMenu.EnableColumnMenu = false;
			this.bgvPositionDetail.OptionsMenu.EnableFooterMenu = false;
			this.bgvPositionDetail.OptionsPrint.AutoWidth = false;
			this.bgvPositionDetail.OptionsView.ColumnAutoWidth = false;
			this.bgvPositionDetail.OptionsView.EnableAppearanceEvenRow = true;
			this.bgvPositionDetail.OptionsView.EnableAppearanceOddRow = true;
			this.bgvPositionDetail.OptionsView.ShowGroupPanel = false;
			this.bgvPositionDetail.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.bandedGridColumn8, ColumnSortOrder.Descending),
				new GridColumnSortInfo(this.bandedGridColumn7, ColumnSortOrder.Descending)
			});
			this.bgvPositionDetail.VertScrollVisibility = ScrollVisibility.Always;
			this.bgvPositionDetail.RowCellStyle += new RowCellStyleEventHandler(this.bgvPositionDetail_RowCellStyle);
			this.gridBand2.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gridBand2.AppearanceHeader.BackColor2 = System.Drawing.Color.Silver;
			this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.gridBand2.AppearanceHeader.Options.UseBackColor = true;
			this.gridBand2.AppearanceHeader.Options.UseFont = true;
			this.gridBand2.Caption = "持仓明细";
			this.gridBand2.Columns.Add(this.bandedGridColumn8);
			this.gridBand2.Columns.Add(this.bandedGridColumn2);
			this.gridBand2.Columns.Add(this.bandedGridColumn3);
			this.gridBand2.Columns.Add(this.bandedGridColumn4);
			this.gridBand2.Columns.Add(this.bandedGridColumn5);
			this.gridBand2.Columns.Add(this.bandedGridColumn6);
			this.gridBand2.Columns.Add(this.bandedGridColumn7);
			this.gridBand2.Name = "gridBand2";
			this.gridBand2.Width = 1035;
			this.bandedGridColumn8.Caption = "编号";
			this.bandedGridColumn8.DisplayFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn8.FieldName = "编号";
			this.bandedGridColumn8.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn8.Name = "bandedGridColumn8";
			this.bandedGridColumn8.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn8.Visible = true;
			this.bandedGridColumn8.Width = 149;
			this.bandedGridColumn2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
			this.bandedGridColumn2.AppearanceCell.Options.UseFont = true;
			this.bandedGridColumn2.Caption = "合约";
			this.bandedGridColumn2.FieldName = "合约";
			this.bandedGridColumn2.Name = "bandedGridColumn2";
			this.bandedGridColumn2.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn2.Visible = true;
			this.bandedGridColumn2.Width = 181;
			this.bandedGridColumn3.Caption = "买卖";
			this.bandedGridColumn3.FieldName = "买卖";
			this.bandedGridColumn3.Name = "bandedGridColumn3";
			this.bandedGridColumn3.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn3.Visible = true;
			this.bandedGridColumn3.Width = 134;
			this.bandedGridColumn4.Caption = "持仓类型";
			this.bandedGridColumn4.DisplayFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn4.FieldName = "持仓类型";
			this.bandedGridColumn4.Name = "bandedGridColumn4";
			this.bandedGridColumn4.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn4.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom)
			});
			this.bandedGridColumn4.Visible = true;
			this.bandedGridColumn4.Width = 134;
			this.bandedGridColumn5.Caption = "成交手数";
			this.bandedGridColumn5.FieldName = "成交手数";
			this.bandedGridColumn5.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn5.Name = "bandedGridColumn5";
			this.bandedGridColumn5.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn5.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.bandedGridColumn5.Visible = true;
			this.bandedGridColumn5.Width = 134;
			this.bandedGridColumn6.Caption = "成交价格";
			this.bandedGridColumn6.FieldName = "成交价格";
			this.bandedGridColumn6.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn6.Name = "bandedGridColumn6";
			this.bandedGridColumn6.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn6.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.bandedGridColumn6.Visible = true;
			this.bandedGridColumn6.Width = 149;
			this.bandedGridColumn7.Caption = "成交时间";
			this.bandedGridColumn7.FieldName = "成交时间";
			this.bandedGridColumn7.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn7.Name = "bandedGridColumn7";
			this.bandedGridColumn7.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn7.Visible = true;
			this.bandedGridColumn7.Width = 154;
			this.panel1.Controls.Add(this.radio_position);
			this.panel1.Controls.Add(this.radio_positiondetail);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 401);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(889, 28);
			this.panel1.TabIndex = 19;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(235, 236, 239);
			base.ClientSize = new System.Drawing.Size(889, 429);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.xdgPosition);
			base.Controls.Add(this.xdgPositionDetail);
			this.DoubleBuffered = true;
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "FrmPosition";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPosition_FormClosing);
			base.Load += new System.EventHandler(this.FrmPosition_Load);
			((ISupportInitialize)this.xdgPosition).EndInit();
			((ISupportInitialize)this.bgvPosition).EndInit();
			((ISupportInitialize)this.xdgPositionDetail).EndInit();
			((ISupportInitialize)this.bgvPositionDetail).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}

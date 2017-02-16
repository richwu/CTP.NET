namespace WinCtp
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslMdStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tpMstOrder = new System.Windows.Forms.TabPage();
            this.gvMstOrder = new WinCtp.DataGridViewEx();
            this.investorIdDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderSysIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instrumentIdDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directionDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.combOffsetFlagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeTotalOriginalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeTotalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeTradedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.insertTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsMstOrder = new System.Windows.Forms.BindingSource(this.components);
            this.tpMstTrade = new System.Windows.Forms.TabPage();
            this.dataGridViewEx2 = new WinCtp.DataGridViewEx();
            this.investorIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instrumentIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcMstTradeDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gcMstTradeOffsetFlag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.priceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeTimeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderLocalIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsMstTradeInfo = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tcMstInstrument = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbMstUser = new System.Windows.Forms.GroupBox();
            this.gvMstUser = new WinCtp.DataGridViewEx();
            this.isCheckedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.userIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isLoginDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmsMstUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSelectAllMstUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMstUserLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMstUserLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRePosiMst = new System.Windows.Forms.ToolStripMenuItem();
            this.dsMstUser = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbSubUser = new System.Windows.Forms.GroupBox();
            this.gvSubUser = new WinCtp.DataGridViewEx();
            this.isCheckedDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.userIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isLoginDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gcSubUserSettlementInfoConfirmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsSubUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSelectAllSubUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSubUserLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSubUserLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettlementInfoConfirm = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRePosiSub = new System.Windows.Forms.ToolStripMenuItem();
            this.dsSubUser = new System.Windows.Forms.BindingSource(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnInsertOrder = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cmbOffsetFlag = new System.Windows.Forms.ComboBox();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.numVolume = new System.Windows.Forms.NumericUpDown();
            this.cmbInstrumentId = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tpSubOrder = new System.Windows.Forms.TabPage();
            this.gvSubOrder = new WinCtp.DataGridViewEx();
            this.investorIdDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderSysIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instrumentIdDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcSubOrderDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gcSubOrderCombOffsetFlag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gcSubOrderStatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.limitPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeTotalOriginalDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeTotalDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeTradedDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.insertTimeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateTimeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcSubOrderErrorMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsSubOrder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCancelOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.dsSubOrder = new System.Windows.Forms.BindingSource(this.components);
            this.tpSubTrade = new System.Windows.Forms.TabPage();
            this.dataGridViewEx1 = new WinCtp.DataGridViewEx();
            this.investorIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instrumentIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcSubTradeDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gcSubTradeOffsetFlag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradeTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderLocalIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsSubTradeInfo = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tcSubInstrument = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsmiUser = new System.Windows.Forms.ToolStripDropDownButton();
            this.ibtnUser = new System.Windows.Forms.ToolStripMenuItem();
            this.ibtnOrderInsertConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.ibtnBroker = new System.Windows.Forms.ToolStripButton();
            this.tsmiListen = new System.Windows.Forms.ToolStripButton();
            this.ibtnSetting = new System.Windows.Forms.ToolStripButton();
            this.ibtnClose = new System.Windows.Forms.ToolStripButton();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tpMstOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMstOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMstOrder)).BeginInit();
            this.tpMstTrade.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMstTradeInfo)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tcMstInstrument.SuspendLayout();
            this.gbMstUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMstUser)).BeginInit();
            this.cmsMstUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsMstUser)).BeginInit();
            this.panel2.SuspendLayout();
            this.gbSubUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubUser)).BeginInit();
            this.cmsSubUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsSubUser)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).BeginInit();
            this.panel3.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tpSubOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubOrder)).BeginInit();
            this.cmsSubOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsSubOrder)).BeginInit();
            this.tpSubTrade.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsSubTradeInfo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tcSubInstrument.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslMdStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 709);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1190, 22);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsslMdStatus
            // 
            this.tsslMdStatus.ForeColor = System.Drawing.Color.Gray;
            this.tsslMdStatus.Name = "tsslMdStatus";
            this.tsslMdStatus.Size = new System.Drawing.Size(32, 17);
            this.tsslMdStatus.Text = "行情";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl2);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.gbMstUser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1190, 204);
            this.panel1.TabIndex = 14;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tpMstOrder);
            this.tabControl2.Controls.Add(this.tpMstTrade);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(586, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(604, 204);
            this.tabControl2.TabIndex = 2;
            // 
            // tpMstOrder
            // 
            this.tpMstOrder.Controls.Add(this.gvMstOrder);
            this.tpMstOrder.Location = new System.Drawing.Point(4, 22);
            this.tpMstOrder.Name = "tpMstOrder";
            this.tpMstOrder.Size = new System.Drawing.Size(596, 178);
            this.tpMstOrder.TabIndex = 2;
            this.tpMstOrder.Text = "委托单";
            this.tpMstOrder.UseVisualStyleBackColor = true;
            // 
            // gvMstOrder
            // 
            this.gvMstOrder.AllowUserToAddRows = false;
            this.gvMstOrder.AllowUserToDeleteRows = false;
            this.gvMstOrder.AutoGenerateColumns = false;
            this.gvMstOrder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvMstOrder.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvMstOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMstOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.investorIdDataGridViewTextBoxColumn3,
            this.orderSysIdDataGridViewTextBoxColumn,
            this.instrumentIdDataGridViewTextBoxColumn3,
            this.directionDataGridViewTextBoxColumn3,
            this.combOffsetFlagDataGridViewTextBoxColumn,
            this.orderStatusDataGridViewTextBoxColumn,
            this.volumeTotalOriginalDataGridViewTextBoxColumn,
            this.volumeTotalDataGridViewTextBoxColumn,
            this.volumeTradedDataGridViewTextBoxColumn,
            this.insertTimeDataGridViewTextBoxColumn,
            this.updateTimeDataGridViewTextBoxColumn});
            this.gvMstOrder.DataSource = this.dsMstOrder;
            this.gvMstOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMstOrder.Location = new System.Drawing.Point(0, 0);
            this.gvMstOrder.Name = "gvMstOrder";
            this.gvMstOrder.ReadOnly = true;
            this.gvMstOrder.RowTemplate.Height = 23;
            this.gvMstOrder.Size = new System.Drawing.Size(596, 178);
            this.gvMstOrder.TabIndex = 0;
            // 
            // investorIdDataGridViewTextBoxColumn3
            // 
            this.investorIdDataGridViewTextBoxColumn3.DataPropertyName = "InvestorId";
            this.investorIdDataGridViewTextBoxColumn3.HeaderText = "投资者";
            this.investorIdDataGridViewTextBoxColumn3.Name = "investorIdDataGridViewTextBoxColumn3";
            this.investorIdDataGridViewTextBoxColumn3.ReadOnly = true;
            this.investorIdDataGridViewTextBoxColumn3.Width = 66;
            // 
            // orderSysIdDataGridViewTextBoxColumn
            // 
            this.orderSysIdDataGridViewTextBoxColumn.DataPropertyName = "OrderSysId";
            this.orderSysIdDataGridViewTextBoxColumn.HeaderText = "编号";
            this.orderSysIdDataGridViewTextBoxColumn.Name = "orderSysIdDataGridViewTextBoxColumn";
            this.orderSysIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderSysIdDataGridViewTextBoxColumn.Width = 54;
            // 
            // instrumentIdDataGridViewTextBoxColumn3
            // 
            this.instrumentIdDataGridViewTextBoxColumn3.DataPropertyName = "InstrumentId";
            this.instrumentIdDataGridViewTextBoxColumn3.HeaderText = "合约";
            this.instrumentIdDataGridViewTextBoxColumn3.Name = "instrumentIdDataGridViewTextBoxColumn3";
            this.instrumentIdDataGridViewTextBoxColumn3.ReadOnly = true;
            this.instrumentIdDataGridViewTextBoxColumn3.Width = 54;
            // 
            // directionDataGridViewTextBoxColumn3
            // 
            this.directionDataGridViewTextBoxColumn3.DataPropertyName = "Direction";
            this.directionDataGridViewTextBoxColumn3.HeaderText = "买卖";
            this.directionDataGridViewTextBoxColumn3.Name = "directionDataGridViewTextBoxColumn3";
            this.directionDataGridViewTextBoxColumn3.ReadOnly = true;
            this.directionDataGridViewTextBoxColumn3.Width = 54;
            // 
            // combOffsetFlagDataGridViewTextBoxColumn
            // 
            this.combOffsetFlagDataGridViewTextBoxColumn.DataPropertyName = "CombOffsetFlag";
            this.combOffsetFlagDataGridViewTextBoxColumn.HeaderText = "开平";
            this.combOffsetFlagDataGridViewTextBoxColumn.Name = "combOffsetFlagDataGridViewTextBoxColumn";
            this.combOffsetFlagDataGridViewTextBoxColumn.ReadOnly = true;
            this.combOffsetFlagDataGridViewTextBoxColumn.Width = 54;
            // 
            // orderStatusDataGridViewTextBoxColumn
            // 
            this.orderStatusDataGridViewTextBoxColumn.DataPropertyName = "OrderStatus";
            this.orderStatusDataGridViewTextBoxColumn.HeaderText = "状态";
            this.orderStatusDataGridViewTextBoxColumn.Name = "orderStatusDataGridViewTextBoxColumn";
            this.orderStatusDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderStatusDataGridViewTextBoxColumn.Width = 54;
            // 
            // volumeTotalOriginalDataGridViewTextBoxColumn
            // 
            this.volumeTotalOriginalDataGridViewTextBoxColumn.DataPropertyName = "VolumeTotalOriginal";
            this.volumeTotalOriginalDataGridViewTextBoxColumn.HeaderText = "报单手数";
            this.volumeTotalOriginalDataGridViewTextBoxColumn.Name = "volumeTotalOriginalDataGridViewTextBoxColumn";
            this.volumeTotalOriginalDataGridViewTextBoxColumn.ReadOnly = true;
            this.volumeTotalOriginalDataGridViewTextBoxColumn.Width = 78;
            // 
            // volumeTotalDataGridViewTextBoxColumn
            // 
            this.volumeTotalDataGridViewTextBoxColumn.DataPropertyName = "VolumeTotal";
            this.volumeTotalDataGridViewTextBoxColumn.HeaderText = "未成交";
            this.volumeTotalDataGridViewTextBoxColumn.Name = "volumeTotalDataGridViewTextBoxColumn";
            this.volumeTotalDataGridViewTextBoxColumn.ReadOnly = true;
            this.volumeTotalDataGridViewTextBoxColumn.Width = 66;
            // 
            // volumeTradedDataGridViewTextBoxColumn
            // 
            this.volumeTradedDataGridViewTextBoxColumn.DataPropertyName = "VolumeTraded";
            this.volumeTradedDataGridViewTextBoxColumn.HeaderText = "成交手数";
            this.volumeTradedDataGridViewTextBoxColumn.Name = "volumeTradedDataGridViewTextBoxColumn";
            this.volumeTradedDataGridViewTextBoxColumn.ReadOnly = true;
            this.volumeTradedDataGridViewTextBoxColumn.Width = 78;
            // 
            // insertTimeDataGridViewTextBoxColumn
            // 
            this.insertTimeDataGridViewTextBoxColumn.DataPropertyName = "InsertTime";
            this.insertTimeDataGridViewTextBoxColumn.HeaderText = "报单时间";
            this.insertTimeDataGridViewTextBoxColumn.Name = "insertTimeDataGridViewTextBoxColumn";
            this.insertTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.insertTimeDataGridViewTextBoxColumn.Width = 78;
            // 
            // updateTimeDataGridViewTextBoxColumn
            // 
            this.updateTimeDataGridViewTextBoxColumn.DataPropertyName = "UpdateTime";
            this.updateTimeDataGridViewTextBoxColumn.HeaderText = "成交时间";
            this.updateTimeDataGridViewTextBoxColumn.Name = "updateTimeDataGridViewTextBoxColumn";
            this.updateTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.updateTimeDataGridViewTextBoxColumn.Width = 78;
            // 
            // dsMstOrder
            // 
            this.dsMstOrder.DataSource = typeof(WinCtp.OrderInfo);
            // 
            // tpMstTrade
            // 
            this.tpMstTrade.Controls.Add(this.dataGridViewEx2);
            this.tpMstTrade.Location = new System.Drawing.Point(4, 22);
            this.tpMstTrade.Name = "tpMstTrade";
            this.tpMstTrade.Padding = new System.Windows.Forms.Padding(3);
            this.tpMstTrade.Size = new System.Drawing.Size(596, 178);
            this.tpMstTrade.TabIndex = 1;
            this.tpMstTrade.Text = "成交记录";
            this.tpMstTrade.UseVisualStyleBackColor = true;
            // 
            // dataGridViewEx2
            // 
            this.dataGridViewEx2.AllowUserToAddRows = false;
            this.dataGridViewEx2.AllowUserToDeleteRows = false;
            this.dataGridViewEx2.AutoGenerateColumns = false;
            this.dataGridViewEx2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewEx2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewEx2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEx2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.investorIdDataGridViewTextBoxColumn1,
            this.tradeIdDataGridViewTextBoxColumn1,
            this.instrumentIdDataGridViewTextBoxColumn1,
            this.gcMstTradeDirection,
            this.gcMstTradeOffsetFlag,
            this.priceDataGridViewTextBoxColumn1,
            this.volumeDataGridViewTextBoxColumn1,
            this.tradeTimeDataGridViewTextBoxColumn1,
            this.orderLocalIdDataGridViewTextBoxColumn1,
            this.exchangeIdDataGridViewTextBoxColumn1});
            this.dataGridViewEx2.DataSource = this.dsMstTradeInfo;
            this.dataGridViewEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEx2.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewEx2.Name = "dataGridViewEx2";
            this.dataGridViewEx2.ReadOnly = true;
            this.dataGridViewEx2.RowTemplate.Height = 23;
            this.dataGridViewEx2.Size = new System.Drawing.Size(590, 172);
            this.dataGridViewEx2.TabIndex = 0;
            // 
            // investorIdDataGridViewTextBoxColumn1
            // 
            this.investorIdDataGridViewTextBoxColumn1.DataPropertyName = "InvestorId";
            this.investorIdDataGridViewTextBoxColumn1.HeaderText = "投资者";
            this.investorIdDataGridViewTextBoxColumn1.Name = "investorIdDataGridViewTextBoxColumn1";
            this.investorIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.investorIdDataGridViewTextBoxColumn1.Width = 66;
            // 
            // tradeIdDataGridViewTextBoxColumn1
            // 
            this.tradeIdDataGridViewTextBoxColumn1.DataPropertyName = "TradeId";
            this.tradeIdDataGridViewTextBoxColumn1.HeaderText = "编号";
            this.tradeIdDataGridViewTextBoxColumn1.Name = "tradeIdDataGridViewTextBoxColumn1";
            this.tradeIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.tradeIdDataGridViewTextBoxColumn1.Width = 54;
            // 
            // instrumentIdDataGridViewTextBoxColumn1
            // 
            this.instrumentIdDataGridViewTextBoxColumn1.DataPropertyName = "InstrumentId";
            this.instrumentIdDataGridViewTextBoxColumn1.HeaderText = "合约";
            this.instrumentIdDataGridViewTextBoxColumn1.Name = "instrumentIdDataGridViewTextBoxColumn1";
            this.instrumentIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.instrumentIdDataGridViewTextBoxColumn1.Width = 54;
            // 
            // gcMstTradeDirection
            // 
            this.gcMstTradeDirection.DataPropertyName = "Direction";
            this.gcMstTradeDirection.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcMstTradeDirection.HeaderText = "买卖";
            this.gcMstTradeDirection.Name = "gcMstTradeDirection";
            this.gcMstTradeDirection.ReadOnly = true;
            this.gcMstTradeDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcMstTradeDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcMstTradeDirection.Width = 54;
            // 
            // gcMstTradeOffsetFlag
            // 
            this.gcMstTradeOffsetFlag.DataPropertyName = "OffsetFlag";
            this.gcMstTradeOffsetFlag.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcMstTradeOffsetFlag.HeaderText = "开平";
            this.gcMstTradeOffsetFlag.Name = "gcMstTradeOffsetFlag";
            this.gcMstTradeOffsetFlag.ReadOnly = true;
            this.gcMstTradeOffsetFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcMstTradeOffsetFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcMstTradeOffsetFlag.Width = 54;
            // 
            // priceDataGridViewTextBoxColumn1
            // 
            this.priceDataGridViewTextBoxColumn1.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn1.HeaderText = "成交价格";
            this.priceDataGridViewTextBoxColumn1.Name = "priceDataGridViewTextBoxColumn1";
            this.priceDataGridViewTextBoxColumn1.ReadOnly = true;
            this.priceDataGridViewTextBoxColumn1.Width = 78;
            // 
            // volumeDataGridViewTextBoxColumn1
            // 
            this.volumeDataGridViewTextBoxColumn1.DataPropertyName = "Volume";
            this.volumeDataGridViewTextBoxColumn1.HeaderText = "成交手数";
            this.volumeDataGridViewTextBoxColumn1.Name = "volumeDataGridViewTextBoxColumn1";
            this.volumeDataGridViewTextBoxColumn1.ReadOnly = true;
            this.volumeDataGridViewTextBoxColumn1.Width = 78;
            // 
            // tradeTimeDataGridViewTextBoxColumn1
            // 
            this.tradeTimeDataGridViewTextBoxColumn1.DataPropertyName = "TradeTime";
            this.tradeTimeDataGridViewTextBoxColumn1.HeaderText = "成交时间";
            this.tradeTimeDataGridViewTextBoxColumn1.Name = "tradeTimeDataGridViewTextBoxColumn1";
            this.tradeTimeDataGridViewTextBoxColumn1.ReadOnly = true;
            this.tradeTimeDataGridViewTextBoxColumn1.Width = 78;
            // 
            // orderLocalIdDataGridViewTextBoxColumn1
            // 
            this.orderLocalIdDataGridViewTextBoxColumn1.DataPropertyName = "OrderLocalId";
            this.orderLocalIdDataGridViewTextBoxColumn1.HeaderText = "报单编号";
            this.orderLocalIdDataGridViewTextBoxColumn1.Name = "orderLocalIdDataGridViewTextBoxColumn1";
            this.orderLocalIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.orderLocalIdDataGridViewTextBoxColumn1.Width = 78;
            // 
            // exchangeIdDataGridViewTextBoxColumn1
            // 
            this.exchangeIdDataGridViewTextBoxColumn1.DataPropertyName = "ExchangeId";
            this.exchangeIdDataGridViewTextBoxColumn1.HeaderText = "交易所";
            this.exchangeIdDataGridViewTextBoxColumn1.Name = "exchangeIdDataGridViewTextBoxColumn1";
            this.exchangeIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.exchangeIdDataGridViewTextBoxColumn1.Width = 66;
            // 
            // dsMstTradeInfo
            // 
            this.dsMstTradeInfo.DataSource = typeof(WinCtp.TradeInfo);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tcMstInstrument);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(314, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 204);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "持仓";
            // 
            // tcMstInstrument
            // 
            this.tcMstInstrument.Controls.Add(this.tabPage2);
            this.tcMstInstrument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMstInstrument.Location = new System.Drawing.Point(3, 17);
            this.tcMstInstrument.Name = "tcMstInstrument";
            this.tcMstInstrument.SelectedIndex = 0;
            this.tcMstInstrument.Size = new System.Drawing.Size(266, 184);
            this.tcMstInstrument.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(258, 158);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gbMstUser
            // 
            this.gbMstUser.Controls.Add(this.gvMstUser);
            this.gbMstUser.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbMstUser.Location = new System.Drawing.Point(0, 0);
            this.gbMstUser.Name = "gbMstUser";
            this.gbMstUser.Size = new System.Drawing.Size(314, 204);
            this.gbMstUser.TabIndex = 0;
            this.gbMstUser.TabStop = false;
            this.gbMstUser.Text = "主账户";
            // 
            // gvMstUser
            // 
            this.gvMstUser.AllowUserToAddRows = false;
            this.gvMstUser.AllowUserToDeleteRows = false;
            this.gvMstUser.AutoGenerateColumns = false;
            this.gvMstUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvMstUser.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvMstUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMstUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isCheckedDataGridViewCheckBoxColumn,
            this.userIdDataGridViewTextBoxColumn,
            this.userNameDataGridViewTextBoxColumn,
            this.isLoginDataGridViewCheckBoxColumn});
            this.gvMstUser.ContextMenuStrip = this.cmsMstUser;
            this.gvMstUser.DataSource = this.dsMstUser;
            this.gvMstUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMstUser.Location = new System.Drawing.Point(3, 17);
            this.gvMstUser.Name = "gvMstUser";
            this.gvMstUser.RowTemplate.Height = 23;
            this.gvMstUser.Size = new System.Drawing.Size(308, 184);
            this.gvMstUser.TabIndex = 0;
            // 
            // isCheckedDataGridViewCheckBoxColumn
            // 
            this.isCheckedDataGridViewCheckBoxColumn.DataPropertyName = "IsChecked";
            this.isCheckedDataGridViewCheckBoxColumn.HeaderText = "选择";
            this.isCheckedDataGridViewCheckBoxColumn.Name = "isCheckedDataGridViewCheckBoxColumn";
            this.isCheckedDataGridViewCheckBoxColumn.Width = 35;
            // 
            // userIdDataGridViewTextBoxColumn
            // 
            this.userIdDataGridViewTextBoxColumn.DataPropertyName = "UserId";
            this.userIdDataGridViewTextBoxColumn.HeaderText = "投资者账户";
            this.userIdDataGridViewTextBoxColumn.Name = "userIdDataGridViewTextBoxColumn";
            this.userIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.userIdDataGridViewTextBoxColumn.Width = 90;
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "投资者";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.userNameDataGridViewTextBoxColumn.Width = 66;
            // 
            // isLoginDataGridViewCheckBoxColumn
            // 
            this.isLoginDataGridViewCheckBoxColumn.DataPropertyName = "IsLogin";
            this.isLoginDataGridViewCheckBoxColumn.HeaderText = "登录";
            this.isLoginDataGridViewCheckBoxColumn.Name = "isLoginDataGridViewCheckBoxColumn";
            this.isLoginDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isLoginDataGridViewCheckBoxColumn.Width = 35;
            // 
            // cmsMstUser
            // 
            this.cmsMstUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelectAllMstUser,
            this.tsmiMstUserLogin,
            this.tsmiMstUserLogout,
            this.tsmiRePosiMst});
            this.cmsMstUser.Name = "cmsMstUser";
            this.cmsMstUser.Size = new System.Drawing.Size(125, 92);
            // 
            // tsmiSelectAllMstUser
            // 
            this.tsmiSelectAllMstUser.Image = global::WinCtp.Properties.Resources.right;
            this.tsmiSelectAllMstUser.Name = "tsmiSelectAllMstUser";
            this.tsmiSelectAllMstUser.Size = new System.Drawing.Size(124, 22);
            this.tsmiSelectAllMstUser.Text = "全选";
            this.tsmiSelectAllMstUser.Click += new System.EventHandler(this.tsmiSelectAllMstUser_Click);
            // 
            // tsmiMstUserLogin
            // 
            this.tsmiMstUserLogin.Image = global::WinCtp.Properties.Resources.man0;
            this.tsmiMstUserLogin.Name = "tsmiMstUserLogin";
            this.tsmiMstUserLogin.Size = new System.Drawing.Size(124, 22);
            this.tsmiMstUserLogin.Text = "登录";
            this.tsmiMstUserLogin.Click += new System.EventHandler(this.tsmiMstUserLogin_Click);
            // 
            // tsmiMstUserLogout
            // 
            this.tsmiMstUserLogout.Name = "tsmiMstUserLogout";
            this.tsmiMstUserLogout.Size = new System.Drawing.Size(124, 22);
            this.tsmiMstUserLogout.Text = "注销";
            this.tsmiMstUserLogout.Click += new System.EventHandler(this.tsmiMstUserLogout_Click);
            // 
            // tsmiRePosiMst
            // 
            this.tsmiRePosiMst.Image = global::WinCtp.Properties.Resources.refresh;
            this.tsmiRePosiMst.Name = "tsmiRePosiMst";
            this.tsmiRePosiMst.Size = new System.Drawing.Size(124, 22);
            this.tsmiRePosiMst.Text = "刷新持仓";
            this.tsmiRePosiMst.Click += new System.EventHandler(this.tsmiRePosiMst_Click);
            // 
            // dsMstUser
            // 
            this.dsMstUser.DataSource = typeof(WinCtp.CtpMstUser);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gbSubUser);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 229);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1190, 207);
            this.panel2.TabIndex = 15;
            // 
            // gbSubUser
            // 
            this.gbSubUser.Controls.Add(this.gvSubUser);
            this.gbSubUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSubUser.Location = new System.Drawing.Point(0, 0);
            this.gbSubUser.Name = "gbSubUser";
            this.gbSubUser.Size = new System.Drawing.Size(976, 207);
            this.gbSubUser.TabIndex = 1;
            this.gbSubUser.TabStop = false;
            this.gbSubUser.Text = "子账户";
            // 
            // gvSubUser
            // 
            this.gvSubUser.AllowUserToAddRows = false;
            this.gvSubUser.AllowUserToDeleteRows = false;
            this.gvSubUser.AutoGenerateColumns = false;
            this.gvSubUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvSubUser.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvSubUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSubUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isCheckedDataGridViewCheckBoxColumn1,
            this.userIdDataGridViewTextBoxColumn1,
            this.userNameDataGridViewTextBoxColumn1,
            this.isLoginDataGridViewCheckBoxColumn1,
            this.gcSubUserSettlementInfoConfirmTime});
            this.gvSubUser.ContextMenuStrip = this.cmsSubUser;
            this.gvSubUser.DataSource = this.dsSubUser;
            this.gvSubUser.Location = new System.Drawing.Point(12, 20);
            this.gvSubUser.Name = "gvSubUser";
            this.gvSubUser.RowTemplate.Height = 23;
            this.gvSubUser.Size = new System.Drawing.Size(388, 157);
            this.gvSubUser.TabIndex = 1;
            // 
            // isCheckedDataGridViewCheckBoxColumn1
            // 
            this.isCheckedDataGridViewCheckBoxColumn1.DataPropertyName = "IsChecked";
            this.isCheckedDataGridViewCheckBoxColumn1.HeaderText = "选择";
            this.isCheckedDataGridViewCheckBoxColumn1.Name = "isCheckedDataGridViewCheckBoxColumn1";
            this.isCheckedDataGridViewCheckBoxColumn1.Width = 35;
            // 
            // userIdDataGridViewTextBoxColumn1
            // 
            this.userIdDataGridViewTextBoxColumn1.DataPropertyName = "UserId";
            this.userIdDataGridViewTextBoxColumn1.HeaderText = "投资者账户";
            this.userIdDataGridViewTextBoxColumn1.Name = "userIdDataGridViewTextBoxColumn1";
            this.userIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.userIdDataGridViewTextBoxColumn1.Width = 90;
            // 
            // userNameDataGridViewTextBoxColumn1
            // 
            this.userNameDataGridViewTextBoxColumn1.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn1.HeaderText = "投资者";
            this.userNameDataGridViewTextBoxColumn1.Name = "userNameDataGridViewTextBoxColumn1";
            this.userNameDataGridViewTextBoxColumn1.ReadOnly = true;
            this.userNameDataGridViewTextBoxColumn1.Width = 66;
            // 
            // isLoginDataGridViewCheckBoxColumn1
            // 
            this.isLoginDataGridViewCheckBoxColumn1.DataPropertyName = "IsLogin";
            this.isLoginDataGridViewCheckBoxColumn1.HeaderText = "登录";
            this.isLoginDataGridViewCheckBoxColumn1.Name = "isLoginDataGridViewCheckBoxColumn1";
            this.isLoginDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.isLoginDataGridViewCheckBoxColumn1.Width = 35;
            // 
            // gcSubUserSettlementInfoConfirmTime
            // 
            this.gcSubUserSettlementInfoConfirmTime.DataPropertyName = "SettlementInfoConfirmTime";
            dataGridViewCellStyle1.Format = "yy-M-d H:m";
            this.gcSubUserSettlementInfoConfirmTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.gcSubUserSettlementInfoConfirmTime.HeaderText = "结算确认";
            this.gcSubUserSettlementInfoConfirmTime.Name = "gcSubUserSettlementInfoConfirmTime";
            this.gcSubUserSettlementInfoConfirmTime.ReadOnly = true;
            this.gcSubUserSettlementInfoConfirmTime.Width = 78;
            // 
            // cmsSubUser
            // 
            this.cmsSubUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelectAllSubUser,
            this.tsmiSubUserLogin,
            this.tsmiSubUserLogout,
            this.tsmiSettlementInfoConfirm,
            this.tsmiRePosiSub});
            this.cmsSubUser.Name = "cmsMstUser";
            this.cmsSubUser.Size = new System.Drawing.Size(125, 114);
            // 
            // tsmiSelectAllSubUser
            // 
            this.tsmiSelectAllSubUser.Image = global::WinCtp.Properties.Resources.right;
            this.tsmiSelectAllSubUser.Name = "tsmiSelectAllSubUser";
            this.tsmiSelectAllSubUser.Size = new System.Drawing.Size(124, 22);
            this.tsmiSelectAllSubUser.Text = "全选";
            this.tsmiSelectAllSubUser.Click += new System.EventHandler(this.tsmiSelectAllSubUser_Click);
            // 
            // tsmiSubUserLogin
            // 
            this.tsmiSubUserLogin.Image = global::WinCtp.Properties.Resources.man0;
            this.tsmiSubUserLogin.Name = "tsmiSubUserLogin";
            this.tsmiSubUserLogin.Size = new System.Drawing.Size(124, 22);
            this.tsmiSubUserLogin.Text = "登录";
            this.tsmiSubUserLogin.Click += new System.EventHandler(this.tsmiSubUserLogin_Click);
            // 
            // tsmiSubUserLogout
            // 
            this.tsmiSubUserLogout.Name = "tsmiSubUserLogout";
            this.tsmiSubUserLogout.Size = new System.Drawing.Size(124, 22);
            this.tsmiSubUserLogout.Text = "注销";
            this.tsmiSubUserLogout.Click += new System.EventHandler(this.tsmiSubUserLogout_Click);
            // 
            // tsmiSettlementInfoConfirm
            // 
            this.tsmiSettlementInfoConfirm.Image = global::WinCtp.Properties.Resources.sms_type6;
            this.tsmiSettlementInfoConfirm.Name = "tsmiSettlementInfoConfirm";
            this.tsmiSettlementInfoConfirm.Size = new System.Drawing.Size(124, 22);
            this.tsmiSettlementInfoConfirm.Text = "结算确认";
            this.tsmiSettlementInfoConfirm.Click += new System.EventHandler(this.tsmiSettlementInfoConfirm_Click);
            // 
            // tsmiRePosiSub
            // 
            this.tsmiRePosiSub.Image = global::WinCtp.Properties.Resources.refresh;
            this.tsmiRePosiSub.Name = "tsmiRePosiSub";
            this.tsmiRePosiSub.Size = new System.Drawing.Size(124, 22);
            this.tsmiRePosiSub.Text = "刷新持仓";
            this.tsmiRePosiSub.Click += new System.EventHandler(this.tsmiRePosiSub_Click);
            // 
            // dsSubUser
            // 
            this.dsSubUser.DataSource = typeof(WinCtp.CtpSubUser);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.btnInsertOrder);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.cmbOffsetFlag);
            this.panel4.Controls.Add(this.cmbDirection);
            this.panel4.Controls.Add(this.numPrice);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.numVolume);
            this.panel4.Controls.Add(this.cmbInstrumentId);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(976, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(214, 207);
            this.panel4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "指定价";
            // 
            // btnInsertOrder
            // 
            this.btnInsertOrder.Location = new System.Drawing.Point(21, 139);
            this.btnInsertOrder.Name = "btnInsertOrder";
            this.btnInsertOrder.Size = new System.Drawing.Size(75, 51);
            this.btnInsertOrder.TabIndex = 11;
            this.btnInsertOrder.Text = "下单";
            this.btnInsertOrder.UseVisualStyleBackColor = true;
            this.btnInsertOrder.Click += new System.EventHandler(this.btnInsertOrder_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(102, 167);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "市价";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // cmbOffsetFlag
            // 
            this.cmbOffsetFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOffsetFlag.FormattingEnabled = true;
            this.cmbOffsetFlag.Location = new System.Drawing.Point(54, 59);
            this.cmbOffsetFlag.Name = "cmbOffsetFlag";
            this.cmbOffsetFlag.Size = new System.Drawing.Size(99, 20);
            this.cmbOffsetFlag.TabIndex = 9;
            // 
            // cmbDirection
            // 
            this.cmbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Location = new System.Drawing.Point(54, 32);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(99, 20);
            this.cmbDirection.TabIndex = 8;
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Location = new System.Drawing.Point(54, 112);
            this.numPrice.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(99, 21);
            this.numPrice.TabIndex = 7;
            this.numPrice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "手数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "开平";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "买卖";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(102, 139);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // numVolume
            // 
            this.numVolume.Location = new System.Drawing.Point(54, 85);
            this.numVolume.Name = "numVolume";
            this.numVolume.Size = new System.Drawing.Size(99, 21);
            this.numVolume.TabIndex = 2;
            this.numVolume.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbInstrumentId
            // 
            this.cmbInstrumentId.FormattingEnabled = true;
            this.cmbInstrumentId.Location = new System.Drawing.Point(54, 5);
            this.cmbInstrumentId.Name = "cmbInstrumentId";
            this.cmbInstrumentId.Size = new System.Drawing.Size(148, 20);
            this.cmbInstrumentId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "合约";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl4);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 436);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1190, 273);
            this.panel3.TabIndex = 16;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tpSubOrder);
            this.tabControl4.Controls.Add(this.tpSubTrade);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(387, 0);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(803, 273);
            this.tabControl4.TabIndex = 1;
            // 
            // tpSubOrder
            // 
            this.tpSubOrder.Controls.Add(this.gvSubOrder);
            this.tpSubOrder.Location = new System.Drawing.Point(4, 22);
            this.tpSubOrder.Name = "tpSubOrder";
            this.tpSubOrder.Size = new System.Drawing.Size(795, 247);
            this.tpSubOrder.TabIndex = 2;
            this.tpSubOrder.Text = "委托单";
            this.tpSubOrder.UseVisualStyleBackColor = true;
            // 
            // gvSubOrder
            // 
            this.gvSubOrder.AllowUserToAddRows = false;
            this.gvSubOrder.AllowUserToDeleteRows = false;
            this.gvSubOrder.AutoGenerateColumns = false;
            this.gvSubOrder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvSubOrder.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvSubOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSubOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.investorIdDataGridViewTextBoxColumn2,
            this.orderSysIdDataGridViewTextBoxColumn1,
            this.instrumentIdDataGridViewTextBoxColumn2,
            this.gcSubOrderDirection,
            this.gcSubOrderCombOffsetFlag,
            this.gcSubOrderStatus,
            this.limitPriceDataGridViewTextBoxColumn,
            this.volumeTotalOriginalDataGridViewTextBoxColumn1,
            this.volumeTotalDataGridViewTextBoxColumn1,
            this.volumeTradedDataGridViewTextBoxColumn1,
            this.insertTimeDataGridViewTextBoxColumn1,
            this.updateTimeDataGridViewTextBoxColumn1,
            this.gcSubOrderErrorMsg});
            this.gvSubOrder.ContextMenuStrip = this.cmsSubOrder;
            this.gvSubOrder.DataSource = this.dsSubOrder;
            this.gvSubOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvSubOrder.Location = new System.Drawing.Point(0, 0);
            this.gvSubOrder.Name = "gvSubOrder";
            this.gvSubOrder.ReadOnly = true;
            this.gvSubOrder.RowTemplate.Height = 23;
            this.gvSubOrder.Size = new System.Drawing.Size(795, 247);
            this.gvSubOrder.TabIndex = 0;
            // 
            // investorIdDataGridViewTextBoxColumn2
            // 
            this.investorIdDataGridViewTextBoxColumn2.DataPropertyName = "InvestorId";
            this.investorIdDataGridViewTextBoxColumn2.HeaderText = "投资者";
            this.investorIdDataGridViewTextBoxColumn2.Name = "investorIdDataGridViewTextBoxColumn2";
            this.investorIdDataGridViewTextBoxColumn2.ReadOnly = true;
            this.investorIdDataGridViewTextBoxColumn2.Width = 66;
            // 
            // orderSysIdDataGridViewTextBoxColumn1
            // 
            this.orderSysIdDataGridViewTextBoxColumn1.DataPropertyName = "OrderSysId";
            this.orderSysIdDataGridViewTextBoxColumn1.HeaderText = "编号";
            this.orderSysIdDataGridViewTextBoxColumn1.Name = "orderSysIdDataGridViewTextBoxColumn1";
            this.orderSysIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.orderSysIdDataGridViewTextBoxColumn1.Width = 54;
            // 
            // instrumentIdDataGridViewTextBoxColumn2
            // 
            this.instrumentIdDataGridViewTextBoxColumn2.DataPropertyName = "InstrumentId";
            this.instrumentIdDataGridViewTextBoxColumn2.HeaderText = "合约";
            this.instrumentIdDataGridViewTextBoxColumn2.Name = "instrumentIdDataGridViewTextBoxColumn2";
            this.instrumentIdDataGridViewTextBoxColumn2.ReadOnly = true;
            this.instrumentIdDataGridViewTextBoxColumn2.Width = 54;
            // 
            // gcSubOrderDirection
            // 
            this.gcSubOrderDirection.DataPropertyName = "Direction";
            this.gcSubOrderDirection.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcSubOrderDirection.HeaderText = "买卖";
            this.gcSubOrderDirection.Name = "gcSubOrderDirection";
            this.gcSubOrderDirection.ReadOnly = true;
            this.gcSubOrderDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSubOrderDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcSubOrderDirection.Width = 54;
            // 
            // gcSubOrderCombOffsetFlag
            // 
            this.gcSubOrderCombOffsetFlag.DataPropertyName = "CombOffsetFlag";
            this.gcSubOrderCombOffsetFlag.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcSubOrderCombOffsetFlag.HeaderText = "开平";
            this.gcSubOrderCombOffsetFlag.Name = "gcSubOrderCombOffsetFlag";
            this.gcSubOrderCombOffsetFlag.ReadOnly = true;
            this.gcSubOrderCombOffsetFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSubOrderCombOffsetFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcSubOrderCombOffsetFlag.Width = 54;
            // 
            // gcSubOrderStatus
            // 
            this.gcSubOrderStatus.DataPropertyName = "OrderStatus";
            this.gcSubOrderStatus.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcSubOrderStatus.HeaderText = "状态";
            this.gcSubOrderStatus.Name = "gcSubOrderStatus";
            this.gcSubOrderStatus.ReadOnly = true;
            this.gcSubOrderStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSubOrderStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcSubOrderStatus.Width = 54;
            // 
            // limitPriceDataGridViewTextBoxColumn
            // 
            this.limitPriceDataGridViewTextBoxColumn.DataPropertyName = "LimitPrice";
            this.limitPriceDataGridViewTextBoxColumn.HeaderText = "价格";
            this.limitPriceDataGridViewTextBoxColumn.Name = "limitPriceDataGridViewTextBoxColumn";
            this.limitPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.limitPriceDataGridViewTextBoxColumn.Width = 54;
            // 
            // volumeTotalOriginalDataGridViewTextBoxColumn1
            // 
            this.volumeTotalOriginalDataGridViewTextBoxColumn1.DataPropertyName = "VolumeTotalOriginal";
            this.volumeTotalOriginalDataGridViewTextBoxColumn1.HeaderText = "报单手数";
            this.volumeTotalOriginalDataGridViewTextBoxColumn1.Name = "volumeTotalOriginalDataGridViewTextBoxColumn1";
            this.volumeTotalOriginalDataGridViewTextBoxColumn1.ReadOnly = true;
            this.volumeTotalOriginalDataGridViewTextBoxColumn1.Width = 78;
            // 
            // volumeTotalDataGridViewTextBoxColumn1
            // 
            this.volumeTotalDataGridViewTextBoxColumn1.DataPropertyName = "VolumeTotal";
            this.volumeTotalDataGridViewTextBoxColumn1.HeaderText = "未成交";
            this.volumeTotalDataGridViewTextBoxColumn1.Name = "volumeTotalDataGridViewTextBoxColumn1";
            this.volumeTotalDataGridViewTextBoxColumn1.ReadOnly = true;
            this.volumeTotalDataGridViewTextBoxColumn1.Width = 66;
            // 
            // volumeTradedDataGridViewTextBoxColumn1
            // 
            this.volumeTradedDataGridViewTextBoxColumn1.DataPropertyName = "VolumeTraded";
            this.volumeTradedDataGridViewTextBoxColumn1.HeaderText = "成交手数";
            this.volumeTradedDataGridViewTextBoxColumn1.Name = "volumeTradedDataGridViewTextBoxColumn1";
            this.volumeTradedDataGridViewTextBoxColumn1.ReadOnly = true;
            this.volumeTradedDataGridViewTextBoxColumn1.Width = 78;
            // 
            // insertTimeDataGridViewTextBoxColumn1
            // 
            this.insertTimeDataGridViewTextBoxColumn1.DataPropertyName = "InsertTime";
            this.insertTimeDataGridViewTextBoxColumn1.HeaderText = "报单时间";
            this.insertTimeDataGridViewTextBoxColumn1.Name = "insertTimeDataGridViewTextBoxColumn1";
            this.insertTimeDataGridViewTextBoxColumn1.ReadOnly = true;
            this.insertTimeDataGridViewTextBoxColumn1.Width = 78;
            // 
            // updateTimeDataGridViewTextBoxColumn1
            // 
            this.updateTimeDataGridViewTextBoxColumn1.DataPropertyName = "UpdateTime";
            this.updateTimeDataGridViewTextBoxColumn1.HeaderText = "成交时间";
            this.updateTimeDataGridViewTextBoxColumn1.Name = "updateTimeDataGridViewTextBoxColumn1";
            this.updateTimeDataGridViewTextBoxColumn1.ReadOnly = true;
            this.updateTimeDataGridViewTextBoxColumn1.Width = 78;
            // 
            // gcSubOrderErrorMsg
            // 
            this.gcSubOrderErrorMsg.DataPropertyName = "ErrorMsg";
            this.gcSubOrderErrorMsg.HeaderText = "错误消息";
            this.gcSubOrderErrorMsg.Name = "gcSubOrderErrorMsg";
            this.gcSubOrderErrorMsg.ReadOnly = true;
            this.gcSubOrderErrorMsg.Width = 78;
            // 
            // cmsSubOrder
            // 
            this.cmsSubOrder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCancelOrder});
            this.cmsSubOrder.Name = "cmsSubOrder";
            this.cmsSubOrder.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmiCancelOrder
            // 
            this.tsmiCancelOrder.Image = global::WinCtp.Properties.Resources.wrong;
            this.tsmiCancelOrder.Name = "tsmiCancelOrder";
            this.tsmiCancelOrder.Size = new System.Drawing.Size(100, 22);
            this.tsmiCancelOrder.Text = "撤单";
            this.tsmiCancelOrder.Click += new System.EventHandler(this.tsmiCancelOrder_Click);
            // 
            // dsSubOrder
            // 
            this.dsSubOrder.DataSource = typeof(WinCtp.OrderInfo);
            // 
            // tpSubTrade
            // 
            this.tpSubTrade.Controls.Add(this.dataGridViewEx1);
            this.tpSubTrade.Location = new System.Drawing.Point(4, 22);
            this.tpSubTrade.Name = "tpSubTrade";
            this.tpSubTrade.Padding = new System.Windows.Forms.Padding(3);
            this.tpSubTrade.Size = new System.Drawing.Size(795, 247);
            this.tpSubTrade.TabIndex = 1;
            this.tpSubTrade.Text = "成交记录";
            this.tpSubTrade.UseVisualStyleBackColor = true;
            // 
            // dataGridViewEx1
            // 
            this.dataGridViewEx1.AllowUserToAddRows = false;
            this.dataGridViewEx1.AllowUserToDeleteRows = false;
            this.dataGridViewEx1.AutoGenerateColumns = false;
            this.dataGridViewEx1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewEx1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewEx1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEx1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.investorIdDataGridViewTextBoxColumn,
            this.tradeIdDataGridViewTextBoxColumn,
            this.instrumentIdDataGridViewTextBoxColumn,
            this.gcSubTradeDirection,
            this.gcSubTradeOffsetFlag,
            this.priceDataGridViewTextBoxColumn,
            this.volumeDataGridViewTextBoxColumn,
            this.tradeTimeDataGridViewTextBoxColumn,
            this.orderLocalIdDataGridViewTextBoxColumn,
            this.exchangeIdDataGridViewTextBoxColumn});
            this.dataGridViewEx1.DataSource = this.dsSubTradeInfo;
            this.dataGridViewEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEx1.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewEx1.Name = "dataGridViewEx1";
            this.dataGridViewEx1.ReadOnly = true;
            this.dataGridViewEx1.RowTemplate.Height = 23;
            this.dataGridViewEx1.Size = new System.Drawing.Size(789, 241);
            this.dataGridViewEx1.TabIndex = 0;
            // 
            // investorIdDataGridViewTextBoxColumn
            // 
            this.investorIdDataGridViewTextBoxColumn.DataPropertyName = "InvestorId";
            this.investorIdDataGridViewTextBoxColumn.HeaderText = "投资者";
            this.investorIdDataGridViewTextBoxColumn.Name = "investorIdDataGridViewTextBoxColumn";
            this.investorIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.investorIdDataGridViewTextBoxColumn.Width = 66;
            // 
            // tradeIdDataGridViewTextBoxColumn
            // 
            this.tradeIdDataGridViewTextBoxColumn.DataPropertyName = "TradeId";
            this.tradeIdDataGridViewTextBoxColumn.HeaderText = "编号";
            this.tradeIdDataGridViewTextBoxColumn.Name = "tradeIdDataGridViewTextBoxColumn";
            this.tradeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.tradeIdDataGridViewTextBoxColumn.Width = 54;
            // 
            // instrumentIdDataGridViewTextBoxColumn
            // 
            this.instrumentIdDataGridViewTextBoxColumn.DataPropertyName = "InstrumentId";
            this.instrumentIdDataGridViewTextBoxColumn.HeaderText = "合约";
            this.instrumentIdDataGridViewTextBoxColumn.Name = "instrumentIdDataGridViewTextBoxColumn";
            this.instrumentIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.instrumentIdDataGridViewTextBoxColumn.Width = 54;
            // 
            // gcSubTradeDirection
            // 
            this.gcSubTradeDirection.DataPropertyName = "Direction";
            this.gcSubTradeDirection.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcSubTradeDirection.HeaderText = "买卖";
            this.gcSubTradeDirection.Name = "gcSubTradeDirection";
            this.gcSubTradeDirection.ReadOnly = true;
            this.gcSubTradeDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSubTradeDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcSubTradeDirection.Width = 54;
            // 
            // gcSubTradeOffsetFlag
            // 
            this.gcSubTradeOffsetFlag.DataPropertyName = "OffsetFlag";
            this.gcSubTradeOffsetFlag.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.gcSubTradeOffsetFlag.HeaderText = "开平";
            this.gcSubTradeOffsetFlag.Name = "gcSubTradeOffsetFlag";
            this.gcSubTradeOffsetFlag.ReadOnly = true;
            this.gcSubTradeOffsetFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSubTradeOffsetFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.gcSubTradeOffsetFlag.Width = 54;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "成交价格";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.ReadOnly = true;
            this.priceDataGridViewTextBoxColumn.Width = 78;
            // 
            // volumeDataGridViewTextBoxColumn
            // 
            this.volumeDataGridViewTextBoxColumn.DataPropertyName = "Volume";
            this.volumeDataGridViewTextBoxColumn.HeaderText = "成交手数";
            this.volumeDataGridViewTextBoxColumn.Name = "volumeDataGridViewTextBoxColumn";
            this.volumeDataGridViewTextBoxColumn.ReadOnly = true;
            this.volumeDataGridViewTextBoxColumn.Width = 78;
            // 
            // tradeTimeDataGridViewTextBoxColumn
            // 
            this.tradeTimeDataGridViewTextBoxColumn.DataPropertyName = "TradeTime";
            this.tradeTimeDataGridViewTextBoxColumn.HeaderText = "成交时间";
            this.tradeTimeDataGridViewTextBoxColumn.Name = "tradeTimeDataGridViewTextBoxColumn";
            this.tradeTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.tradeTimeDataGridViewTextBoxColumn.Width = 78;
            // 
            // orderLocalIdDataGridViewTextBoxColumn
            // 
            this.orderLocalIdDataGridViewTextBoxColumn.DataPropertyName = "OrderLocalId";
            this.orderLocalIdDataGridViewTextBoxColumn.HeaderText = "报单编号";
            this.orderLocalIdDataGridViewTextBoxColumn.Name = "orderLocalIdDataGridViewTextBoxColumn";
            this.orderLocalIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderLocalIdDataGridViewTextBoxColumn.Width = 78;
            // 
            // exchangeIdDataGridViewTextBoxColumn
            // 
            this.exchangeIdDataGridViewTextBoxColumn.DataPropertyName = "ExchangeId";
            this.exchangeIdDataGridViewTextBoxColumn.HeaderText = "交易所";
            this.exchangeIdDataGridViewTextBoxColumn.Name = "exchangeIdDataGridViewTextBoxColumn";
            this.exchangeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.exchangeIdDataGridViewTextBoxColumn.Width = 66;
            // 
            // dsSubTradeInfo
            // 
            this.dsSubTradeInfo.DataSource = typeof(WinCtp.TradeInfo);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tcSubInstrument);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 273);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "持仓";
            // 
            // tcSubInstrument
            // 
            this.tcSubInstrument.Controls.Add(this.tabPage1);
            this.tcSubInstrument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSubInstrument.Location = new System.Drawing.Point(3, 17);
            this.tcSubInstrument.Name = "tcSubInstrument";
            this.tcSubInstrument.SelectedIndex = 0;
            this.tcSubInstrument.Size = new System.Drawing.Size(381, 253);
            this.tcSubInstrument.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(373, 227);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUser,
            this.ibtnBroker,
            this.tsmiListen,
            this.ibtnSetting,
            this.ibtnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1190, 25);
            this.toolStrip.TabIndex = 17;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsmiUser
            // 
            this.tsmiUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ibtnUser,
            this.ibtnOrderInsertConfig});
            this.tsmiUser.Image = global::WinCtp.Properties.Resources.sms_type6;
            this.tsmiUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiUser.Name = "tsmiUser";
            this.tsmiUser.Size = new System.Drawing.Size(61, 22);
            this.tsmiUser.Text = "账户";
            // 
            // ibtnUser
            // 
            this.ibtnUser.Image = global::WinCtp.Properties.Resources.man0;
            this.ibtnUser.Name = "ibtnUser";
            this.ibtnUser.Size = new System.Drawing.Size(124, 22);
            this.ibtnUser.Text = "账户配置";
            this.ibtnUser.Click += new System.EventHandler(this.ibtnUser_Click);
            // 
            // ibtnOrderInsertConfig
            // 
            this.ibtnOrderInsertConfig.Image = global::WinCtp.Properties.Resources._6;
            this.ibtnOrderInsertConfig.Name = "ibtnOrderInsertConfig";
            this.ibtnOrderInsertConfig.Size = new System.Drawing.Size(124, 22);
            this.ibtnOrderInsertConfig.Text = "下单配置";
            this.ibtnOrderInsertConfig.Click += new System.EventHandler(this.ibtnOrderInsertConfig_Click);
            // 
            // ibtnBroker
            // 
            this.ibtnBroker.Image = global::WinCtp.Properties.Resources.flag;
            this.ibtnBroker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnBroker.Name = "ibtnBroker";
            this.ibtnBroker.Size = new System.Drawing.Size(76, 22);
            this.ibtnBroker.Text = "期货公司";
            this.ibtnBroker.Click += new System.EventHandler(this.ibtnBroker_Click);
            // 
            // tsmiListen
            // 
            this.tsmiListen.Image = global::WinCtp.Properties.Resources.clock;
            this.tsmiListen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiListen.Name = "tsmiListen";
            this.tsmiListen.Size = new System.Drawing.Size(76, 22);
            this.tsmiListen.Text = "启动监听";
            this.tsmiListen.ToolTipText = "启动监听";
            this.tsmiListen.Click += new System.EventHandler(this.tsmiListen_Click);
            // 
            // ibtnSetting
            // 
            this.ibtnSetting.Image = global::WinCtp.Properties.Resources._6;
            this.ibtnSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnSetting.Name = "ibtnSetting";
            this.ibtnSetting.Size = new System.Drawing.Size(52, 22);
            this.ibtnSetting.Text = "设置";
            this.ibtnSetting.Click += new System.EventHandler(this.ibtnSetting_Click);
            // 
            // ibtnClose
            // 
            this.ibtnClose.Image = global::WinCtp.Properties.Resources.close;
            this.ibtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnClose.Name = "ibtnClose";
            this.ibtnClose.Size = new System.Drawing.Size(52, 22);
            this.ibtnClose.Text = "关闭";
            this.ibtnClose.Click += new System.EventHandler(this.ibtnClose_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 731);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "跟单系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tpMstOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMstOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMstOrder)).EndInit();
            this.tpMstTrade.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMstTradeInfo)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tcMstInstrument.ResumeLayout(false);
            this.gbMstUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMstUser)).EndInit();
            this.cmsMstUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsMstUser)).EndInit();
            this.panel2.ResumeLayout(false);
            this.gbSubUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSubUser)).EndInit();
            this.cmsSubUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsSubUser)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).EndInit();
            this.panel3.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tpSubOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSubOrder)).EndInit();
            this.cmsSubOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsSubOrder)).EndInit();
            this.tpSubTrade.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsSubTradeInfo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tcSubInstrument.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox gbMstUser;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tpMstTrade;
        private System.Windows.Forms.TabControl tcMstInstrument;
        private DataGridViewEx gvMstUser;
        private System.Windows.Forms.GroupBox gbSubUser;
        private DataGridViewEx gvSubUser;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tpSubTrade;
        private System.Windows.Forms.TabControl tcSubInstrument;
        private System.Windows.Forms.Button btnInsertOrder;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox cmbOffsetFlag;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numVolume;
        private System.Windows.Forms.ComboBox cmbInstrumentId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsmiListen;
        private System.Windows.Forms.BindingSource dsMstUser;
        private System.Windows.Forms.BindingSource dsSubUser;
        private System.Windows.Forms.ContextMenuStrip cmsMstUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAllMstUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiMstUserLogin;
        private System.Windows.Forms.ToolStripMenuItem tsmiMstUserLogout;
        private System.Windows.Forms.ContextMenuStrip cmsSubUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAllSubUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiSubUserLogin;
        private System.Windows.Forms.ToolStripMenuItem tsmiSubUserLogout;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isCheckedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLoginDataGridViewCheckBoxColumn;
        private DataGridViewEx dataGridViewEx1;
        private System.Windows.Forms.BindingSource dsSubTradeInfo;
        private System.Windows.Forms.TabPage tabPage1;
        private DataGridViewEx dataGridViewEx2;
        private System.Windows.Forms.BindingSource dsMstTradeInfo;
        private System.Windows.Forms.ToolStripDropDownButton tsmiUser;
        private System.Windows.Forms.ToolStripMenuItem ibtnUser;
        private System.Windows.Forms.ToolStripMenuItem ibtnOrderInsertConfig;
        private System.Windows.Forms.TabPage tpSubOrder;
        private System.Windows.Forms.ToolStripButton ibtnBroker;
        private System.Windows.Forms.TabPage tpMstOrder;
        private DataGridViewEx gvSubOrder;
        private System.Windows.Forms.BindingSource dsSubOrder;
        private DataGridViewEx gvMstOrder;
        private System.Windows.Forms.BindingSource dsMstOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn investorIdDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderSysIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn instrumentIdDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn directionDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn combOffsetFlagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeTotalOriginalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeTotalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeTradedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn insertTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettlementInfoConfirm;
        private System.Windows.Forms.DataGridViewTextBoxColumn investorIdDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderSysIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn instrumentIdDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcSubOrderDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcSubOrderCombOffsetFlag;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcSubOrderStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn limitPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeTotalOriginalDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeTotalDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeTradedDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn insertTimeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateTimeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcSubOrderErrorMsg;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isCheckedDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLoginDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcSubUserSettlementInfoConfirmTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn investorIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn instrumentIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcSubTradeDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcSubTradeOffsetFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderLocalIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn investorIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn instrumentIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcMstTradeDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn gcMstTradeOffsetFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeTimeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderLocalIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.ToolStripButton ibtnSetting;
        private System.Windows.Forms.ContextMenuStrip cmsSubOrder;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancelOrder;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem tsmiRePosiMst;
        private System.Windows.Forms.ToolStripMenuItem tsmiRePosiSub;
        private System.Windows.Forms.ToolStripButton ibtnClose;
        private System.Windows.Forms.ToolStripStatusLabel tsslMdStatus;
    }
}


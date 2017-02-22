namespace WinCtp
{
    partial class FrmBroker
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
            System.Windows.Forms.Label idLabel;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label traderFrontAddressLabel;
            System.Windows.Forms.Label marketFrontAddressLabel;
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMst = new System.Windows.Forms.TabPage();
            this.gvBroker = new WinCtp.DataGridViewEx();
            this.dsBroker = new System.Windows.Forms.BindingSource(this.components);
            this.tpDet = new System.Windows.Forms.TabPage();
            this.marketFrontAddressTextBox = new System.Windows.Forms.TextBox();
            this.traderFrontAddressTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ibtnMst = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnNew = new System.Windows.Forms.ToolStripButton();
            this.ibtnEdit = new System.Windows.Forms.ToolStripButton();
            this.ibtnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnClose = new System.Windows.Forms.ToolStripButton();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            idLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            traderFrontAddressLabel = new System.Windows.Forms.Label();
            marketFrontAddressLabel = new System.Windows.Forms.Label();
            this.tcMain.SuspendLayout();
            this.tpMst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvBroker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBroker)).BeginInit();
            this.tpDet.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new System.Drawing.Point(43, 17);
            idLabel.Name = "idLabel";
            idLabel.Size = new System.Drawing.Size(59, 12);
            idLabel.TabIndex = 0;
            idLabel.Text = "公司代码:";
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(43, 54);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(59, 12);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "公司名称:";
            // 
            // traderFrontAddressLabel
            // 
            traderFrontAddressLabel.AutoSize = true;
            traderFrontAddressLabel.Location = new System.Drawing.Point(19, 93);
            traderFrontAddressLabel.Name = "traderFrontAddressLabel";
            traderFrontAddressLabel.Size = new System.Drawing.Size(83, 12);
            traderFrontAddressLabel.TabIndex = 4;
            traderFrontAddressLabel.Text = "交易接口地址:";
            // 
            // marketFrontAddressLabel
            // 
            marketFrontAddressLabel.AutoSize = true;
            marketFrontAddressLabel.Location = new System.Drawing.Point(19, 131);
            marketFrontAddressLabel.Name = "marketFrontAddressLabel";
            marketFrontAddressLabel.Size = new System.Drawing.Size(83, 12);
            marketFrontAddressLabel.TabIndex = 6;
            marketFrontAddressLabel.Text = "行情接口地址:";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpMst);
            this.tcMain.Controls.Add(this.tpDet);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 25);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(663, 402);
            this.tcMain.TabIndex = 0;
            // 
            // tpMst
            // 
            this.tpMst.Controls.Add(this.gvBroker);
            this.tpMst.Location = new System.Drawing.Point(4, 22);
            this.tpMst.Name = "tpMst";
            this.tpMst.Padding = new System.Windows.Forms.Padding(3);
            this.tpMst.Size = new System.Drawing.Size(655, 376);
            this.tpMst.TabIndex = 1;
            this.tpMst.Text = "概要";
            this.tpMst.UseVisualStyleBackColor = true;
            // 
            // gvBroker
            // 
            this.gvBroker.AllowUserToAddRows = false;
            this.gvBroker.AllowUserToDeleteRows = false;
            this.gvBroker.AutoGenerateColumns = false;
            this.gvBroker.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvBroker.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvBroker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBroker.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.gvBroker.DataSource = this.dsBroker;
            this.gvBroker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvBroker.Location = new System.Drawing.Point(3, 3);
            this.gvBroker.Name = "gvBroker";
            this.gvBroker.ReadOnly = true;
            this.gvBroker.RowTemplate.Height = 23;
            this.gvBroker.Size = new System.Drawing.Size(649, 370);
            this.gvBroker.TabIndex = 0;
            this.gvBroker.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBroker_CellContentDoubleClick);
            // 
            // dsBroker
            // 
            this.dsBroker.DataSource = typeof(WinCtp.BrokerInfo);
            // 
            // tpDet
            // 
            this.tpDet.Controls.Add(marketFrontAddressLabel);
            this.tpDet.Controls.Add(this.marketFrontAddressTextBox);
            this.tpDet.Controls.Add(traderFrontAddressLabel);
            this.tpDet.Controls.Add(this.traderFrontAddressTextBox);
            this.tpDet.Controls.Add(nameLabel);
            this.tpDet.Controls.Add(this.nameTextBox);
            this.tpDet.Controls.Add(idLabel);
            this.tpDet.Controls.Add(this.idTextBox);
            this.tpDet.Location = new System.Drawing.Point(4, 22);
            this.tpDet.Name = "tpDet";
            this.tpDet.Padding = new System.Windows.Forms.Padding(3);
            this.tpDet.Size = new System.Drawing.Size(655, 376);
            this.tpDet.TabIndex = 0;
            this.tpDet.Text = "明细";
            this.tpDet.UseVisualStyleBackColor = true;
            // 
            // marketFrontAddressTextBox
            // 
            this.marketFrontAddressTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsBroker, "MarketFrontAddress", true));
            this.marketFrontAddressTextBox.Location = new System.Drawing.Point(108, 128);
            this.marketFrontAddressTextBox.Name = "marketFrontAddressTextBox";
            this.marketFrontAddressTextBox.PasswordChar = '#';
            this.marketFrontAddressTextBox.ReadOnly = true;
            this.marketFrontAddressTextBox.Size = new System.Drawing.Size(288, 21);
            this.marketFrontAddressTextBox.TabIndex = 7;
            this.marketFrontAddressTextBox.Tag = "New Edit";
            // 
            // traderFrontAddressTextBox
            // 
            this.traderFrontAddressTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsBroker, "TraderFrontAddress", true));
            this.traderFrontAddressTextBox.Location = new System.Drawing.Point(108, 90);
            this.traderFrontAddressTextBox.Name = "traderFrontAddressTextBox";
            this.traderFrontAddressTextBox.PasswordChar = '#';
            this.traderFrontAddressTextBox.ReadOnly = true;
            this.traderFrontAddressTextBox.Size = new System.Drawing.Size(288, 21);
            this.traderFrontAddressTextBox.TabIndex = 5;
            this.traderFrontAddressTextBox.Tag = "New Edit";
            // 
            // nameTextBox
            // 
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsBroker, "Name", true));
            this.nameTextBox.Location = new System.Drawing.Point(108, 51);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(151, 21);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.Tag = "New Edit";
            // 
            // idTextBox
            // 
            this.idTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsBroker, "Id", true));
            this.idTextBox.Location = new System.Drawing.Point(108, 14);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.ReadOnly = true;
            this.idTextBox.Size = new System.Drawing.Size(151, 21);
            this.idTextBox.TabIndex = 1;
            this.idTextBox.Tag = "New";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ibtnMst,
            this.toolStripSeparator1,
            this.ibtnNew,
            this.ibtnEdit,
            this.ibtnCancel,
            this.toolStripSeparator2,
            this.ibtnSave,
            this.toolStripSeparator3,
            this.ibtnDelete,
            this.toolStripSeparator4,
            this.ibtnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(663, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // ibtnMst
            // 
            this.ibtnMst.Image = global::WinCtp.Properties.Resources._5;
            this.ibtnMst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnMst.Name = "ibtnMst";
            this.ibtnMst.Size = new System.Drawing.Size(52, 22);
            this.ibtnMst.Tag = "Det";
            this.ibtnMst.Text = "概要";
            this.ibtnMst.Click += new System.EventHandler(this.ibtnMst_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ibtnNew
            // 
            this.ibtnNew.Image = global::WinCtp.Properties.Resources._new;
            this.ibtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnNew.Name = "ibtnNew";
            this.ibtnNew.Size = new System.Drawing.Size(52, 22);
            this.ibtnNew.Tag = "Mst Det";
            this.ibtnNew.Text = "新建";
            this.ibtnNew.Click += new System.EventHandler(this.ibtnNew_Click);
            // 
            // ibtnEdit
            // 
            this.ibtnEdit.Image = global::WinCtp.Properties.Resources.edit;
            this.ibtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnEdit.Name = "ibtnEdit";
            this.ibtnEdit.Size = new System.Drawing.Size(52, 22);
            this.ibtnEdit.Tag = "Mst Det";
            this.ibtnEdit.Text = "编辑";
            this.ibtnEdit.Click += new System.EventHandler(this.ibtnEdit_Click);
            // 
            // ibtnCancel
            // 
            this.ibtnCancel.Image = global::WinCtp.Properties.Resources.undo;
            this.ibtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnCancel.Name = "ibtnCancel";
            this.ibtnCancel.Size = new System.Drawing.Size(52, 22);
            this.ibtnCancel.Tag = "Edit";
            this.ibtnCancel.Text = "取消";
            this.ibtnCancel.Click += new System.EventHandler(this.ibtnCancel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ibtnSave
            // 
            this.ibtnSave.Image = global::WinCtp.Properties.Resources.save;
            this.ibtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnSave.Name = "ibtnSave";
            this.ibtnSave.Size = new System.Drawing.Size(52, 22);
            this.ibtnSave.Tag = "Edit";
            this.ibtnSave.Text = "保存";
            this.ibtnSave.Click += new System.EventHandler(this.ibtnSave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ibtnDelete
            // 
            this.ibtnDelete.Image = global::WinCtp.Properties.Resources.delete;
            this.ibtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnDelete.Name = "ibtnDelete";
            this.ibtnDelete.Size = new System.Drawing.Size(52, 22);
            this.ibtnDelete.Text = "删除";
            this.ibtnDelete.Click += new System.EventHandler(this.ibtnDelete_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
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
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.FillWeight = 120F;
            this.idDataGridViewTextBoxColumn.HeaderText = "公司代码";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 78;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.FillWeight = 120F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "公司名称";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 78;
            // 
            // FrmBroker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 427);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBroker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "期货公司";
            this.tcMain.ResumeLayout(false);
            this.tpMst.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvBroker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBroker)).EndInit();
            this.tpDet.ResumeLayout(false);
            this.tpDet.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpMst;
        private System.Windows.Forms.TabPage tpDet;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ibtnClose;
        private System.Windows.Forms.ToolStripButton ibtnMst;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ibtnNew;
        private System.Windows.Forms.ToolStripButton ibtnEdit;
        private System.Windows.Forms.ToolStripButton ibtnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ibtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private DataGridViewEx gvBroker;
        private System.Windows.Forms.BindingSource dsBroker;
        private System.Windows.Forms.TextBox marketFrontAddressTextBox;
        private System.Windows.Forms.TextBox traderFrontAddressTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.ToolStripButton ibtnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    }
}
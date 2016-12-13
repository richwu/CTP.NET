namespace WinCtp
{
    partial class FrmUserInsertOrderConfig
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
            System.Windows.Forms.Label instrumentLabel;
            System.Windows.Forms.Label mstUserIdLabel;
            System.Windows.Forms.Label subUserIdLabel;
            System.Windows.Forms.Label volumeLabel;
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMst = new System.Windows.Forms.TabPage();
            this.gvData = new WinCtp.DataGridViewEx();
            this.dsData = new System.Windows.Forms.BindingSource(this.components);
            this.tpDet = new System.Windows.Forms.TabPage();
            this.volumeTextBox = new System.Windows.Forms.TextBox();
            this.subUserIdTextBox = new System.Windows.Forms.TextBox();
            this.mstUserIdTextBox = new System.Windows.Forms.TextBox();
            this.isInverseCheckBox = new System.Windows.Forms.CheckBox();
            this.instrumentTextBox = new System.Windows.Forms.TextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ibtnMst = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnNew = new System.Windows.Forms.ToolStripButton();
            this.ibtnEdit = new System.Windows.Forms.ToolStripButton();
            this.ibtnCancel = new System.Windows.Forms.ToolStripButton();
            this.ibtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnClose = new System.Windows.Forms.ToolStripButton();
            this.subUserIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instrumentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mstUserIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volumeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isInverseDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            instrumentLabel = new System.Windows.Forms.Label();
            mstUserIdLabel = new System.Windows.Forms.Label();
            subUserIdLabel = new System.Windows.Forms.Label();
            volumeLabel = new System.Windows.Forms.Label();
            this.tcMain.SuspendLayout();
            this.tpMst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsData)).BeginInit();
            this.tpDet.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // instrumentLabel
            // 
            instrumentLabel.AutoSize = true;
            instrumentLabel.Location = new System.Drawing.Point(33, 78);
            instrumentLabel.Name = "instrumentLabel";
            instrumentLabel.Size = new System.Drawing.Size(35, 12);
            instrumentLabel.TabIndex = 0;
            instrumentLabel.Text = "品种:";
            // 
            // mstUserIdLabel
            // 
            mstUserIdLabel.AutoSize = true;
            mstUserIdLabel.Location = new System.Drawing.Point(21, 47);
            mstUserIdLabel.Name = "mstUserIdLabel";
            mstUserIdLabel.Size = new System.Drawing.Size(47, 12);
            mstUserIdLabel.TabIndex = 4;
            mstUserIdLabel.Text = "主账户:";
            // 
            // subUserIdLabel
            // 
            subUserIdLabel.AutoSize = true;
            subUserIdLabel.Location = new System.Drawing.Point(21, 17);
            subUserIdLabel.Name = "subUserIdLabel";
            subUserIdLabel.Size = new System.Drawing.Size(47, 12);
            subUserIdLabel.TabIndex = 6;
            subUserIdLabel.Text = "子账户:";
            // 
            // volumeLabel
            // 
            volumeLabel.AutoSize = true;
            volumeLabel.Location = new System.Drawing.Point(9, 114);
            volumeLabel.Name = "volumeLabel";
            volumeLabel.Size = new System.Drawing.Size(59, 12);
            volumeLabel.TabIndex = 8;
            volumeLabel.Text = "手数倍率:";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpMst);
            this.tcMain.Controls.Add(this.tpDet);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 25);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(486, 357);
            this.tcMain.TabIndex = 0;
            // 
            // tpMst
            // 
            this.tpMst.Controls.Add(this.gvData);
            this.tpMst.Location = new System.Drawing.Point(4, 22);
            this.tpMst.Name = "tpMst";
            this.tpMst.Padding = new System.Windows.Forms.Padding(3);
            this.tpMst.Size = new System.Drawing.Size(478, 331);
            this.tpMst.TabIndex = 0;
            this.tpMst.Text = "概要";
            this.tpMst.UseVisualStyleBackColor = true;
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            this.gvData.AutoGenerateColumns = false;
            this.gvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvData.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.subUserIdDataGridViewTextBoxColumn,
            this.instrumentDataGridViewTextBoxColumn,
            this.mstUserIdDataGridViewTextBoxColumn,
            this.gcPrice,
            this.volumeDataGridViewTextBoxColumn,
            this.isInverseDataGridViewCheckBoxColumn});
            this.gvData.DataSource = this.dsData;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(3, 3);
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowTemplate.Height = 23;
            this.gvData.Size = new System.Drawing.Size(472, 325);
            this.gvData.TabIndex = 0;
            this.gvData.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentDoubleClick);
            // 
            // dsData
            // 
            this.dsData.DataSource = typeof(WinCtp.UserInserOrderConfig);
            // 
            // tpDet
            // 
            this.tpDet.AutoScroll = true;
            this.tpDet.Controls.Add(volumeLabel);
            this.tpDet.Controls.Add(this.volumeTextBox);
            this.tpDet.Controls.Add(subUserIdLabel);
            this.tpDet.Controls.Add(this.subUserIdTextBox);
            this.tpDet.Controls.Add(mstUserIdLabel);
            this.tpDet.Controls.Add(this.mstUserIdTextBox);
            this.tpDet.Controls.Add(this.isInverseCheckBox);
            this.tpDet.Controls.Add(instrumentLabel);
            this.tpDet.Controls.Add(this.instrumentTextBox);
            this.tpDet.Location = new System.Drawing.Point(4, 22);
            this.tpDet.Name = "tpDet";
            this.tpDet.Padding = new System.Windows.Forms.Padding(3);
            this.tpDet.Size = new System.Drawing.Size(478, 331);
            this.tpDet.TabIndex = 1;
            this.tpDet.Text = "明细";
            this.tpDet.UseVisualStyleBackColor = true;
            // 
            // volumeTextBox
            // 
            this.volumeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsData, "Volume", true));
            this.volumeTextBox.Location = new System.Drawing.Point(74, 111);
            this.volumeTextBox.Name = "volumeTextBox";
            this.volumeTextBox.ReadOnly = true;
            this.volumeTextBox.Size = new System.Drawing.Size(100, 21);
            this.volumeTextBox.TabIndex = 9;
            this.volumeTextBox.Tag = "New Edit";
            // 
            // subUserIdTextBox
            // 
            this.subUserIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsData, "SubUserId", true));
            this.subUserIdTextBox.Location = new System.Drawing.Point(74, 14);
            this.subUserIdTextBox.Name = "subUserIdTextBox";
            this.subUserIdTextBox.ReadOnly = true;
            this.subUserIdTextBox.Size = new System.Drawing.Size(100, 21);
            this.subUserIdTextBox.TabIndex = 7;
            this.subUserIdTextBox.Tag = "New";
            // 
            // mstUserIdTextBox
            // 
            this.mstUserIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsData, "MstUserId", true));
            this.mstUserIdTextBox.Location = new System.Drawing.Point(74, 44);
            this.mstUserIdTextBox.Name = "mstUserIdTextBox";
            this.mstUserIdTextBox.ReadOnly = true;
            this.mstUserIdTextBox.Size = new System.Drawing.Size(100, 21);
            this.mstUserIdTextBox.TabIndex = 5;
            this.mstUserIdTextBox.Tag = "New";
            // 
            // isInverseCheckBox
            // 
            this.isInverseCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dsData, "IsInverse", true));
            this.isInverseCheckBox.Enabled = false;
            this.isInverseCheckBox.Location = new System.Drawing.Point(204, 12);
            this.isInverseCheckBox.Name = "isInverseCheckBox";
            this.isInverseCheckBox.Size = new System.Drawing.Size(104, 24);
            this.isInverseCheckBox.TabIndex = 3;
            this.isInverseCheckBox.Tag = "New Edit";
            this.isInverseCheckBox.Text = "是否反向";
            this.isInverseCheckBox.UseVisualStyleBackColor = true;
            // 
            // instrumentTextBox
            // 
            this.instrumentTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsData, "Instrument", true));
            this.instrumentTextBox.Location = new System.Drawing.Point(74, 75);
            this.instrumentTextBox.Name = "instrumentTextBox";
            this.instrumentTextBox.ReadOnly = true;
            this.instrumentTextBox.Size = new System.Drawing.Size(100, 21);
            this.instrumentTextBox.TabIndex = 1;
            this.instrumentTextBox.Tag = "New Edit";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ibtnMst,
            this.toolStripSeparator2,
            this.ibtnNew,
            this.ibtnEdit,
            this.ibtnCancel,
            this.ibtnSave,
            this.toolStripSeparator1,
            this.ibtnDelete,
            this.toolStripSeparator3,
            this.ibtnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(486, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // ibtnMst
            // 
            this.ibtnMst.Image = global::WinCtp.Properties.Resources._5;
            this.ibtnMst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnMst.Name = "ibtnMst";
            this.ibtnMst.Size = new System.Drawing.Size(53, 22);
            this.ibtnMst.Tag = "Det";
            this.ibtnMst.Text = "概要";
            this.ibtnMst.Click += new System.EventHandler(this.ibtnMst_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ibtnNew
            // 
            this.ibtnNew.Image = global::WinCtp.Properties.Resources._new;
            this.ibtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnNew.Name = "ibtnNew";
            this.ibtnNew.Size = new System.Drawing.Size(53, 22);
            this.ibtnNew.Tag = "Mst Det";
            this.ibtnNew.Text = "新建";
            this.ibtnNew.Click += new System.EventHandler(this.ibtnNew_Click);
            // 
            // ibtnEdit
            // 
            this.ibtnEdit.Image = global::WinCtp.Properties.Resources.edit;
            this.ibtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnEdit.Name = "ibtnEdit";
            this.ibtnEdit.Size = new System.Drawing.Size(53, 22);
            this.ibtnEdit.Tag = "Mst Det";
            this.ibtnEdit.Text = "编辑";
            this.ibtnEdit.Click += new System.EventHandler(this.ibtnEdit_Click);
            // 
            // ibtnCancel
            // 
            this.ibtnCancel.Image = global::WinCtp.Properties.Resources.undo;
            this.ibtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnCancel.Name = "ibtnCancel";
            this.ibtnCancel.Size = new System.Drawing.Size(53, 22);
            this.ibtnCancel.Tag = "Edit";
            this.ibtnCancel.Text = "取消";
            this.ibtnCancel.Click += new System.EventHandler(this.ibtnCancel_Click);
            // 
            // ibtnSave
            // 
            this.ibtnSave.Image = global::WinCtp.Properties.Resources.save;
            this.ibtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnSave.Name = "ibtnSave";
            this.ibtnSave.Size = new System.Drawing.Size(53, 22);
            this.ibtnSave.Tag = "Edit";
            this.ibtnSave.Text = "保存";
            this.ibtnSave.Click += new System.EventHandler(this.ibtnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ibtnDelete
            // 
            this.ibtnDelete.Image = global::WinCtp.Properties.Resources.delete;
            this.ibtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnDelete.Name = "ibtnDelete";
            this.ibtnDelete.Size = new System.Drawing.Size(53, 22);
            this.ibtnDelete.Tag = "Mst Det";
            this.ibtnDelete.Text = "删除";
            this.ibtnDelete.Click += new System.EventHandler(this.ibtnDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ibtnClose
            // 
            this.ibtnClose.Image = global::WinCtp.Properties.Resources.close;
            this.ibtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ibtnClose.Name = "ibtnClose";
            this.ibtnClose.Size = new System.Drawing.Size(53, 22);
            this.ibtnClose.Text = "关闭";
            this.ibtnClose.Click += new System.EventHandler(this.ibtnClose_Click);
            // 
            // subUserIdDataGridViewTextBoxColumn
            // 
            this.subUserIdDataGridViewTextBoxColumn.DataPropertyName = "SubUserId";
            this.subUserIdDataGridViewTextBoxColumn.HeaderText = "子账户";
            this.subUserIdDataGridViewTextBoxColumn.Name = "subUserIdDataGridViewTextBoxColumn";
            this.subUserIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.subUserIdDataGridViewTextBoxColumn.Width = 66;
            // 
            // instrumentDataGridViewTextBoxColumn
            // 
            this.instrumentDataGridViewTextBoxColumn.DataPropertyName = "Instrument";
            this.instrumentDataGridViewTextBoxColumn.HeaderText = "品种";
            this.instrumentDataGridViewTextBoxColumn.Name = "instrumentDataGridViewTextBoxColumn";
            this.instrumentDataGridViewTextBoxColumn.ReadOnly = true;
            this.instrumentDataGridViewTextBoxColumn.Width = 54;
            // 
            // mstUserIdDataGridViewTextBoxColumn
            // 
            this.mstUserIdDataGridViewTextBoxColumn.DataPropertyName = "MstUserId";
            this.mstUserIdDataGridViewTextBoxColumn.HeaderText = "主账户";
            this.mstUserIdDataGridViewTextBoxColumn.Name = "mstUserIdDataGridViewTextBoxColumn";
            this.mstUserIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.mstUserIdDataGridViewTextBoxColumn.Width = 66;
            // 
            // gcPrice
            // 
            this.gcPrice.DataPropertyName = "Price";
            this.gcPrice.HeaderText = "价格";
            this.gcPrice.Name = "gcPrice";
            this.gcPrice.ReadOnly = true;
            this.gcPrice.Width = 54;
            // 
            // volumeDataGridViewTextBoxColumn
            // 
            this.volumeDataGridViewTextBoxColumn.DataPropertyName = "Volume";
            this.volumeDataGridViewTextBoxColumn.HeaderText = "手数倍率";
            this.volumeDataGridViewTextBoxColumn.Name = "volumeDataGridViewTextBoxColumn";
            this.volumeDataGridViewTextBoxColumn.ReadOnly = true;
            this.volumeDataGridViewTextBoxColumn.Width = 78;
            // 
            // isInverseDataGridViewCheckBoxColumn
            // 
            this.isInverseDataGridViewCheckBoxColumn.DataPropertyName = "IsInverse";
            this.isInverseDataGridViewCheckBoxColumn.HeaderText = "是否反向";
            this.isInverseDataGridViewCheckBoxColumn.Name = "isInverseDataGridViewCheckBoxColumn";
            this.isInverseDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isInverseDataGridViewCheckBoxColumn.Width = 59;
            // 
            // FrmUserInsertOrderConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 382);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.toolStrip);
            this.Name = "FrmUserInsertOrderConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "下单配置";
            this.tcMain.ResumeLayout(false);
            this.tpMst.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsData)).EndInit();
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
        private System.Windows.Forms.ToolStripButton ibtnNew;
        private System.Windows.Forms.ToolStripButton ibtnEdit;
        private System.Windows.Forms.ToolStripButton ibtnCancel;
        private System.Windows.Forms.ToolStripButton ibtnSave;
        private DataGridViewEx gvData;
        private System.Windows.Forms.ToolStripButton ibtnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.BindingSource dsData;
        private System.Windows.Forms.TextBox volumeTextBox;
        private System.Windows.Forms.TextBox subUserIdTextBox;
        private System.Windows.Forms.TextBox mstUserIdTextBox;
        private System.Windows.Forms.CheckBox isInverseCheckBox;
        private System.Windows.Forms.TextBox instrumentTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn subUserIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn instrumentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mstUserIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isInverseDataGridViewCheckBoxColumn;
    }
}
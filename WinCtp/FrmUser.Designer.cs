namespace WinCtp
{
    partial class FrmUser
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
            System.Windows.Forms.Label brokerIdLabel;
            System.Windows.Forms.Label userIdLabel;
            System.Windows.Forms.Label userNameLabel;
            System.Windows.Forms.Label passwordLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUser));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ibtnMst = new System.Windows.Forms.ToolStripButton();
            this.ibtnNew = new System.Windows.Forms.ToolStripButton();
            this.ibtnEdit = new System.Windows.Forms.ToolStripButton();
            this.ibtnCancel = new System.Windows.Forms.ToolStripButton();
            this.ibtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ibtnClose = new System.Windows.Forms.ToolStripButton();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMst = new System.Windows.Forms.TabPage();
            this.gvAccount = new WinCtp.DataGridViewEx();
            this.userIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brokerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isSubDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dsUser = new System.Windows.Forms.BindingSource(this.components);
            this.tpDet = new System.Windows.Forms.TabPage();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.brokerIdComboBox = new System.Windows.Forms.ComboBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.userIdTextBox = new System.Windows.Forms.TextBox();
            this.isSubCheckBox = new System.Windows.Forms.CheckBox();
            brokerIdLabel = new System.Windows.Forms.Label();
            userIdLabel = new System.Windows.Forms.Label();
            userNameLabel = new System.Windows.Forms.Label();
            passwordLabel = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpMst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsUser)).BeginInit();
            this.tpDet.SuspendLayout();
            this.SuspendLayout();
            // 
            // brokerIdLabel
            // 
            brokerIdLabel.AutoSize = true;
            brokerIdLabel.Location = new System.Drawing.Point(19, 80);
            brokerIdLabel.Name = "brokerIdLabel";
            brokerIdLabel.Size = new System.Drawing.Size(59, 12);
            brokerIdLabel.TabIndex = 0;
            brokerIdLabel.Text = "期货公司:";
            // 
            // userIdLabel
            // 
            userIdLabel.AutoSize = true;
            userIdLabel.Location = new System.Drawing.Point(19, 24);
            userIdLabel.Name = "userIdLabel";
            userIdLabel.Size = new System.Drawing.Size(47, 12);
            userIdLabel.TabIndex = 4;
            userIdLabel.Text = "账户ID:";
            // 
            // userNameLabel
            // 
            userNameLabel.AutoSize = true;
            userNameLabel.Location = new System.Drawing.Point(19, 52);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new System.Drawing.Size(47, 12);
            userNameLabel.TabIndex = 6;
            userNameLabel.Text = "账户名:";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(235, 24);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(35, 12);
            passwordLabel.TabIndex = 8;
            passwordLabel.Text = "密码:";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ibtnMst,
            this.ibtnNew,
            this.ibtnEdit,
            this.ibtnCancel,
            this.ibtnSave,
            this.toolStripSeparator1,
            this.ibtnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(572, 25);
            this.toolStrip.TabIndex = 3;
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
            this.ibtnEdit.Image = ((System.Drawing.Image)(resources.GetObject("ibtnEdit.Image")));
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpMst);
            this.tcMain.Controls.Add(this.tpDet);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 25);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(572, 337);
            this.tcMain.TabIndex = 4;
            // 
            // tpMst
            // 
            this.tpMst.Controls.Add(this.gvAccount);
            this.tpMst.Location = new System.Drawing.Point(4, 22);
            this.tpMst.Name = "tpMst";
            this.tpMst.Padding = new System.Windows.Forms.Padding(3);
            this.tpMst.Size = new System.Drawing.Size(564, 311);
            this.tpMst.TabIndex = 0;
            this.tpMst.Text = "概要";
            this.tpMst.UseVisualStyleBackColor = true;
            // 
            // gvAccount
            // 
            this.gvAccount.AllowUserToAddRows = false;
            this.gvAccount.AllowUserToDeleteRows = false;
            this.gvAccount.AutoGenerateColumns = false;
            this.gvAccount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvAccount.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAccount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userIdDataGridViewTextBoxColumn,
            this.userNameDataGridViewTextBoxColumn,
            this.brokerIdDataGridViewTextBoxColumn,
            this.isSubDataGridViewCheckBoxColumn});
            this.gvAccount.DataSource = this.dsUser;
            this.gvAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAccount.Location = new System.Drawing.Point(3, 3);
            this.gvAccount.Name = "gvAccount";
            this.gvAccount.ReadOnly = true;
            this.gvAccount.RowTemplate.Height = 23;
            this.gvAccount.Size = new System.Drawing.Size(558, 305);
            this.gvAccount.TabIndex = 0;
            this.gvAccount.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAccount_CellContentDoubleClick);
            // 
            // userIdDataGridViewTextBoxColumn
            // 
            this.userIdDataGridViewTextBoxColumn.DataPropertyName = "UserId";
            this.userIdDataGridViewTextBoxColumn.HeaderText = "账户ID";
            this.userIdDataGridViewTextBoxColumn.Name = "userIdDataGridViewTextBoxColumn";
            this.userIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.userIdDataGridViewTextBoxColumn.Width = 66;
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "账户名";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.userNameDataGridViewTextBoxColumn.Width = 66;
            // 
            // brokerIdDataGridViewTextBoxColumn
            // 
            this.brokerIdDataGridViewTextBoxColumn.DataPropertyName = "BrokerId";
            this.brokerIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.brokerIdDataGridViewTextBoxColumn.HeaderText = "期货公司";
            this.brokerIdDataGridViewTextBoxColumn.Name = "brokerIdDataGridViewTextBoxColumn";
            this.brokerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.brokerIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.brokerIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.brokerIdDataGridViewTextBoxColumn.Width = 78;
            // 
            // isSubDataGridViewCheckBoxColumn
            // 
            this.isSubDataGridViewCheckBoxColumn.DataPropertyName = "IsSub";
            this.isSubDataGridViewCheckBoxColumn.HeaderText = "子账户";
            this.isSubDataGridViewCheckBoxColumn.Name = "isSubDataGridViewCheckBoxColumn";
            this.isSubDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isSubDataGridViewCheckBoxColumn.Width = 47;
            // 
            // dsUser
            // 
            this.dsUser.DataSource = typeof(WinCtp.UserInfo);
            // 
            // tpDet
            // 
            this.tpDet.Controls.Add(passwordLabel);
            this.tpDet.Controls.Add(this.passwordTextBox);
            this.tpDet.Controls.Add(this.brokerIdComboBox);
            this.tpDet.Controls.Add(userNameLabel);
            this.tpDet.Controls.Add(this.userNameTextBox);
            this.tpDet.Controls.Add(userIdLabel);
            this.tpDet.Controls.Add(this.userIdTextBox);
            this.tpDet.Controls.Add(this.isSubCheckBox);
            this.tpDet.Controls.Add(brokerIdLabel);
            this.tpDet.Location = new System.Drawing.Point(4, 22);
            this.tpDet.Name = "tpDet";
            this.tpDet.Padding = new System.Windows.Forms.Padding(3);
            this.tpDet.Size = new System.Drawing.Size(564, 311);
            this.tpDet.TabIndex = 1;
            this.tpDet.Text = "明细";
            this.tpDet.UseVisualStyleBackColor = true;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsUser, "Password", true));
            this.passwordTextBox.Location = new System.Drawing.Point(276, 21);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.ReadOnly = true;
            this.passwordTextBox.Size = new System.Drawing.Size(127, 21);
            this.passwordTextBox.TabIndex = 9;
            this.passwordTextBox.Tag = "New Edit";
            // 
            // brokerIdComboBox
            // 
            this.brokerIdComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.dsUser, "BrokerId", true));
            this.brokerIdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.brokerIdComboBox.FormattingEnabled = true;
            this.brokerIdComboBox.Location = new System.Drawing.Point(91, 80);
            this.brokerIdComboBox.Name = "brokerIdComboBox";
            this.brokerIdComboBox.Size = new System.Drawing.Size(129, 20);
            this.brokerIdComboBox.TabIndex = 8;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsUser, "UserName", true));
            this.userNameTextBox.Location = new System.Drawing.Point(91, 49);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.ReadOnly = true;
            this.userNameTextBox.Size = new System.Drawing.Size(129, 21);
            this.userNameTextBox.TabIndex = 7;
            this.userNameTextBox.Tag = "New Edit";
            // 
            // userIdTextBox
            // 
            this.userIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsUser, "UserId", true));
            this.userIdTextBox.Location = new System.Drawing.Point(91, 21);
            this.userIdTextBox.Name = "userIdTextBox";
            this.userIdTextBox.ReadOnly = true;
            this.userIdTextBox.Size = new System.Drawing.Size(129, 21);
            this.userIdTextBox.TabIndex = 5;
            this.userIdTextBox.Tag = "New";
            // 
            // isSubCheckBox
            // 
            this.isSubCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dsUser, "IsSub", true));
            this.isSubCheckBox.Enabled = false;
            this.isSubCheckBox.Location = new System.Drawing.Point(237, 52);
            this.isSubCheckBox.Name = "isSubCheckBox";
            this.isSubCheckBox.Size = new System.Drawing.Size(104, 24);
            this.isSubCheckBox.TabIndex = 3;
            this.isSubCheckBox.Tag = "New Edit";
            this.isSubCheckBox.Text = "子账户";
            this.isSubCheckBox.UseVisualStyleBackColor = true;
            // 
            // FrmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 362);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "账户管理";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpMst.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsUser)).EndInit();
            this.tpDet.ResumeLayout(false);
            this.tpDet.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewEx gvAccount;
        private System.Windows.Forms.BindingSource dsUser;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ibtnSave;
        private System.Windows.Forms.ToolStripButton ibtnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ibtnNew;
        private System.Windows.Forms.ToolStripButton ibtnClose;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpMst;
        private System.Windows.Forms.TabPage tpDet;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox userIdTextBox;
        private System.Windows.Forms.CheckBox isSubCheckBox;
        private System.Windows.Forms.ToolStripButton ibtnEdit;
        private System.Windows.Forms.ToolStripButton ibtnMst;
        private System.Windows.Forms.ComboBox brokerIdComboBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn brokerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isSubDataGridViewCheckBoxColumn;
    }
}
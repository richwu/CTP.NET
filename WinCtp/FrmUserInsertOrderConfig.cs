using System;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmUserInsertOrderConfig : Form
    {
        private UserInserOrderConfig Current
        {
            get { return (UserInserOrderConfig)dsData.Current; }
            set
            {
                if (value == null)
                    dsData.RemoveCurrent();
                else dsData[dsData.Position] = value;
                dsData.ResetCurrentItem();
            }
        }

        private UserInserOrderConfig _orig;

        public FrmUserInsertOrderConfig()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ToMst();
            dsData.DataSource = UserInserOrderConfig.GetAll();
        }

        private void ToMst()
        {
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
        }

        private void ToDet()
        {
            toolStrip.SetStatus("Det");
            tcMain.ShowPage(tpDet);
        }

        private void ToEdit()
        {
            toolStrip.SetStatus("Edit");
            tcMain.ShowPage(tpDet);
        }

        private void ibtnMst_Click(object sender, EventArgs e)
        {
            ToMst();
        }

        private void ibtnNew_Click(object sender, EventArgs e)
        {
            _orig = null;
            var obj = new UserInserOrderConfig();
            dsData.Position = dsData.Add(obj);
            dsData.ResetCurrentItem();
            ToEdit();
            tpDet.SetEditable(true, "New");
        }

        private void ibtnEdit_Click(object sender, EventArgs e)
        {
            if (dsData.Current == null)
                return;
            ToEdit();
            tpDet.SetEditable(true, "Edit");
            _orig = Current.Clone();
        }

        private void ibtnCancel_Click(object sender, EventArgs e)
        {
            Current = _orig;
            tpDet.SetEditable(false);
            if(_orig == null)
                ToMst();
            _orig = null;
        }

        private void ibtnSave_Click(object sender, EventArgs e)
        {
            gvData.EndEdit();
            dsData.EndEdit();
            var cur = Current;
            if (string.IsNullOrWhiteSpace(cur.MstUserId))
            {
                MsgBox.Info("请输入主账户");
                return;
            }
            if (string.IsNullOrWhiteSpace(cur.SubUserId))
            {
                MsgBox.Info("请输入子账户");
                return;
            }
            if (_orig == null)
                cur.Save();
            else cur.Update();
            tpDet.SetEditable(false);
            _orig = null;
            ToDet();
        }

        private void ibtnDelete_Click(object sender, EventArgs e)
        {
            if (dsData.Current == null)
                return;
            Current.Delete();
            Current = null;
            ToMst();
        }

        private void ibtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dsData.Current == null)
                return;
            ToDet();
        }
    }
}

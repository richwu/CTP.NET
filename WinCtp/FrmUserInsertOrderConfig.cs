using System;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmUserInsertOrderConfig : Form
    {
        private UserInserOrderConfig _orig;

        public FrmUserInsertOrderConfig()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tcMain.ShowPage(tpMst);
            toolStrip.SetStatus("Mst");
            dsData.DataSource = UserInserOrderConfig.GetAll();
        }

        private void ibtnMst_Click(object sender, EventArgs e)
        {
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
        }

        private void ibtnNew_Click(object sender, EventArgs e)
        {
            _orig = null;
            var obj = new UserInserOrderConfig();
            dsData.Position = dsData.Add(obj);
            toolStrip.SetStatus("Edit");
            tpDet.SetEditable(true, "New");
            tcMain.ShowPage(tpDet);
        }

        private void ibtnEdit_Click(object sender, EventArgs e)
        {
            if (dsData.Current == null)
                return;
            var cur = (UserInserOrderConfig)dsData.Current;
            _orig = cur.Clone();
            toolStrip.SetStatus("Edit");
            tpDet.SetEditable(true, "Edit");
            tcMain.ShowPage(tpDet);
        }

        private void ibtnCancel_Click(object sender, EventArgs e)
        {
            if (_orig == null)
            {
                dsData.RemoveCurrent();
                toolStrip.SetStatus("Mst");
                tcMain.ShowPage(tpMst);
            }
            else
            {
                dsData[dsData.Position] = _orig;
                dsData.ResetCurrentItem();
                _orig = null;
                toolStrip.SetStatus("Det");
            }
            tpDet.SetEditable(false);
        }

        private void ibtnSave_Click(object sender, EventArgs e)
        {
            gvData.EndEdit();
            dsData.EndEdit();
            var cur = (UserInserOrderConfig)dsData.Current;
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

            _orig = null;
            dsData.ResetCurrentItem();
            toolStrip.SetStatus("Det");
            tpDet.SetEditable(false);
        }

        private void ibtnDelete_Click(object sender, EventArgs e)
        {
            if (dsData.Current == null)
                return;
            if (!MsgBox.Ask("确定删除？"))
                return;
            var cur = (UserInserOrderConfig)dsData.Current;
            cur.Delete();
            dsData.RemoveCurrent();
            dsData.ResetBindings(false);
            tcMain.ShowPage(tpMst);
            toolStrip.SetStatus("Mst");
        }

        private void ibtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dsData.Current == null)
                return;
            toolStrip.SetStatus("Det");
            tcMain.ShowPage(tpDet);
        }
    }
}

using System;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmUser : Form
    {
        private UserInfo _orig;

        public FrmUser()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tcMain.ShowPage(tpMst);
            toolStrip.SetStatus("Mst");
            var brokers = BrokerInfo.GetAll();
            brokerIdDataGridViewTextBoxColumn.Bind(brokers);
            brokerIdComboBox.Bind(brokers);
            dsUser.DataSource = UserInfo.GetAll();
        }

        private void ibtnSave_Click(object sender, EventArgs e)
        {
            gvAccount.EndEdit();
            dsUser.EndEdit();
            var ds = (UserInfo)dsUser.Current;
            if (string.IsNullOrWhiteSpace(ds.UserId))
            {
                MsgBox.Info("请输入账户ID");
                return;
            }
            if (string.IsNullOrWhiteSpace(ds.BrokerId))
            {
                MsgBox.Info("请选择期货公司");
                return;
            }
            if (_orig == null)
            {
                if (ds.Exists())
                {
                    MsgBox.Info("账户ID已存在");
                    return;
                }
                ds.Save();
            }
            else ds.Update();
            _orig = null;
            dsUser.ResetCurrentItem();
            toolStrip.SetStatus("Det");
            tpDet.SetEditable(false);
        }

        private void ibtnCancel_Click(object sender, EventArgs e)
        {
            if (_orig == null)
            {
                dsUser.RemoveCurrent();
                toolStrip.SetStatus("Mst");
                tcMain.ShowPage(tpMst);
            }
            else
            {
                dsUser[dsUser.Position] = _orig;
                dsUser.ResetCurrentItem();
                _orig = null;
                toolStrip.SetStatus("Det");
            }
            tpDet.SetEditable(false);            
        }

        private void ibtnNew_Click(object sender, EventArgs e)
        {
            _orig = null;
            var obj = new UserInfo();
            dsUser.Position = dsUser.Add(obj);
            toolStrip.SetStatus("Edit");
            tpDet.SetEditable(true,"New");
            tcMain.ShowPage(tpDet);
        }

        private void ibtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ibtnEdit_Click(object sender, EventArgs e)
        {
            if (dsUser.Current == null)
                return;
            var cur = (UserInfo)dsUser.Current;
            _orig = cur.Clone();
            toolStrip.SetStatus("Edit");
            tpDet.SetEditable(true, "Edit");
            tcMain.ShowPage(tpDet);
        }

        private void gvAccount_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dsUser.Current == null)
                return;
            toolStrip.SetStatus("Det");
            tcMain.ShowPage(tpDet);
        }

        private void ibtnMst_Click(object sender, EventArgs e)
        {
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
        }

        private void ibtnDelete_Click(object sender, EventArgs e)
        {
            if (dsUser.Current == null)
                return;
            var user = (UserInfo) dsUser.Current;
            if (!MsgBox.Ask($"您确定删除账户[{user.UserId}]?"))
                return;
            user.Delete();
            dsUser.RemoveCurrent();
            dsUser.ResetBindings(false);
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
        }
    }
}

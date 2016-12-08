using System;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmBroker : Form
    {
        private BrokerInfo _orig;

        public FrmBroker()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
            dsBroker.DataSource = BrokerInfo.GetAll();
        }

        private void ibtnMst_Click(object sender, EventArgs e)
        {
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
        }

        private void ibtnNew_Click(object sender, EventArgs e)
        {
            _orig = null;
            var obj = new BrokerInfo();
            dsBroker.Position = dsBroker.Add(obj);
            dsBroker.ResetCurrentItem();
            toolStrip.SetStatus("Edit");
            tcMain.ShowPage(tpDet);
            tpDet.SetEditable(true, "New");
        }

        private void ibtnEdit_Click(object sender, EventArgs e)
        {
            var obj = (BrokerInfo) dsBroker.Current;
            if (obj == null)
                return;
            tcMain.ShowPage(tpDet);
            toolStrip.SetStatus("Edit");
            tpDet.SetEditable(true, "Edit");
            _orig = obj.Clone();
        }

        private void ibtnCancel_Click(object sender, EventArgs e)
        {
            if (_orig == null)
            {
                dsBroker.RemoveCurrent();
                toolStrip.SetStatus("Mst");
                tcMain.ShowPage(tpMst);
            }
            else
            {
                dsBroker[dsBroker.Position] = _orig;
                dsBroker.ResetCurrentItem();
                toolStrip.SetStatus("Det");
            }
            tpDet.SetEditable(false);
            _orig = null;
        }

        private void ibtnSave_Click(object sender, EventArgs e)
        {
            gvBroker.EndEdit();
            dsBroker.EndEdit();
            var obj = (BrokerInfo)dsBroker.Current;
            if (_orig == null)
                obj.Save();
            else obj.Update();
            tpDet.SetEditable(false);
            toolStrip.SetStatus("Det");
        }

        private void ibtnDelete_Click(object sender, EventArgs e)
        {
            var obj = (BrokerInfo)dsBroker.Current;
            if (obj == null)
                return;
            obj.Delete();
            dsBroker.RemoveCurrent();
            dsBroker.ResetBindings(false);
            toolStrip.SetStatus("Mst");
            tcMain.ShowPage(tpMst);
        }

        private void ibtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvBroker_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dsBroker.Current == null)
                return;
            toolStrip.SetStatus("Det");
            tcMain.ShowPage(tpDet);
        }
    }
}

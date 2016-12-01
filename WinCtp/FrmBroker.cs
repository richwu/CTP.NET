using System;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmBroker : Form
    {
        private CtpBroker Current
        {
            get { return (CtpBroker)dsBroker.Current; }
            set
            {
                if (value == null)
                    dsBroker.RemoveCurrent();
                else dsBroker[dsBroker.Position] = value;
                dsBroker.ResetCurrentItem();
            }
        }

        private CtpBroker _orig;

        public FrmBroker()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ToMst();
            dsBroker.DataSource = CtpBroker.GetAll();
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
            var obj = new CtpBroker();
            dsBroker.Position = dsBroker.Add(obj);
            dsBroker.ResetCurrentItem();
            ToEdit();
            tpDet.SetEditable(true, "New");
        }

        private void ibtnEdit_Click(object sender, EventArgs e)
        {
            if (dsBroker.Current == null)
                return;
            ToEdit();
            tpDet.SetEditable(true, "Edit");
            _orig = Current.Clone();
        }

        private void ibtnCancel_Click(object sender, EventArgs e)
        {
            Current = _orig;
            tpDet.SetEditable(false);
            if (_orig == null)
                ToMst();
            _orig = null;
        }

        private void ibtnSave_Click(object sender, EventArgs e)
        {
            gvBroker.EndEdit();
            dsBroker.EndEdit();
            var cur = Current;
            if (_orig == null)
                cur.Save();
            else cur.Update();
            tpDet.SetEditable(false);
        }

        private void ibtnDelete_Click(object sender, EventArgs e)
        {
            if (dsBroker.Current == null)
                return;
            Current.Delete();
            Current = null;
            ToMst();
        }

        private void ibtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvBroker_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dsBroker.Current == null)
                return;
            ToDet();
        }
    }
}

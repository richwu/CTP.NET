using System;
using System.IO;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmRegister : Form
    {
        public FrmRegister()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtCpuId.Text = HardwareInfo.CupId;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;
            var s = dlg.FileName;
            var d = Path.Combine(Application.StartupPath, Path.GetFileName(s));
            File.Copy(s, d, true);
            MsgBox.Info("已注册");
        }
    }
}

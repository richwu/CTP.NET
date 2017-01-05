using System;
using System.Configuration;
using System.Windows.Forms;

namespace WinCtp
{
    public partial class FrmSetting : Form
    {
        public FrmSetting()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
             var followIntervalStr = ConfigurationManager.AppSettings["follow.interval"];
            var followInterval = 0;
            if (!string.IsNullOrWhiteSpace(followIntervalStr))
            {
                try { followInterval = Convert.ToInt32(followIntervalStr); }
                catch
                { // ignored
                }
            }
            numFollowInterval.Value = followInterval;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfg.AppSettings.Settings["follow.interval"].Value = ((int)numFollowInterval.Value).ToString();
            cfg.Save(ConfigurationSaveMode.Modified);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CTPTractor
{
	[System.Drawing.ToolboxBitmap(typeof(DraggableTabControl))]
	public class DraggableTabControl : System.Windows.Forms.TabControl
	{
		private Container components = null;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();

		public DraggableTabControl()
		{
			this.InitializeComponent();
			this.AllowDrop = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.ResumeLayout(false);
		}

		protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			System.Drawing.Point point = new System.Drawing.Point(e.X, e.Y);
			System.Drawing.Point point2 = base.PointToScreen(point);
			System.Windows.Forms.TabPage tabPageByTab = this.GetTabPageByTab(point);
			System.Windows.Forms.Form form = new System.Windows.Forms.Form();
			if (tabPageByTab != null)
			{
				foreach (System.Windows.Forms.Control control in tabPageByTab.Controls)
				{
					if (control is System.Windows.Forms.Form)
					{
						form = (control as System.Windows.Forms.Form);
						tabPageByTab.Controls.Remove(form);
						tabPageByTab.Dispose();
						form.TopLevel = true;
						form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
						form.Show();
					}
				}
			}
		}

		private System.Windows.Forms.TabPage GetTabPageByTab(System.Drawing.Point pt)
		{
			System.Windows.Forms.TabPage result = null;
			for (int i = 0; i < base.TabPages.Count; i++)
			{
				if (base.GetTabRect(i).Contains(pt))
				{
					result = base.TabPages[i];
					break;
				}
			}
			return result;
		}

		private int FindIndex(System.Windows.Forms.TabPage page)
		{
			int result;
			for (int i = 0; i < base.TabPages.Count; i++)
			{
				if (base.TabPages[i] == page)
				{
					result = i;
					return result;
				}
			}
			result = -1;
			return result;
		}
	}
}

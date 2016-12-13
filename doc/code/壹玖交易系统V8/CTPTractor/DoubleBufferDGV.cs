using System;
using System.Windows.Forms;

namespace CTPTractor
{
	internal class DoubleBufferDGV : System.Windows.Forms.DataGridView
	{
		public DoubleBufferDGV()
		{
			base.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.DoubleBuffer | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
			base.UpdateStyles();
		}
	}
}

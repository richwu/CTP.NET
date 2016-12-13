using DevExpress.Data;
using System;
using System.Windows.Forms;

namespace CTPTractor
{
	internal static class Program
	{
		[System.STAThread]
		private static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			CurrencyDataController.DisableThreadingProblemsDetection = true;
			System.Windows.Forms.Application.Run(new Login());
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcFrontStatusField
	{
		public int FrontID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string LastReportDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string LastReportTime;

		public int IsActive;
	}
}

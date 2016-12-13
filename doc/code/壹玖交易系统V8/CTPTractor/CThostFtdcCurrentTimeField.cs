using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcCurrentTimeField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string CurrDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string CurrTime;

		public int CurrMillisec;
	}
}

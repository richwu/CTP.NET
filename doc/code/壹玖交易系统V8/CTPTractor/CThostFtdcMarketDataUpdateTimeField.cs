using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcMarketDataUpdateTimeField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string UpdateTime;

		public int UpdateMillisec;
	}
}

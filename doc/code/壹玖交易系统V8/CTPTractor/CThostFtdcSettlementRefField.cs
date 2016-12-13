using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcSettlementRefField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public int SettlementID;
	}
}

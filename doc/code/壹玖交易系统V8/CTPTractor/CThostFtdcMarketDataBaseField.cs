using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcMarketDataBaseField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public double PreSettlementPrice;

		public double PreClosePrice;

		public double PreOpenInterest;

		public double PreDelta;
	}
}

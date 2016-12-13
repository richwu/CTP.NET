using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class PositionDetailField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
		public string TradeID;

		public DirectionType Direction;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string OpenDate;

		public double OpenPrice;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public double MarginRateByMoney;

		public int Volume;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;
	}
}

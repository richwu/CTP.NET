using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class TradeField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string TradeID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string ExchangeID;

		public DirectionType Direction;

		public EOffsetType Offset;

		public HedgeType Hedge;

		public double Price;

		public int Volume;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string TradeTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string TradingDay;

		public int OrderID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string OrderRef;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
		public string OrderSysID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
		public string TradeType;
	}
}

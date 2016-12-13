using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class SettlementInfoField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public int SettlementID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		public int SequenceNo;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 501)]
		public string Content;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class UserLoginField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string LoginTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
		public string SystemName;

		public int FrontID;

		public int SessionID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string MaxOrderRef;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string SHFETime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string DCETime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string CZCETime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string FFEXTime;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class InstrumentMarginRateField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		public double LongMarginRatioByMoney;

		public double LongMarginRatioByVolume;

		public double ShortMarginRatioByMoney;

		public double ShortMarginRatioByVolume;

		public int IsRelative;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;
	}
}

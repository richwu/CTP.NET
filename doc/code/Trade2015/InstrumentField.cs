using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class InstrumentField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string ProductID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string ExchangeID;

		public int VolumeMultiple;

		public double PriceTick;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
		public string InstrumentName;

		public double LongMarginRatio;

		public double ShortMarginRatio;

		public int MaxLimitOrderVolume;

		public int MinMarketOrderVolume;
	}
}

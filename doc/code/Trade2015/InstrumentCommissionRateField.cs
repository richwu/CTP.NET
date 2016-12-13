using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class InstrumentCommissionRateField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		public double OpenRatioByMoney;

		public double OpenRatioByVolume;

		public double CloseRatioByMoney;

		public double CloseRatioByVolume;

		public double CloseTodayRatioByMoney;

		public double CloseTodayRatioByVolume;
	}
}

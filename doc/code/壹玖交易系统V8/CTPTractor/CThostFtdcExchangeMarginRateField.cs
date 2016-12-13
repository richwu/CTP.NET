using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcExchangeMarginRateField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		public EnumHedgeFlagType HedgeFlag;

		public double LongMarginRatioByMoney;

		public double LongMarginRatioByVolume;

		public double ShortMarginRatioByMoney;

		public double ShortMarginRatioByVolume;
	}
}

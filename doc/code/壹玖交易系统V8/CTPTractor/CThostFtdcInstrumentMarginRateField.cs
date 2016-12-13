using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcInstrumentMarginRateField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		public EnumInvestorRangeType InvestorRange;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		public EnumHedgeFlagType HedgeFlag;

		public double LongMarginRatioByMoney;

		public double LongMarginRatioByVolume;

		public double ShortMarginRatioByMoney;

		public double ShortMarginRatioByVolume;

		public EnumBoolType IsRelative;
	}
}

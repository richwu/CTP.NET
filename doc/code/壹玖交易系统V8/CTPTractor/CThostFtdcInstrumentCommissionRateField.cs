using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcInstrumentCommissionRateField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		public EnumInvestorRangeType InvestorRange;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		public double OpenRatioByMoney;

		public double OpenRatioByVolume;

		public double CloseRatioByMoney;

		public double CloseRatioByVolume;

		public double CloseTodayRatioByMoney;

		public double CloseTodayRatioByVolume;
	}
}

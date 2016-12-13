using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcMarketDataField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExchangeID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string ExchangeInstID;

		public double LastPrice;

		public double PreSettlementPrice;

		public double PreClosePrice;

		public double PreOpenInterest;

		public double OpenPrice;

		public double HighestPrice;

		public double LowestPrice;

		public int Volume;

		public double Turnover;

		public double OpenInterest;

		public double ClosePrice;

		public double SettlementPrice;

		public double UpperLimitPrice;

		public double LowerLimitPrice;

		public double PreDelta;

		public double CurrDelta;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string UpdateTime;

		public int UpdateMillisec;
	}
}

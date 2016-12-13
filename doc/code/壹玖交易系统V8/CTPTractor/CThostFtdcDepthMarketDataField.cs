using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcDepthMarketDataField
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

		public double BidPrice1;

		public int BidVolume1;

		public double AskPrice1;

		public int AskVolume1;

		public double BidPrice2;

		public int BidVolume2;

		public double AskPrice2;

		public int AskVolume2;

		public double BidPrice3;

		public int BidVolume3;

		public double AskPrice3;

		public int AskVolume3;

		public double BidPrice4;

		public int BidVolume4;

		public double AskPrice4;

		public int AskVolume4;

		public double BidPrice5;

		public int BidVolume5;

		public double AskPrice5;

		public int AskVolume5;

		public double AveragePrice;
	}
}

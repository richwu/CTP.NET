using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcInvestorPositionDetailField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		public EnumHedgeFlagType HedgeFlag;

		public EnumDirectionType Direction;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string OpenDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 21)]
		public string TradeID;

		public int Volume;

		public double OpenPrice;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public int SettlementID;

		public EnumTradeTypeType TradeType;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string CombInstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExchangeID;

		public double CloseProfitByDate;

		public double CloseProfitByTrade;

		public double PositionProfitByDate;

		public double PositionProfitByTrade;

		public double Margin;

		public double ExchMargin;

		public double MarginRateByMoney;

		public double MarginRateByVolume;

		public double LastSettlementPrice;

		public double SettlementPrice;

		public int CloseVolume;

		public double CloseAmount;
	}
}

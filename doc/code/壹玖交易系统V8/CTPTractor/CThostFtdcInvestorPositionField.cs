using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcInvestorPositionField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		public EnumPosiDirectionType PosiDirection;

		public EnumHedgeFlagType HedgeFlag;

		public EnumPositionDateType PositionDate;

		public int YdPosition;

		public int Position;

		public int LongFrozen;

		public int ShortFrozen;

		public double LongFrozenAmount;

		public double ShortFrozenAmount;

		public int OpenVolume;

		public int CloseVolume;

		public double OpenAmount;

		public double CloseAmount;

		public double PositionCost;

		public double PreMargin;

		public double UseMargin;

		public double FrozenMargin;

		public double FrozenCash;

		public double FrozenCommission;

		public double CashIn;

		public double Commission;

		public double CloseProfit;

		public double PositionProfit;

		public double PreSettlementPrice;

		public double SettlementPrice;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public int SettlementID;

		public double OpenCost;

		public double ExchangeMargin;

		public int CombPosition;

		public int CombLongFrozen;

		public int CombShortFrozen;

		public double CloseProfitByDate;

		public double CloseProfitByTrade;

		public int TodayPosition;

		public double MarginRateByMoney;

		public double MarginRateByVolume;
	}
}

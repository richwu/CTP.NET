using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcBrokerWithdrawAlgorithmField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		public EnumAlgorithmType WithdrawAlgorithm;

		public double UsingRatio;

		public EnumIncludeCloseProfitType IncludeCloseProfit;

		public EnumAllWithoutTradeType AllWithoutTrade;

		public EnumIncludeCloseProfitType AvailIncludeCloseProfit;

		public int IsBrokerUserEvent;
	}
}

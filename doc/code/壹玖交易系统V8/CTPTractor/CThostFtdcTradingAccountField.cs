using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcTradingAccountField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string AccountID;

		public double PreMortgage;

		public double PreCredit;

		public double PreDeposit;

		public double PreBalance;

		public double PreMargin;

		public double InterestBase;

		public double Interest;

		public double Deposit;

		public double Withdraw;

		public double FrozenMargin;

		public double FrozenCash;

		public double FrozenCommission;

		public double CurrMargin;

		public double CashIn;

		public double Commission;

		public double CloseProfit;

		public double PositionProfit;

		public double Balance;

		public double Available;

		public double WithdrawQuota;

		public double Reserve;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string TradingDay;

		public int SettlementID;

		public double Credit;

		public double Mortgage;

		public double ExchangeMargin;

		public double DeliveryMargin;

		public double ExchangeDeliveryMargin;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class TradingAccount
	{
		public double PreBalance;

		public double PositionProfit;

		public double CloseProfit;

		public double Commission;

		public double CurrMargin;

		public double FrozenCash;

		public double Available;

		public double Balance;

		public double Deposit;

		public double Withdraw;

		public double FrozenMargin;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string AccountID;
	}
}

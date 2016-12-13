using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcDiscountField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		public EnumInvestorRangeType InvestorRange;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		public double Discount;
	}
}

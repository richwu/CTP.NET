using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcTransferFutureToBankRspField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 5)]
		public string RetCode;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 129)]
		public string RetInfo;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string FutureAccount;

		public double TradeAmt;

		public double CustFee;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 4)]
		public string CurrencyCode;
	}
}

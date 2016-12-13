using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcQrySyncDepositField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 15)]
		public string DepositSeqNo;
	}
}

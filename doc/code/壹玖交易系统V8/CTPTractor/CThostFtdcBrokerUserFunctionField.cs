using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcBrokerUserFunctionField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;

		public EnumBrokerFunctionCodeType BrokerFunctionCode;
	}
}

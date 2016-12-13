using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcQryBrokerUserFunctionField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;
	}
}

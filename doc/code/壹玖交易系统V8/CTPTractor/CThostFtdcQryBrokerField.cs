using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcQryBrokerField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;
	}
}

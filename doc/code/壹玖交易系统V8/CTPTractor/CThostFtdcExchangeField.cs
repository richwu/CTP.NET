using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcExchangeField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExchangeID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string ExchangeName;

		public EnumExchangePropertyType ExchangeProperty;
	}
}

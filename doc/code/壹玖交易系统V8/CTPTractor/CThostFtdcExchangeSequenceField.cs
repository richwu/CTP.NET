using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcExchangeSequenceField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExchangeID;

		public int SequenceNo;

		public EnumInstrumentStatusType MarketStatus;
	}
}

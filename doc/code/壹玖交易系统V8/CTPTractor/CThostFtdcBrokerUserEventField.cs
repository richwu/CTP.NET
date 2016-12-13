using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcBrokerUserEventField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;

		public EnumUserEventTypeType UserEventType;

		public int EventSequenceNo;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string EventDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string EventTime;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string UserEventInfo;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;
	}
}

using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcNoticeField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 501)]
		public string Content;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
		public string SequenceLabel;
	}
}

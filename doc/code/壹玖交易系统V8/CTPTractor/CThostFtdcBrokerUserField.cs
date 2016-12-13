using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcBrokerUserField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 81)]
		public string UserName;

		public EnumUserTypeType UserType;

		public int IsActive;

		public int IsUsingOTP;
	}
}

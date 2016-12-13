using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcLogoutAllField
	{
		public int FrontID;

		public int SessionID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 41)]
		public string SystemName;
	}
}

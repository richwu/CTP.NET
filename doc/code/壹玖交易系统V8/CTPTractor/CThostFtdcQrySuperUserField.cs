using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcQrySuperUserField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;
	}
}

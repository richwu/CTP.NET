using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcReturnResultField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 7)]
		public string ReturnCode;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 129)]
		public string DescrInfoForReturnCode;
	}
}

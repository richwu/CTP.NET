using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcSuperUserFunctionField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;

		public EnumFunctionCodeType FunctionCode;
	}
}

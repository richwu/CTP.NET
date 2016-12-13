using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcRspInfoField
	{
		public int ErrorID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 81)]
		public string ErrorMsg;
	}
}

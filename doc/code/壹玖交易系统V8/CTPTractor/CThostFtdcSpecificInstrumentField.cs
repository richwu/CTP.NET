using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcSpecificInstrumentField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;
	}
}

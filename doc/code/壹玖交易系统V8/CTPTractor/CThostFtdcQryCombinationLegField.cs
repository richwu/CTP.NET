using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcQryCombinationLegField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string CombInstrumentID;

		public int LegID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string LegInstrumentID;
	}
}

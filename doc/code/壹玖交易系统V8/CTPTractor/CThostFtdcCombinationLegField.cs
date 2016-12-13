using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcCombinationLegField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string CombInstrumentID;

		public int LegID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string LegInstrumentID;

		public EnumDirectionType Direction;

		public int LegMultiple;

		public int ImplyLevel;
	}
}

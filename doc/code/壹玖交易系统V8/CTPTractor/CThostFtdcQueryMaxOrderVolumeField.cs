using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcQueryMaxOrderVolumeField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		public EnumDirectionType Direction;

		public EnumOffsetFlagType OffsetFlag;

		public EnumHedgeFlagType HedgeFlag;

		public int MaxVolume;
	}
}

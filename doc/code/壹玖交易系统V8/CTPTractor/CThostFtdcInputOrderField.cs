using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcInputOrderField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 13)]
		public string OrderRef;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UserID;

		public EnumOrderPriceTypeType OrderPriceType;

		public EnumDirectionType Direction;

		public EnumOffsetFlagType CombOffsetFlag_0;

		public EnumOffsetFlagType CombOffsetFlag_1;

		public EnumOffsetFlagType CombOffsetFlag_2;

		public EnumOffsetFlagType CombOffsetFlag_3;

		public EnumOffsetFlagType CombOffsetFlag_4;

		public EnumHedgeFlagType CombHedgeFlag_0;

		public EnumHedgeFlagType CombHedgeFlag_1;

		public EnumHedgeFlagType CombHedgeFlag_2;

		public EnumHedgeFlagType CombHedgeFlag_3;

		public EnumHedgeFlagType CombHedgeFlag_4;

		public double LimitPrice;

		public int VolumeTotalOriginal;

		public EnumTimeConditionType TimeCondition;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string GTDDate;

		public EnumVolumeConditionType VolumeCondition;

		public int MinVolume;

		public EnumContingentConditionType ContingentCondition;

		public double StopPrice;

		public EnumForceCloseReasonType ForceCloseReason;

		public int IsAutoSuspend;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 21)]
		public string BusinessUnit;

		public int RequestID;

		public int UserForceClose;
	}
}

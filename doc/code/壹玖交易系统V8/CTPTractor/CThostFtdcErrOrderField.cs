using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcErrOrderField
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

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 5)]
		public string CombOffsetFlag;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 5)]
		public string CombHedgeFlag;

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

		public int ErrorID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 81)]
		public string ErrorMsg;
	}
}

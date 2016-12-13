using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class OrderField
	{
		public int OrderID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string OrderRef;

		public int FrontID;

		public int SessionID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string InsertDate;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
		public string StatusMsg;

		public int SequenceNo;

		public DirectionType Direction;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
		public string OrderSysID;

		public EOffsetType Offset;

		public double LimitPrice;

		public double AvgPrice;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string InsertTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string UpdateTime;

		public int VolumeTraded;

		public int VolumeTotalOriginal;

		public int VolumeTotal;

		public HedgeType Hedge;

		public OrderStatus Status;

		public bool IsLocal;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string Custom;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
		public string UserProductInfo;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;
	}
}

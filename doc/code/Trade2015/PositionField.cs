using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class PositionField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		public DirectionType Direction;

		public int Position;

		public int YdPosition;

		public int TdPosition;

		public double UseMargin;

		public HedgeType Hedge;

		public double PositionProfit;

		public double PositionCost;

		public double OpenCost;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
		public string BrokerID;

		public PositionDateType PositionDate;
	}
}

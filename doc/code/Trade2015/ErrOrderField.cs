using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class ErrOrderField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string OrderRef;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string InstrumentID;

		public DirectionType Direction;

		public EOffsetType Offset;

		public double LimitPrice;

		public int VolumeTotalOriginal;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;
	}
}

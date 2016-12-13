using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class InvestorField
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
		public string InvestorID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
		public string InvestorName;
	}
}

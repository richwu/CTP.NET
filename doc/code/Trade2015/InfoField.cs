using System;
using System.Runtime.InteropServices;

namespace Trade2015
{
	[StructLayout(LayoutKind.Sequential)]
	public class InfoField
	{
		public int ErrorID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
		public string ErrorMsg;
	}
}

using System;
using System.Runtime.InteropServices;

namespace ClsDiskInfo
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct GetVersionOutParams
	{
		public byte bVersion;

		public byte bRevision;

		public byte bReserved;

		public byte bIDEDeviceMap;

		public uint fCapabilities;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] dwReserved;
	}
}

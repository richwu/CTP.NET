using System;
using System.Runtime.InteropServices;

namespace ClsDiskInfo
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SendCmdInParams
	{
		public uint cBufferSize;

		public IdeRegs irDriveRegs;

		public byte bDriveNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] bReserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] dwReserved;

		public byte bBuffer;
	}
}

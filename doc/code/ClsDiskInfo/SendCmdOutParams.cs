using System;
using System.Runtime.InteropServices;

namespace ClsDiskInfo
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SendCmdOutParams
	{
		public uint cBufferSize;

		public DriverStatus DriverStatus;

		public IdSector bBuffer;
	}
}

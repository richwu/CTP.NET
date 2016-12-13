using System;
using System.Runtime.InteropServices;

namespace ClsDiskInfo
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct IdeRegs
	{
		public byte bFeaturesReg;

		public byte bSectorCountReg;

		public byte bSectorNumberReg;

		public byte bCylLowReg;

		public byte bCylHighReg;

		public byte bDriveHeadReg;

		public byte bCommandReg;

		public byte bReserved;
	}
}

using System;

namespace ClsDiskInfo
{
	[Serializable]
	public struct HardDiskInfo
	{
		public string ModuleNumber;

		public string Firmware;

		public string SerialNumber;

		public uint Capacity;
	}
}

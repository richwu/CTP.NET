using System;

namespace CTPTractor
{
	public enum EnumOffsetFlagType : byte
	{
		Open = 48,
		Close,
		ForceClose,
		CloseToday,
		CloseYesterday,
		ForceOff,
		LocalForceClose
	}
}

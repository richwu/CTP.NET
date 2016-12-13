using System;

namespace CTPTractor
{
	public enum EnumSystemStatusType : byte
	{
		NonActive = 49,
		Startup,
		Initialize,
		Initialized,
		Close,
		Closed,
		Settlement
	}
}

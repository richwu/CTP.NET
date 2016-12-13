using System;

namespace CTPTractor
{
	public enum EnumBrokerRepealFlagType : byte
	{
		BrokerNotNeedRepeal = 48,
		BrokerWaitingRepeal,
		BrokerBeenRepealed
	}
}

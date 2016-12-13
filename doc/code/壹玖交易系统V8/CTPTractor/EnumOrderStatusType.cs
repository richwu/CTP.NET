using System;

namespace CTPTractor
{
	public enum EnumOrderStatusType : byte
	{
		AllTraded = 48,
		PartTradedQueueing,
		PartTradedNotQueueing,
		NoTradeQueueing,
		NoTradeNotQueueing,
		Canceled,
		Unknown = 97,
		NotTouched,
		Touched,
		Ordered
	}
}

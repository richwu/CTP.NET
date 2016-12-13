using System;

namespace Trade2015
{
	public enum OrderStatus
	{
		Unknown,
		PartTradedQueueing,
		AllTraded,
		NoTradeQueueing,
		NoTradeNotQueueing,
		PartTradedNotQueueing,
		NotTouched,
		Touched,
		Canceled,
		Ordered
	}
}

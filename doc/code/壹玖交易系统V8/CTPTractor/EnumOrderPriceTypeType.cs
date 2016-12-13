using System;

namespace CTPTractor
{
	public enum EnumOrderPriceTypeType : byte
	{
		AnyPrice = 49,
		LimitPrice,
		BestPrice,
		LastPrice,
		LastPricePlusOneTicks,
		LastPricePlusTwoTicks,
		LastPricePlusThreeTicks,
		AskPrice1,
		AskPrice1PlusOneTicks,
		AskPrice1PlusTwoTicks = 65,
		AskPrice1PlusThreeTicks,
		BidPrice1,
		BidPrice1PlusOneTicks,
		BidPrice1PlusTwoTicks,
		BidPrice1PlusThreeTicks
	}
}

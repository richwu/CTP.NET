using System;

namespace CTPTractor
{
	public enum EnumContingentConditionType : byte
	{
		Immediately = 49,
		Touch,
		TouchProfit,
		ParkedOrder,
		LastPriceGreaterThanStopPrice,
		LastPriceGreaterEqualStopPrice,
		LastPriceLesserThanStopPrice,
		LastPriceLesserEqualStopPrice,
		AskPriceGreaterThanStopPrice,
		AskPriceGreaterEqualStopPrice = 65,
		AskPriceLesserThanStopPrice,
		AskPriceLesserEqualStopPrice,
		BidPriceGreaterThanStopPrice,
		BidPriceGreaterEqualStopPrice,
		BidPriceLesserThanStopPrice,
		BidPriceLesserEqualStopPrice = 72
	}
}

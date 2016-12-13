using System;

namespace CTPTractor
{
	public enum EnumInstrumentStatusType : byte
	{
		BeforeTrading = 48,
		NoTrading,
		Continous,
		AuctionOrdering,
		AuctionBalance,
		AuctionMatch,
		Closed
	}
}

using System;

namespace CTPTractor
{
	public enum EnumOrderTypeType : byte
	{
		Normal = 48,
		DeriveFromQuote,
		DeriveFromCombination,
		Combination,
		ConditionalOrder,
		Swap
	}
}

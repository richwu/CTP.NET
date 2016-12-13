using System;

namespace CTPTractor
{
	public enum EnumUserEventTypeType : byte
	{
		All = 32,
		Login = 49,
		Logout,
		Trading,
		TradingError,
		UpdatePassword,
		Other = 57
	}
}

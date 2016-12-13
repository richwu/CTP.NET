using System;

namespace CTPTractor
{
	public enum EnumTraderConnectStatusType : byte
	{
		NotConnected = 49,
		Connected,
		QryInstrumentSent,
		SubPrivateFlow
	}
}

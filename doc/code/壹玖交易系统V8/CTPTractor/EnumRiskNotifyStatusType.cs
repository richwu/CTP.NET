using System;

namespace CTPTractor
{
	public enum EnumRiskNotifyStatusType : byte
	{
		NotGen = 48,
		Generated,
		SendError,
		SendOk,
		Received,
		Confirmed
	}
}

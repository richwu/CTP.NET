using System;

namespace CTPTractor
{
	public enum EnumSendTypeType : byte
	{
		NoSend = 48,
		Sended,
		Generated,
		SendFail,
		Success,
		Fail,
		Cancel
	}
}

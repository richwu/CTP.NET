using System;

namespace CTPTractor
{
	public enum EnumCfmmcReturnCodeType : byte
	{
		Success = 48,
		Working,
		InfoFail,
		IDCardFail,
		OtherFail
	}
}

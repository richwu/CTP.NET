using System;

namespace CTPTractor
{
	public enum EnumUpdateFlagType : byte
	{
		NoUpdate = 48,
		Success,
		Fail,
		TCSuccess,
		TCFail,
		Cancel
	}
}

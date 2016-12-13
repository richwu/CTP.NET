using System;

namespace CTPTractor
{
	public enum EnumForceCloseReasonType : byte
	{
		NotForceClose = 48,
		LackDeposit,
		ClientOverPositionLimit,
		MemberOverPositionLimit,
		NotMultiple,
		Violation,
		Other,
		PersonDeliv
	}
}

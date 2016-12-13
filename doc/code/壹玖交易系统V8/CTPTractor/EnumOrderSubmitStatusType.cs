using System;

namespace CTPTractor
{
	public enum EnumOrderSubmitStatusType : byte
	{
		InsertSubmitted = 48,
		CancelSubmitted,
		ModifySubmitted,
		Accepted,
		InsertRejected,
		CancelRejected,
		ModifyRejected
	}
}

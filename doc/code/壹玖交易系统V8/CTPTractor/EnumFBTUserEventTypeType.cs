using System;

namespace CTPTractor
{
	public enum EnumFBTUserEventTypeType : byte
	{
		SignIn = 48,
		FromBankToFuture,
		FromFutureToBank,
		OpenAccount,
		CancelAccount,
		ChangeAccount,
		RepealFromBankToFuture,
		RepealFromFutureToBank,
		QueryBankAccount,
		QueryFutureAccount,
		SignOut = 65,
		SyncKey,
		Other = 90
	}
}

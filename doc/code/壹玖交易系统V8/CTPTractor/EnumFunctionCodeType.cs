using System;

namespace CTPTractor
{
	public enum EnumFunctionCodeType : byte
	{
		DataAsync = 49,
		ForceUserLogout,
		UserPasswordUpdate,
		BrokerPasswordUpdate,
		InvestorPasswordUpdate,
		OrderInsert,
		OrderAction,
		SyncSystemData,
		SyncBrokerData,
		BachSyncBrokerData = 65,
		SuperQuery,
		ParkedOrderInsert,
		ParkedOrderAction,
		SyncOTP
	}
}

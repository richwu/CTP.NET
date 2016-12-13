using System;

namespace CTPTractor
{
	public enum EnumFileBusinessCodeType : byte
	{
		Others = 48,
		TransferDetails,
		CustAccStatus,
		AccountTradeDetails,
		FutureAccountChangeInfoDetails,
		CustMoneyDetail,
		CustCancelAccountInfo,
		CustMoneyResult,
		OthersExceptionResult,
		CustInterestNetMoneyDetails,
		CustMoneySendAndReceiveDetails = 97,
		CorporationMoneyTotal,
		MainbodyMoneyTotal,
		MainPartMonitorData,
		PreparationMoney,
		BankMoneyMonitorData
	}
}

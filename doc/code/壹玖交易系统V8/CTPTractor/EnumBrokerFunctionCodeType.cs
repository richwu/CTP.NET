using System;

namespace CTPTractor
{
	public enum EnumBrokerFunctionCodeType : byte
	{
		ForceUserLogout = 49,
		UserPasswordUpdate,
		SyncBrokerData,
		BachSyncBrokerData,
		OrderInsert,
		OrderAction,
		AllQuery,
		log = 97,
		BaseQry,
		TradeQry,
		Trade,
		Virement,
		Risk,
		Session,
		RiskNoticeCtl,
		RiskNotice,
		BrokerDeposit,
		QueryFund,
		QueryOrder,
		QueryTrade,
		QueryPosition,
		QueryMarketData,
		QueryUserEvent,
		QueryRiskNotify,
		QueryFundChange,
		QueryInvestor,
		QueryTradingCode,
		ForceClose,
		PressTest,
		RemainCalc,
		NetPositionInd,
		RiskPredict,
		DataExport,
		RiskTargetSetup = 65,
		MarketDataWarn,
		SyncOTP = 69
	}
}

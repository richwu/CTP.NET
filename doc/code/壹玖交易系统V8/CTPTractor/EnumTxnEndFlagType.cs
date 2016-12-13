using System;

namespace CTPTractor
{
	public enum EnumTxnEndFlagType : byte
	{
		NormalProcessing = 48,
		Success,
		Failed,
		Abnormal,
		ManualProcessedForException,
		CommuFailedNeedManualProcess,
		SysErrorNeedManualProcess
	}
}

using System;

namespace CTPTractor
{
	public enum EnumOrganStatusType : byte
	{
		Ready = 48,
		CheckIn,
		CheckOut,
		CheckFileArrived,
		CheckDetail,
		DayEndClean,
		Invalid = 57
	}
}

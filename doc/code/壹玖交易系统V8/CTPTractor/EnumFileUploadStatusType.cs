using System;

namespace CTPTractor
{
	public enum EnumFileUploadStatusType : byte
	{
		SucceedUpload = 49,
		FailedUpload,
		SucceedLoad,
		PartSucceedLoad,
		FailedLoad
	}
}

using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcVerifyCustInfoField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 51)]
		public string CustomerName;

		public EnumIdCardTypeType IdCardType;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 51)]
		public string IdentifiedCardNo;

		public EnumCustTypeType CustType;
	}
}

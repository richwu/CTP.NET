using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcProductField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string ProductID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 21)]
		public string ProductName;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExchangeID;

		public EnumProductClassType ProductClass;

		public int VolumeMultiple;

		public double PriceTick;

		public int MaxMarketOrderVolume;

		public int MinMarketOrderVolume;

		public int MaxLimitOrderVolume;

		public int MinLimitOrderVolume;

		public EnumPositionTypeType PositionType;

		public EnumPositionDateTypeType PositionDateType;
	}
}

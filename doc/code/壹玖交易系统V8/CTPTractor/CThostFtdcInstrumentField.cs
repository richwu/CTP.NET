using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public struct CThostFtdcInstrumentField
	{
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string InstrumentID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExchangeID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 21)]
		public string InstrumentName;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string ExchangeInstID;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 31)]
		public string ProductID;

		public EnumProductClassType ProductClass;

		public int DeliveryYear;

		public int DeliveryMonth;

		public int MaxMarketOrderVolume;

		public int MinMarketOrderVolume;

		public int MaxLimitOrderVolume;

		public int MinLimitOrderVolume;

		public int VolumeMultiple;

		public double PriceTick;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string CreateDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string OpenDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string ExpireDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string StartDelivDate;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 9)]
		public string EndDelivDate;

		public EnumInstLifePhaseType InstLifePhase;

		public int IsTrading;

		public EnumPositionTypeType PositionType;

		public EnumPositionDateTypeType PositionDateType;

		public double LongMarginRatio;

		public double ShortMarginRatio;
	}
}

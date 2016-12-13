using System;

namespace Trade2015
{
	public class StatusEventArgs : EventArgs
	{
		public string Exchange = string.Empty;

		public ExchangeStatusType Status = ExchangeStatusType.Trading;
	}
}

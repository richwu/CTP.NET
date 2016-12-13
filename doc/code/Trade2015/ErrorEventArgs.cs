using System;

namespace Trade2015
{
	public class ErrorEventArgs : EventArgs
	{
		public int ErrorID = 0;

		public string ErrorMsg = string.Empty;
	}
}

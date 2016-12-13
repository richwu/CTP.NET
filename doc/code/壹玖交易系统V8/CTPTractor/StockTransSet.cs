using System;
using System.ComponentModel;

namespace CTPTractor
{
	internal class StockTransSet
	{
		public string _Number;

		public string _Variety;

		public string _Account;

		public double _Lots_Mul;

		public double _Lots;

		public double _LastLots;

		public double _NowLots;

		public string _Date;

		[Description("序号")]
		public string Number
		{
			get
			{
				return this._Number;
			}
			set
			{
				this._Number = value;
			}
		}

		[Description("合约名称")]
		public string Variety
		{
			get
			{
				return this._Variety;
			}
			set
			{
				this._Variety = value;
			}
		}

		[Description("投资者帐户")]
		public string Account
		{
			get
			{
				return this._Account;
			}
			set
			{
				this._Account = value;
			}
		}

		[Description("手数倍数")]
		public double LotsMul
		{
			get
			{
				return this._Lots_Mul;
			}
			set
			{
				this._Lots_Mul = value;
			}
		}

		[Description("读取手数")]
		public double Lots
		{
			get
			{
				return this._Lots;
			}
			set
			{
				this._Lots = value;
			}
		}

		[Description("前持仓")]
		public double LastLots
		{
			get
			{
				return this._LastLots;
			}
			set
			{
				this._LastLots = value;
			}
		}

		[Description("今持仓")]
		public double NowLots
		{
			get
			{
				return this._NowLots;
			}
			set
			{
				this._NowLots = value;
			}
		}

		[Description("时间")]
		public string Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				this._Date = value;
			}
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", new object[]
			{
				this.Number,
				this.Variety,
				this.Account,
				this.LotsMul,
				this.Lots,
				this.LastLots,
				this.NowLots,
				this.Date
			});
		}

		public string ToString(string name)
		{
			string result;
			switch (name)
			{
			case "序号":
				result = this.Number;
				return result;
			case "时间":
				result = this.Date;
				return result;
			case "手数倍数":
				result = this.LotsMul.ToString();
				return result;
			case "手数":
				result = this.Lots.ToString();
				return result;
			case "今持仓":
				result = this.NowLots.ToString();
				return result;
			case "前持仓":
				result = this.LastLots.ToString();
				return result;
			case "合约名称":
				result = this.Variety;
				return result;
			case "投资者帐户":
				result = this.Account;
				return result;
			}
			result = null;
			return result;
		}
	}
}

using System;

namespace CTPTractor
{
	public class MSOrderMode
	{
		public string 主账户 = "";

		public string 前置编号 = "";

		public string 会话编号 = "";

		public string 成交编号 = "";

		public string 报单编号 = "";

		public string 序列号 = "";

		public double 价格 = 0.0;

		public string 主状态 = "";

		public string 子账户 = "";

		public string 子前置编号 = "";

		public string 子会话编号 = "";

		public string 子序列号 = "";

		public string 子撤单时间 = "";

		public string 合约 = "";

		public string 买卖 = "";

		public string 开平 = "";

		public int 开多让点 = 0;

		public int 平多让点 = 0;

		public int 开空让点 = 0;

		public int 平空让点 = 0;

		public int 手数 = 0;

		public int 成交手数 = 0;

		public string 子价格 = "";

		public string 主开时间 = "";

		public string 本地时间 = "";

		public string 子下单时间 = "";

		public string 子状态 = "";

		public bool 子追单 = false;

		public bool 执行 = false;

		public bool candel = false;

		public string 撤单等待时间 = "";
	}
}

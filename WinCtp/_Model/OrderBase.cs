using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    public abstract class OrderBase
    {
        public string BrokerId { get; set; }

        /// <summary>
        /// 投资者ID。
        /// </summary>
        public string InvestorId { get; set; }

        /// <summary>
        /// 交易所代码。
        /// </summary>
        public string ExchangeId { get; set; }

        /// <summary>
        /// 报单编号。
        /// </summary>
        public string OrderSysId { get; set; }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentId { get; set; }

        /// <summary>
        /// 买卖。
        /// </summary>
        public byte Direction { get; set; }

        /// <summary>
        /// 跟单主键。
        /// </summary>
        public string FollowKey { get; set; }
    }
}
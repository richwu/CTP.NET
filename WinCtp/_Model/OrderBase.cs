
namespace WinCtp
{
    /// <summary>
    /// 期货单据。
    /// </summary>
    /// <remarks>报单、成交单。</remarks>
    public abstract class OrderBase
    {
        /// <summary>
        /// 经纪公司代码。
        /// </summary>
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
        /// 买卖方向。
        /// </summary>
        public byte Direction { get; set; }

        /// <summary>
        /// 跟单主键。
        /// </summary>
        public string FollowKey { get; set; }
    }
}
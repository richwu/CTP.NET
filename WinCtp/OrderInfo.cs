using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 报单信息。
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class OrderInfo
    {
        /// <summary>
        /// 请求ID。
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 投资者代码。
        /// </summary>
        public string InvestorId { get; set; }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentId { get; set; }

        /// <summary>
        /// 报单引用。
        /// </summary>
        public string OrderRef { get; set; }

        /// <summary>
        /// 交易所代码。
        /// </summary>
        public string ExchangeId { get; set; }

        /// <summary>
        /// 报单编号。
        /// </summary>
        public string OrderSysId { get; set; }

        /// <summary>
        /// 买卖。
        /// </summary>
        public byte Direction { get; set; }

        /// <summary>
        /// 价格。
        /// </summary>
        public double LimitPrice { get; set; }

        /// <summary>
        /// 止损价。
        /// </summary>
        public double StopPrice { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        public int VolumeTotalOriginal { get; set; }

        /// <summary>
        /// 今成交数量。
        /// </summary>
        public int VolumeTraded { get; set; }

        /// <summary>
        /// 剩余数量。
        /// </summary>
        public int VolumeTotal { get; set; }

        /// <summary>
        /// 报单日期。
        /// </summary>
        public string InsertDate { get; set; }

        /// <summary>
        /// 报单时间。
        /// </summary>
        public string InsertTime { get; set; }

        /// <summary>
        /// 最后修改时间。
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 报单状态。
        /// </summary>
        public byte OrderStatus { get; set; }

        /// <summary>
        /// 组合开平标识。
        /// </summary>
        public byte CombOffsetFlag { get; set; }

        public OrderInfo() { }

        public OrderInfo(CtpTrade ctpTrade)
        {
            InvestorId = ctpTrade.InvestorID;
            InstrumentId = ctpTrade.InstrumentID;
            OrderRef = ctpTrade.OrderRef;
            ExchangeId = ctpTrade.ExchangeID;
            OrderSysId = ctpTrade.OrderSysID;
            Direction = ctpTrade.Direction;
            LimitPrice = ctpTrade.Price;
            VolumeTotal = ctpTrade.Volume;
            CombOffsetFlag = ctpTrade.OffsetFlag;
        }

        public OrderInfo(CtpOrder ctpOrder)
        {
            
        }
    }
}
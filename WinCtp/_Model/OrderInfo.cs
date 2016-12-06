using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 报单信息。
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class OrderInfo : OrderBase
    {
        /// <summary>
        /// 请求ID。
        /// </summary>
        public int RequestId { get; set; }

        public int FrontId { get; set; }

        public int SessionId { get; set; }

        /// <summary>
        /// 报单引用。
        /// </summary>
        public string OrderRef { get; set; }

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
        public string CombOffsetFlag { get; set; }

        /// <summary>
        /// 异常消息。
        /// </summary>
        public string ErrorMsg { get; set; }

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
            CombOffsetFlag = ((char)ctpTrade.OffsetFlag).ToString();
        }

        public OrderInfo(CtpOrder ctpOrder)
        {
            
        }

        public OrderInfo(CtpInputOrder inputOrder)
        {
            BrokerId = inputOrder.BrokerID;
            InvestorId = inputOrder.InvestorID;
            InstrumentId = inputOrder.InstrumentID;
            Direction = inputOrder.Direction;
            RequestId = inputOrder.RequestID;
            OrderRef = inputOrder.OrderRef;
            LimitPrice = inputOrder.LimitPrice;
            StopPrice = inputOrder.StopPrice;
            OrderStatus = CtpOrderStatusType.Unknown;
            VolumeTotalOriginal = inputOrder.VolumeTotalOriginal;
            CombOffsetFlag = inputOrder.CombOffsetFlag;
        }
    }
}
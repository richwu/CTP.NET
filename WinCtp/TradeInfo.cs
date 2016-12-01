using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 成交单。
    /// </summary>
    public class TradeInfo
    {
        public string OrderSysId { get; set; }

        /// <summary>
        /// 投资者ID。
        /// </summary>
        public string InvestorId { get; set; }

        /// <summary>
        /// 合约。
        /// </summary>
        public string InstrumentId { get; set; }

        /// <summary>
        /// 交易所。
        /// </summary>
        public string ExchangeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TradeId { get; set; }

        public byte Direction { get; set; }

        public double Price { get; set; }

        public int Volume { get; set; }

        public string TradeDate { get; set; }

        public string TradeTime { get; set; }

        public string OrderLocalId { get; set; }

        public byte OffsetFlag { get; set; }

        public TradeInfo() { }

        public TradeInfo(CtpTrade ctp)
        {
            OrderSysId = ctp.OrderSysID;
            InvestorId = ctp.InvestorID;
            InstrumentId = ctp.InvestorID;
            ExchangeId = ctp.ExchangeID;
            TradeId = ctp.TradeID;
            Direction = ctp.Direction;
            Price = ctp.Price;
            Volume = ctp.Volume;
            TradeDate = ctp.TradeDate;
            TradeTime = ctp.TradeTime;
            OrderLocalId = ctp.OrderLocalID;
            OffsetFlag = ctp.OffsetFlag;
        }
    }
}
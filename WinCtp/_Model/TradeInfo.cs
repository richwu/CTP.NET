using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 成交单。
    /// </summary>
    public class TradeInfo : OrderBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string TradeId { get; set; }

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
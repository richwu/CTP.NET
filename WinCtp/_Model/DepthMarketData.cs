using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 深度行情
    /// </summary>
    public class DepthMarketData
    {
        /// <summary>
        /// 交易日
        /// </summary>
        public string TradingDay { get; set; }

        ///合约代码
        public string InstrumentId { get; set; }

        ///交易所代码
        public string ExchangeId { get; set; }

        ///合约在交易所的代码
        public string ExchangeInstId { get; set; }

        ///最新价
        public double LastPrice { get; set; }

        ///上次结算价
        public double PreSettlementPrice { get; set; }

        ///昨收盘
        public double PreClosePrice { get; set; }

        ///昨持仓量
        public double PreOpenInterest { get; set; }

        ///今开盘
        public double OpenPrice { get; set; }

        ///最高价
        public double HighestPrice { get; set; }

        ///最低价
        public double LowestPrice { get; set; }

        ///数量
        public int Volume { get; set; }

        ///成交金额
        public double Turnover { get; set; }

        ///持仓量
        public double OpenInterest { get; set; }

        ///今收盘
        public double ClosePrice { get; set; }

        ///本次结算价
        public double SettlementPrice { get; set; }

        ///涨停板价
        public double UpperLimitPrice { get; set; }

        ///跌停板价
        public double LowerLimitPrice { get; set; }

        ///昨虚实度
        public double PreDelta { get; set; }

        ///今虚实度
        public double CurrDelta { get; set; }

        ///最后修改时间
        public string UpdateTime { get; set; }

        ///最后修改毫秒
        public double UpdateMillisec { get; set; }

        ///申买价一
        public double BidPrice1 { get; set; }

        ///申买量一
        public double BidVolume1 { get; set; }

        ///申卖价一
        public double AskPrice1 { get; set; }

        ///申卖量一
        public double AskVolume1 { get; set; }

        ///申买价二
        public double BidPrice2 { get; set; }

        ///申买量二
        public double BidVolume2 { get; set; }

        ///申卖价二
        public double AskPrice2 { get; set; }

        ///申卖量二
        public double AskVolume2 { get; set; }

        ///申买价三
        public double BidPrice3 { get; set; }

        ///申买量三
        public double BidVolume3 { get; set; }

        ///申卖价三
        public double AskPrice3 { get; set; }

        ///申卖量三
        public double AskVolume3 { get; set; }

        ///申买价四
        public double BidPrice4 { get; set; }

        ///申买量四
        public double BidVolume4 { get; set; }

        ///申卖价四
        public double AskPrice4 { get; set; }

        ///申卖量四
        public double AskVolume4 { get; set; }

        ///申买价五
        public double BidPrice5 { get; set; }

        ///申买量五
        public double BidVolume5 { get; set; }

        ///申卖价五
        public double AskPrice5 { get; set; }

        ///申卖量五
        public double AskVolume5 { get; set; }

        ///当日均价
        public double AveragePrice { get; set; }

        ///业务日期
        public string ActionDay { get; set; }

        public DepthMarketData()
        {
            
        }

        public DepthMarketData(CtpDepthMarketData ctp)
        {
            TradingDay = ctp.TradingDay;
            InstrumentId = ctp.InstrumentID;
            ExchangeId = ctp.ExchangeID;
            ExchangeInstId = ctp.ExchangeInstID;

            LastPrice = ctp.LastPrice;
            PreSettlementPrice = ctp.PreSettlementPrice;
            PreClosePrice = ctp.PreClosePrice;
            PreOpenInterest = ctp.PreOpenInterest;
            OpenPrice = ctp.OpenPrice;
            HighestPrice = ctp.HighestPrice;
            LowestPrice = ctp.LowestPrice;
            Volume = ctp.Volume;
            Turnover = ctp.Turnover;
            OpenInterest = ctp.OpenInterest;
            ClosePrice = ctp.ClosePrice;
            SettlementPrice = ctp.SettlementPrice;
            UpperLimitPrice = ctp.UpperLimitPrice;
            LowerLimitPrice = ctp.LowerLimitPrice;
            PreDelta = ctp.PreDelta;
            CurrDelta = ctp.CurrDelta;
            UpdateTime = ctp.UpdateTime;
            UpdateMillisec = ctp.UpdateMillisec;

            BidPrice1 = ctp.BidPrice1;
            BidVolume1 = ctp.BidVolume1;
            AskPrice1 = ctp.AskPrice1;
            AskVolume1 = ctp.AskVolume1;

            BidPrice2 = ctp.BidPrice2;
            BidVolume2 = ctp.BidVolume2;
            AskPrice2 = ctp.AskPrice2;
            AskVolume2 = ctp.AskVolume2;

            BidPrice3 = ctp.BidPrice3;
            BidVolume3 = ctp.BidVolume3;
            AskPrice3 = ctp.AskPrice3;
            AskVolume3 = ctp.AskVolume3;

            BidPrice4 = ctp.BidPrice4;
            BidVolume4 = ctp.BidVolume4;
            AskPrice4 = ctp.AskPrice4;
            AskVolume4 = ctp.BidVolume4;

            BidPrice5 = ctp.BidPrice5;
            BidVolume5 = ctp.BidVolume5;
            AskPrice5 = ctp.AskPrice5;
            AskVolume5 = ctp.AskVolume5;

            AveragePrice = ctp.AveragePrice;
            ActionDay = ctp.ActionDay;
        }
    }
}
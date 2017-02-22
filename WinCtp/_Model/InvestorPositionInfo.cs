using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 投资者持仓。
    /// </summary>
    [System.Serializable]
    public class InvestorPositionInfo
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// 投机套保标志
        /// </summary>
        public byte HedgeFlag { get; set; }

        /// <summary>
        /// 持仓日期
        /// </summary>
        public byte PositionDate { get; set; }

        /// <summary>
        /// 上日持仓
        /// </summary>
        public int YdPosition { get; set; }

        /// <summary>
        /// 今日持仓
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 多头冻结
        /// </summary>
        public int LongFrozen { get; set; }

        /// <summary>
        /// 空头冻结
        /// </summary>
        public int ShortFrozen { get; set; }

        /// <summary>
        /// 开仓冻结金额
        /// </summary>
        public double LongFrozenAmount { get; set; }

        /// <summary>
        /// 开仓冻结金额
        /// </summary>
        public double ShortFrozenAmount { get; set; }

        /// <summary>
        /// 平仓量
        /// </summary>
        public int CloseVolume { get; set; }

        /// <summary>
        /// 开仓金额
        /// </summary>
        public double OpenAmount { get; set; }

        /// <summary>
        /// 平仓金额
        /// </summary>
        public double CloseAmount { get; set; }

        /// <summary>
        /// 上次占用的保证金
        /// </summary>
        public double PreMargin { get; set; }

        /// <summary>
        /// 占用的保证金
        /// </summary>
        public double UseMargin { get; set; }

        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin { get; set; }

        /// <summary>
        /// 冻结的资金
        /// </summary>
        public double FrozenCash { get; set; }

        /// <summary>
        /// 冻结的手续费
        /// </summary>
        public double FrozenCommission { get; set; }

        /// <summary>
        /// 资金差额
        /// </summary>
        public double CashIn { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public double Commission { get; set; }

        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit { get; set; }

        /// <summary>
        /// 上次结算价
        /// </summary>
        public double PreSettlementPrice { get; set; }

        /// <summary>
        /// 本次结算价
        /// </summary>
        public double SettlementPrice { get; set; }

        /// <summary>
        /// 交易日
        /// </summary>
        public string TradingDay { get; set; }

        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementId { get; set; }

        /// <summary>
        /// 开仓成本
        /// </summary>
        public double OpenCost { get; set; }

        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchangeMargin { get; set; }

        /// <summary>
        /// 组合成交形成的持仓
        /// </summary>
        public double CombPosition { get; set; }

        /// <summary>
        /// 组合多头冻结
        /// </summary>
        public double CombLongFrozen { get; set; }

        /// <summary>
        /// 组合空头冻结
        /// </summary>
        public double CombShortFrozen { get; set; }

        /// <summary>
        /// 逐日盯市平仓盈亏
        /// </summary>
        public double CloseProfitByDate { get; set; }

        /// <summary>
        /// 逐笔对冲平仓盈亏
        /// </summary>
        public double CloseProfitByTrade { get; set; }

        /// <summary>
        /// 今日持仓
        /// </summary>
        public int TodayPosition { get; set; }

        /// <summary>
        /// 保证金率
        /// </summary>
        public double MarginRateByMoney { get; set; }

        /// <summary>
        /// 保证金率(按手数)
        /// </summary>
        public double MarginRateByVolume { get; set; }

        /// <summary>
        /// 执行冻结
        /// </summary>
        public int StrikeFrozen { get; set; }

        /// <summary>
        /// 执行冻结金额
        /// </summary>
        public double StrikeFrozenAmount { get; set; }

        /// <summary>
        /// 放弃执行冻结
        /// </summary>
        public double AbandonFrozen { get; set; }

        /// <summary>
        /// 交易所代码
        /// </summary>
        public string ExchangeId { get; set; }

        /// <summary>
        /// 执行冻结的昨仓
        /// </summary>
        public double YdStrikeFrozen { get; set; }

        /// <summary>
        /// 投资者代码。
        /// </summary>
        public string InvestorId { get; set; }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentId { get; set; }

        /// <summary>
        /// 持仓多空方向。
        /// </summary>
        public byte PosiDirection { get; set; }

        /// <summary>
        /// 开仓量。
        /// </summary>
        public int OpenVolume { get; set; }

        /// <summary>
        /// 持仓成本。
        /// </summary>
        public double PositionCost { get; set; }

        /// <summary>
        /// 持仓盈亏。
        /// </summary>
        public double PositionProfit { get; set; }

        public InvestorPositionInfo() { }

        public InvestorPositionInfo(CtpInvestorPosition ctp)
        {
            InvestorId = ctp.InvestorID;
            InstrumentId = ctp.InstrumentID;
            PosiDirection = ctp.PosiDirection;
            OpenVolume = ctp.OpenVolume;
            PositionCost = ctp.PositionCost;
            PositionProfit = ctp.PositionProfit;
            BrokerId = ctp.BrokerID;
            HedgeFlag = ctp.HedgeFlag;
            PositionDate = ctp.PositionDate;
            YdPosition = ctp.YdPosition;
            Position = ctp.Position;
            LongFrozen = ctp.LongFrozen;
            ShortFrozen = ctp.ShortFrozen;
            LongFrozenAmount = ctp.LongFrozenAmount;
            ShortFrozenAmount = ctp.ShortFrozenAmount;
            CloseVolume = ctp.CloseVolume;
            OpenAmount = ctp.OpenAmount;
            CloseAmount = ctp.CloseAmount;
            PreMargin = ctp.PreMargin;
            UseMargin = ctp.UseMargin;
            FrozenMargin = ctp.FrozenMargin;
            FrozenCash = ctp.FrozenCash;
            FrozenCommission = ctp.FrozenCommission;
            CashIn = ctp.CashIn;
            Commission = ctp.Commission;
            CloseProfit = ctp.CloseProfit;
            PreSettlementPrice = ctp.PreSettlementPrice;
            SettlementPrice = ctp.SettlementPrice;
            TradingDay = ctp.TradingDay;
            SettlementId = ctp.SettlementID;
            OpenCost = ctp.OpenCost;
            ExchangeMargin = ctp.ExchangeMargin;
            CombPosition = ctp.CombPosition;
            CombLongFrozen = ctp.CombLongFrozen;
            CombShortFrozen = ctp.CombShortFrozen;
            CloseProfitByDate = ctp.CloseProfitByDate;
            CloseProfitByTrade = ctp.CloseProfitByTrade;
            TodayPosition = ctp.TodayPosition;
            MarginRateByMoney = ctp.MarginRateByMoney;
            MarginRateByVolume = ctp.MarginRateByVolume;
            StrikeFrozen = ctp.StrikeFrozen;
            StrikeFrozenAmount = ctp.StrikeFrozenAmount;
            AbandonFrozen = ctp.AbandonFrozen;
        }
    }
}
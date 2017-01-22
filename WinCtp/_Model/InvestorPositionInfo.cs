using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 持仓。
    /// </summary>
    [System.Serializable]
    public class InvestorPositionInfo
    {
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
        }
    }
}
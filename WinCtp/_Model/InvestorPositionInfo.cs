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
        /// 投资者账户。
        /// </summary>
        public string InvestorId { get; set; }

        /// <summary>
        /// 合约。
        /// </summary>
        public string InstrumentId { get; set; }

        public InvestorPositionInfo() { }

        public InvestorPositionInfo(CtpInvestorPosition ctp)
        {
            InvestorId = ctp.InvestorID;
            InstrumentId = ctp.InstrumentID;
        }
    }
}
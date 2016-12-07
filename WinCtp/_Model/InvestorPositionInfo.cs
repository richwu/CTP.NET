using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 持仓。
    /// </summary>
    public class InvestorPositionInfo
    {
        public string InvestorId { get; set; }

        public string InstrumentId { get; set; }

        public InvestorPositionInfo() { }

        public InvestorPositionInfo(CtpInvestorPosition ctp)
        {
            InvestorId = ctp.InvestorID;
            InstrumentId = ctp.InstrumentID;
        }
    }
}
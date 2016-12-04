using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 持仓。
    /// </summary>
    public class InvestorPositionInfo
    {
        public double CashIn { get; set; }

        public string InstrumentId { get; set; }

        public InvestorPositionInfo() { }

        public InvestorPositionInfo(CtpInvestorPosition ctp)
        {
            CashIn = ctp.CashIn;
            InstrumentId = ctp.InstrumentID;
        }
    }
}
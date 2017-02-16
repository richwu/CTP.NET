using Amib.Threading;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    public interface IMainView
    {
        void NotifyMdStatus(int flag,string msg = null);

        void NotifyTrade(CtpTrade trade);

        void Run(WorkItemCallback callback);
    }
}
using System.Collections.Concurrent;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    public static class DataCache
    {
        public static ConcurrentDictionary<string, CtpDepthMarketData> DepthMarketData { get; private set; }

        static DataCache()
        {
            DepthMarketData = new ConcurrentDictionary<string, CtpDepthMarketData>();
        }
    }
}
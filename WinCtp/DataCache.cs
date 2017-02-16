using System.Collections.Concurrent;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    public static class DataCache
    {
        public static ConcurrentDictionary<string, DepthMarketData> DepthMarketData { get; private set; }

        public static ConcurrentDictionary<string, CtpInstrument> Instrument { get; private set; }

        static DataCache()
        {
            DepthMarketData = new ConcurrentDictionary<string, DepthMarketData>();
            Instrument = new ConcurrentDictionary<string, CtpInstrument>();
        }
    }
}
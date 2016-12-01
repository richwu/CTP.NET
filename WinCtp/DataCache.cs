using System;
using System.Runtime.Caching;

namespace WinCtp
{
    public class DataCache
    {
        private ObjectCache _cache;
        private readonly CacheItemPolicy _policy;

        public DataCache()
        {
            _cache = new MemoryCache("CTP");
            _policy = new CacheItemPolicy();
            _policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now,new TimeSpan(999999999));
        }

        public void Set()
        {
            _cache.Add("", "", new DateTimeOffset());
        }
    }
}
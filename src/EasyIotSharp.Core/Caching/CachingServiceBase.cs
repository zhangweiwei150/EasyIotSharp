using StackExchange.Redis;
using UPrime;
using UPrime.Runtime.Caching;
using UPrime.Runtime.Caching.Redis;

namespace EasyIotSharp.Core.Caching
{
    /// <summary>
    /// 缓存服务基类
    /// </summary>
    public abstract class CachingServiceBase
    {
        protected readonly ICacheManager CacheManager;
        protected readonly IDatabase Database;

        public CachingServiceBase()
        {
            CacheManager = UPrimeEngine.Instance.Resolve<ICacheManagerProvider>().GetCacheManager();
            Database = UPrimeEngine.Instance.Resolve<IUPrimeRedisCacheDatabaseProvider>().GetDatabase();
        }
    }
}
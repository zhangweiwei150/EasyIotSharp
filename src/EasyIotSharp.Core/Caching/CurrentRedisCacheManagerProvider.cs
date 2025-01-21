using UPrime;
using UPrime.Dependency;
using UPrime.Runtime.Caching;
using UPrime.Runtime.Caching.Memory;
using UPrime.Runtime.Caching.Redis;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Core.Caching
{
    /// <summary>
    /// 根据配置文件返回CacheManager的提供
    /// Redis或内存
    /// </summary>
    public class CurrentRedisCacheManagerProvider : ICacheManagerProvider, ISingletonDependency
    {
        private readonly CachingOptions _cachingOptions;

        public CurrentRedisCacheManagerProvider()
        {
            _cachingOptions = UPrimeEngine.Instance.Resolve<CachingOptions>();
        }

        public ICacheManager GetCacheManager()
        {
            if (_cachingOptions.IsOpen)
            {
                if (_cachingOptions.RedisEnabled)
                {
                    return UPrimeEngine.Instance.Resolve<UPrimeRedisCacheManager>();
                }
                else
                {
                    return UPrimeEngine.Instance.Resolve<UPrimeMemoryCacheManager>();
                }
            }
            else
            {
                return UPrimeEngine.Instance.Resolve<NullCacheManager>();
            }
        }
    }
}
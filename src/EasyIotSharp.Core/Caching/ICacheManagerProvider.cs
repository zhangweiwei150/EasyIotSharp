using UPrime.Runtime.Caching;

namespace EasyIotSharp.Core.Caching
{
    public interface ICacheManagerProvider
    {
        ICacheManager GetCacheManager();
    }
}
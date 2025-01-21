using UPrime.Dependency;
using UPrime.Runtime.Caching;

namespace EasyIotSharp.Core.Caching
{
    /// <summary>
    /// 缓存服务的标记
    /// </summary>
    public interface ICacheService : ITransientDependency
    {
        ICache Cache { get; }

        void Clear();
    }
}
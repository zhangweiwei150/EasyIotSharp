using UPrime.Dependency;
using UPrime.Events.Bus.Handlers;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Events;

namespace EasyIotSharp.Core.Caching.Consumers
{
    public class Caching_Consumer :
         IEventHandler<Cache_ClearAll_Event>,
        ITransientDependency
    {
        private readonly CachingOptions _cachingOptions;

        public Caching_Consumer(CachingOptions cachingOptions)
        {
            _cachingOptions = cachingOptions;
        }

        public void HandleEvent(Cache_ClearAll_Event eventData)
        {

        }
    }
}
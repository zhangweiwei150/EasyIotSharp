using System.Threading.Tasks;
using UPrime;
using EasyIotSharp.Core.Events;

namespace EasyIotSharp.Core.Services.Cache.Admin.Impl
{
    public class CacheAdminService : ServiceBase, ICacheAdminService
    {
        public async Task<ActionOutput> ClearAll()
        {
            await EventBus.TriggerAsync(new Cache_ClearAll_Event { });
            return new ActionOutput();
        }
    }
}
using System.Threading.Tasks;
using UPrime;

namespace EasyIotSharp.Core.Services.Cache.Admin
{
    public interface ICacheAdminService
    {
        /// <summary>
        /// 清除项目所有缓存
        /// </summary>
        /// <returns></returns>
        Task<ActionOutput> ClearAll();
    }
}
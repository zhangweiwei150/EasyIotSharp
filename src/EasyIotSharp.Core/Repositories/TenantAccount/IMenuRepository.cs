using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Core.Domain.TenantAccount;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface IMenuRepository : IMySqlRepositoryBase<Menu, string>
    {
        /// <summary>
        /// 通过条件分页查询菜单列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="isEnable">-1=不参与查询  1=启用  2=禁用</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<(int totalCount, List<Menu> items)> Query(string keyword, int isEnable, int pageIndex, int pageSize);

        /// <summary>
        /// 通过id集合获取菜单列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<Menu>> QueryByIds(List<string> ids);
    }
}

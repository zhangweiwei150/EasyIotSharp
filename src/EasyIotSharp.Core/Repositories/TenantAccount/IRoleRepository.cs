using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface IRoleRepository : IMySqlRepositoryBase<Role, string>
    {
        /// <summary>
        /// 通过条件分页查询角色列表
        /// </summary>
        /// <param name="tenantNumId">租户numId</param>
        /// <param name="keyword">名字</param>
        /// <param name="isEnable">是否启用 -1=不参与查询  1=启用 2=禁用</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <returns></returns>
        Task<(int totalCount, List<Role> items)> Query(int tenantNumId,string keyword,int isEnable,int pageIndex,int pageSize,bool isPage = true);

        /// <summary>
        /// 通过id集合获取角色列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<Role>> QueryByIds(List<string> ids);
    }
}

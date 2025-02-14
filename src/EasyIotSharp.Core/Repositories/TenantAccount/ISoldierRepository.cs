using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface ISoldierRepository : IMySqlRepositoryBase<Soldier, string>
    {
        /// <summary>
        /// 通过条件分页查询用户列表
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="keyword">姓名/手机号</param>
        /// <param name="isEnable">是否启用 -1=不参与查询 1=启用 2=禁用</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条</param>
        /// <returns></returns>
        Task<(int totalCount, List<Soldier> items)> Query(int tenantId, string keyword, int isEnable, int pageIndex, int pageSize);
    }
}

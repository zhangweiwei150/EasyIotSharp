using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface ISoldierRoleRepository : IMySqlRepositoryBase<SoldierRole, string>
    {
        /// <summary>
        /// 通过用户id查询用户角色列表
        /// </summary>
        /// <param name="soldierId"></param>
        /// <returns></returns>
        Task<List<SoldierRole>> QueryBySoldierId(string soldierId);

        /// <summary>
        /// 通过用户id批量删除用户角色信息
        /// </summary>
        /// <param name="soldierId"></param>
        /// <returns></returns>
        Task<int> DeleteManyBySoldierId(string soldierId);
    }
}

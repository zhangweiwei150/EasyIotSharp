using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface IRoleMenuRepository : IMySqlRepositoryBase<RoleMenu, string>
    {
        /// <summary>
        /// 通过角色id集合获取角色菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<RoleMenu>> QueryByRoleIds(List<string> roleIds);

        /// <summary>
        /// 通过角色id批量删除角色菜单信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<int> DeleteManyByRoleId(string roleId);
    }
}

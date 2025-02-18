using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using LinqKit;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class RoleMenuRepository : MySqlRepositoryBase<RoleMenu, string>, IRoleMenuRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider">数据库提供者</param>
        public RoleMenuRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        /// <summary>
        /// 根据角色ID集合查询角色菜单关系
        /// </summary>
        /// <param name="roleIds">角色ID集合</param>
        /// <returns>返回角色菜单关系列表</returns>
        public async Task<List<RoleMenu>> QueryByRoleIds(List<string> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return new List<RoleMenu>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<RoleMenu>(false); // 初始化为空条件
            foreach (var roleId in roleIds)
            {
                var tempRoleId = roleId; // 避免闭包问题
                predicate = predicate.Or(m => m.RoleId == tempRoleId);
            }

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }

        /// <summary>
        /// 根据角色ID删除角色菜单关系
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> DeleteManyByRoleId(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException("角色ID不能为空", nameof(roleId));
            }

            // 删除符合条件的数据
            var count = await GetDbClient().Deleteable<RoleMenu>()
                                     .Where(m => m.RoleId == roleId)
                                     .ExecuteCommandAsync();
            return count;
        }
    }
}
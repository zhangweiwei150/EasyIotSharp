using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using SqlSugar;
using LinqKit;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class SoldierRoleRepository : MySqlRepositoryBase<SoldierRole, string>, ISoldierRoleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SoldierRoleRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        /// <summary>
        /// 根据用户ID查询用户角色关系
        /// </summary>
        /// <param name="soldierId">用户ID</param>
        /// <returns>返回用户角色关系列表</returns>
        public async Task<List<SoldierRole>> QueryBySoldierId(string soldierId)
        {
            if (string.IsNullOrWhiteSpace(soldierId))
            {
                return new List<SoldierRole>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<SoldierRole>(sr => sr.SoldierId == soldierId);

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }

        public async Task<List<SoldierRole>> QueryBySoldierIds(List<string> soldierIds)
        {
            if (soldierIds.Count<=0)
            {
                return new List<SoldierRole>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<SoldierRole>(sr => soldierIds.Contains(sr.SoldierId));

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }

        /// <summary>
        /// 根据用户ID删除用户角色关系
        /// </summary>
        /// <param name="soldierId">用户ID</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> DeleteManyBySoldierId(string soldierId)
        {
            if (string.IsNullOrWhiteSpace(soldierId))
            {
                throw new ArgumentException("用户ID不能为空", nameof(soldierId));
            }

            // 删除符合条件的数据
            var count = await Client.Deleteable<SoldierRole>()
                                     .Where(sr => sr.SoldierId == soldierId)
                                     .ExecuteCommandAsync();
            return count;
        }
    }
}
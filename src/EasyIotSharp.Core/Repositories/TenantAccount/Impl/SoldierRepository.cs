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
    public class SoldierRepository : MySqlRepositoryBase<Soldier, string>, ISoldierRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider">数据库提供者</param>
        public SoldierRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        /// <summary>
        /// 分页查询用户列表
        /// </summary>
        /// <param name="tenantNumId">租户编号（大于0时参与查询）</param>
        /// <param name="keyword">关键字（模糊匹配用户名或手机号）</param>
        /// <param name="isEnable">是否启用（-1=不参与查询，0=禁用，1=启用）</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>返回总记录数和分页数据</returns>
        public async Task<(int totalCount, List<Soldier> items)> Query(int tenantNumId, string keyword, int isEnable, int pageIndex, int pageSize)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<Soldier>(s => s.IsDelete == false);

            // 租户编号过滤
            if (tenantNumId > 0)
            {
                predicate = predicate.And(s => s.TenantNumId == tenantNumId);
            }

            // 关键字模糊查询（支持用户名或手机号）
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(s => s.Username.Contains(keyword) || s.Mobile.Contains(keyword));
            }

            // 是否启用过滤
            if (isEnable > -1)
            {
                predicate = predicate.And(s => s.IsEnable == (isEnable == 1));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<Soldier>());
            }

            // 手动拼接排序和分页逻辑
            var query = GetDbClient().Queryable<Soldier>()
                               .Where(predicate)
                               .OrderByDescending(s => s.CreationTime) // 默认按 CreationTime 降序排序
                               .Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize);

            // 查询数据
            var items = await query.ToListAsync();
            return (totalCount, items);
        }
    }
}
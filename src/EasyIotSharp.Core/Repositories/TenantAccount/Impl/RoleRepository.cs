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
    public class RoleRepository : MySqlRepositoryBase<Role, string>, IRoleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider">数据库提供者</param>
        public RoleRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        /// <summary>
        /// 分页查询角色列表
        /// </summary>
        /// <param name="tenantNumId">租户编号（大于0时参与查询）</param>
        /// <param name="keyword">关键字（模糊匹配角色名称）</param>
        /// <param name="isEnable">是否启用（-1=不参与查询，0=禁用，1=启用）</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>返回总记录数和分页数据</returns>
        public async Task<(int totalCount, List<Role> items)> Query(int tenantNumId, string keyword, int isEnable, int pageIndex, int pageSize, bool isPage = true)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<Role>(r => r.IsDelete == false);

            // 租户编号过滤
            if (tenantNumId > 0)
            {
                predicate = predicate.And(r => r.TenantNumId == tenantNumId);
            }

            // 关键字模糊查询
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(r => r.Name.Contains(keyword));
            }

            // 是否启用过滤
            if (isEnable > -1)
            {
                predicate = predicate.And(r => r.IsEnable == (isEnable == 1));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<Role>());
            }
            if (isPage)
            {
                // 手动拼接排序和分页逻辑
                var query = GetDbClient().Queryable<Role>().Where(predicate)
                                  .OrderByDescending(m => m.CreationTime) // 默认按 CreationTime 降序排序
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 分页查询，默认按 CreationTime 降序排序
                var items = await GetPagedListAsync(predicate, pageIndex, pageSize);
                return (totalCount, items);
            }
            else
            {
                // 手动拼接排序和分页逻辑
                var query = await GetDbClient().Queryable<Role>().Where(predicate)
                                  .OrderByDescending(m => m.CreationTime).ToListAsync(); // 默认按 CreationTime 降序排序
                // 分页查询，默认按 CreationTime 降序排序
                var items = query;
                return (totalCount, items);
            }
        }

        /// <summary>
        /// 根据ID集合查询菜单
        /// </summary>
        /// <param name="ids">菜单ID集合</param>
        /// <returns>返回菜单列表</returns>
        public async Task<List<Role>> QueryByIds(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<Role>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<Role>(false); // 初始化为空条件
            foreach (var id in ids)
            {
                var tempId = id; // 避免闭包问题
                predicate = predicate.Or(m => m.Id == tempId);
            }
            // 添加是否删除 = false 和 是否启用 = true 条件
            predicate = predicate.And(m => m.IsDelete == false); // 是否删除 = false
            predicate = predicate.And(m => m.IsEnable == true);  // 是否启用 = true

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }
    }
}
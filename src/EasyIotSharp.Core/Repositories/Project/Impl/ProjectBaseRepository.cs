using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;

namespace EasyIotSharp.Core.Repositories.Project.Impl
{
    public class ProjectBaseRepository : MySqlRepositoryBase<ProjectBase, string>, IProjectBaseRepository
    {
        public ProjectBaseRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<ProjectBase> items)> Query(int tenantNumId,
                                                      string keyword,
                                                      int state,
                                                      DateTime? createStartTime,
                                                      DateTime? createEndTime,
                                                      int pageIndex,
                                                      int pageSize)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<ProjectBase>(t => t.IsDelete == false);
            if (tenantNumId>0)
            {
                predicate = predicate.And(t => t.TenantNumId.Equals(tenantNumId));
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(t => t.Name.Contains(keyword) || t.Remark.Contains(keyword));
            }

            if (state > -1)
            {
                predicate = predicate.And(t => t.State.Equals(state));
            }

            if (createStartTime.HasValue && createEndTime.HasValue)
            {
                predicate = predicate.And(t => t.CreationTime >= createStartTime.Value && t.CreationTime <= createEndTime.Value);
            }
            else
            {
                if (createStartTime.HasValue)
                {
                    predicate = predicate.And(t => t.CreationTime >= createStartTime.Value);
                }
                if (createEndTime.HasValue)
                {
                    predicate = predicate.And(t => t.CreationTime <= createEndTime.Value);
                }
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<ProjectBase>());
            }

            // 分页查询
            var items = await GetPagedListAsync(predicate, pageIndex, pageSize);

            return (totalCount, items);
        }

        public async Task<List<ProjectBase>> QueryByIds(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<ProjectBase>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<ProjectBase>(false); // 初始化为空条件
            foreach (var id in ids)
            {
                var tempId = id; // 避免闭包问题
                predicate = predicate.Or(m => m.Id == tempId);
            }
            predicate = predicate.And(m => m.IsDelete == false); // 是否删除 = false

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }
    }
}

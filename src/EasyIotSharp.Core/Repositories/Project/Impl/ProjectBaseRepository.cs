using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
    }
}

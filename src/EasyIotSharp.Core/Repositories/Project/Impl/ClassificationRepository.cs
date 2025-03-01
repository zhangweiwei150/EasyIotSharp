﻿using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project.Impl
{
    public class ClassificationRepository : MySqlRepositoryBase<Classification, string>, IClassificationRepository
    {
        public ClassificationRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<Classification> items)> Query(int tenantNumId,
                                                                              string projectId,
                                                                              int pageIndex,
                                                                              int pageSize,
                                                                              bool isPage)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<Classification>(t => t.IsDelete == false);
            if (tenantNumId > 0)
            {
                predicate = predicate.And(t => t.TenantNumId.Equals(tenantNumId));
            }

            if (!string.IsNullOrWhiteSpace(projectId))
            {
                predicate = predicate.And(t => t.ProjectId.Equals(projectId));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<Classification>());
            }

            if (isPage==true)
            {
                var query = GetDbClient().Queryable<Classification>().Where(predicate)
                                  .OrderBy(x => x.Sort, OrderByType.Desc)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                var query = GetDbClient().Queryable<Classification>().Where(predicate)
                  .OrderBy(x => x.Sort, OrderByType.Desc)
                  .OrderBy(m => m.CreationTime, OrderByType.Desc);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }
    }
}

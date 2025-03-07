using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project.Impl
{
    public class SensorPointTypeRepository : MySqlRepositoryBase<SensorPointType, string>, ISensorPointTypeRepository
    {
        public SensorPointTypeRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<SensorPointType> items)> Query(int tenantNumId,
                                                                               string keyword,
                                                                               int pageIndex,
                                                                               int pageSize,
                                                                               bool isPage)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<SensorPointType>(t => t.IsDelete == false);
            if (tenantNumId > 0)
            {
                predicate = predicate.And(t => t.TenantNumId.Equals(tenantNumId));
            }
            if (!string.IsNullOrWhiteSpace(keyword)) 
            {
                predicate = predicate.And(t => t.Name.Contains(keyword) || t.BriefName.Contains(keyword) || t.Supplier.Contains(keyword));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<SensorPointType>());
            }

            if (isPage == true)
            {
                var query = GetDbClient().Queryable<SensorPointType>().Where(predicate)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                var query = GetDbClient().Queryable<SensorPointType>().Where(predicate)
                  .OrderBy(m => m.CreationTime, OrderByType.Desc);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }
    }
}

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
    public class SensorPointTypeQuotaRepository : MySqlRepositoryBase<SensorPointTypeQuota, string>, ISensorPointTypeQuotaRepository
    {
        public SensorPointTypeQuotaRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
        public async Task<(int totalCount, List<SensorPointTypeQuota> items)> Query(int tenantNumId,
                                                                               string sensorPointTypeId,
                                                                               string keyword,
                                                                               DataTypeMenu dataType,
                                                                               int pageIndex,
                                                                               int pageSize,
                                                                               bool isPage)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<SensorPointTypeQuota>(t => t.IsDelete == false);
            if (tenantNumId > 0)
            {
                predicate = predicate.And(t => t.TenantNumId.Equals(tenantNumId));
            }
            if (!string.IsNullOrWhiteSpace(sensorPointTypeId))
            {
                predicate = predicate.And(t => t.SensorPointTypeId.Equals(sensorPointTypeId));
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(t => t.Name.Contains(keyword) || t.Identifier.Contains(keyword));
            }
            if (dataType!= DataTypeMenu.None)
            {
                predicate = predicate.And(t => t.DataType.Equals(dataType));
            }


            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<SensorPointTypeQuota>());
            }

            if (isPage == true)
            {
                var query = GetDbClient().Queryable<SensorPointTypeQuota>().Where(predicate)
                                  .OrderBy(t => t.Sort,OrderByType.Desc)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                var query = GetDbClient().Queryable<SensorPointTypeQuota>().Where(predicate)
                                  .OrderBy(t => t.Sort, OrderByType.Desc)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }
    }
}

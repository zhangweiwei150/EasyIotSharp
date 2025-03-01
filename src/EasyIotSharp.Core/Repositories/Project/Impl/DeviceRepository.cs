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
    public class DeviceRepository: MySqlRepositoryBase<Device, string>, IDeviceRepository
    {
        public DeviceRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<Device> items)> Query(int tenantNumId,
                                                                      string Keyword,
                                                                      int state,
                                                                      string protocolId,
                                                                      string projectId,
                                                                      int pageIndex,
                                                                      int pageSize)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<Device>(t => t.IsDelete == false);
            if (tenantNumId > 0)
            {
                predicate = predicate.And(t => t.TenantNumId.Equals(tenantNumId));
            }

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                predicate = predicate.And(t => t.Name.Contains(Keyword));
            }

            if (state > -1)
            {
                predicate = predicate.And(t => t.State.Equals(state));
            }

            if (!string.IsNullOrWhiteSpace(protocolId))
            {
                predicate = predicate.And(t => t.ProtocolId.Equals(protocolId));
            }

            if (!string.IsNullOrWhiteSpace(projectId))
            {
                predicate = predicate.And(t => t.ProjectId.Equals(projectId));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<Device>());
            }

            var query = GetDbClient().Queryable<Device>().Where(predicate)
                              .OrderBy(m => m.CreationTime, OrderByType.Desc)
                              .Skip((pageIndex - 1) * pageSize)
                              .Take(pageSize);
            // 查询数据
            var items = await query.ToListAsync();
            return (totalCount, items);
        }
    }
}

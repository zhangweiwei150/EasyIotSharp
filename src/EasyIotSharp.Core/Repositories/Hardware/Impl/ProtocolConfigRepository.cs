using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Domain.Hardware;
using System.Threading.Tasks;
using LinqKit;
using SqlSugar;

namespace EasyIotSharp.Core.Repositories.Hardware.Impl
{
    public class ProtocolConfigRepository:MySqlRepositoryBase<ProtocolConfig, string>, IProtocolConfigRepository
    {
        public ProtocolConfigRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<ProtocolConfig> items)> Query(string protocolId, 
                                                                              string keyword, 
                                                                              TagTypeMenu tagType, 
                                                                              int pageIndex, 
                                                                              int pageSize,
                                                                              bool isPage)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<ProtocolConfig>(t => t.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(protocolId))
            {
                predicate = predicate.And(t => t.ProtocolId.Equals(protocolId));
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(t => t.Label.Contains(keyword) || t.Placeholder.Contains(keyword) || t.Tag.Contains(keyword));
            }
            if (tagType!= TagTypeMenu.None)
            {
                predicate = predicate.And(t => t.TagType.Equals(tagType));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<ProtocolConfig>());
            }

            if (isPage == true)
            {
                var query = Client.Queryable<ProtocolConfig>().Where(predicate)
                                  .OrderBy(m => m.Sort, OrderByType.Desc)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                var query = Client.Queryable<ProtocolConfig>().Where(predicate)
                                  .OrderBy(m => m.Sort, OrderByType.Desc)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }
    }
}

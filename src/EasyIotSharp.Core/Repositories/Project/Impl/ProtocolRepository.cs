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
    public class ProtocolRepository : MySqlRepositoryBase<Protocol, string>, IProtocolRepository
    {
        public ProtocolRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<Protocol> items)> Query(int isEnable,
                                                                        string keyword,
                                                                        int pageIndex,
                                                                        int pageSize,
                                                                        bool isPage)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<Protocol>(t => t.IsDelete == false);

            if (isEnable>-1)
            {
                predicate = predicate.And(t => t.IsEnable.Equals(isEnable == 1));
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(t => t.Name.Contains(keyword) || t.Remark.Contains(keyword));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<Protocol>());
            }

            if (isPage == true)
            {
                var query = GetDbClient().Queryable<Protocol>().Where(predicate)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                var query = GetDbClient().Queryable<Protocol>().Where(predicate)
                  .OrderBy(m => m.CreationTime, OrderByType.Desc);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }
    }
}

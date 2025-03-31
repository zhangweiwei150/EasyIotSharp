using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Hardware.Impl
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
                var query = Client.Queryable<Protocol>().Where(predicate)
                                  .OrderBy(m => m.CreationTime, OrderByType.Desc)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                var query = Client.Queryable<Protocol>().Where(predicate)
                  .OrderBy(m => m.CreationTime, OrderByType.Desc);
                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }

        public async Task<List<Protocol>> QueryByIds(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<Protocol>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<Protocol>(false); // 初始化为空条件
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

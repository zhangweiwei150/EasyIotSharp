using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Hardware.Impl
{
    public  class ProtocolConfigExtRepository : MySqlRepositoryBase<ProtocolConfigExt, string>, IProtocolConfigExtRepository
    {
        public ProtocolConfigExtRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<List<ProtocolConfigExt>> QueryByConfigIds(List<string> configIds)
        {
            if (configIds == null || configIds.Count == 0)
            {
                return new List<ProtocolConfigExt>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<ProtocolConfigExt>(false); // 初始化为空条件
            foreach (var configId in configIds)
            {
                var tempConfigId = configId; // 避免闭包问题
                predicate = predicate.Or(m => m.ProtocolConfigId == tempConfigId);
            }
            predicate = predicate.And(m => m.IsDelete == false); // 是否删除 = false

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }

        public async Task<int> DeleteManyByConfigId(string configId)
        {
            if (string.IsNullOrWhiteSpace(configId))
            {
                throw new ArgumentException("协议配置ID不能为空", nameof(configId));
            }

            // 删除符合条件的数据
            var count = await GetDbClient().Deleteable<ProtocolConfigExt>()
                                     .Where(m => m.ProtocolConfigId == configId)
                                     .ExecuteCommandAsync();
            return count;
        }
    }
}

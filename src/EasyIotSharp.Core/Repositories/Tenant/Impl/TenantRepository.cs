using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyIotSharp.Core.Extensions;
using EasyIotSharp.Core.Repositories.Mysql;
using LinqKit;
using SqlSugar;

namespace EasyIotSharp.Core.Repositories.Tenant.Impl
{
    public class TenantRepository : MySqlRepositoryBase<EasyIotSharp.Core.Domain.Tenant.Tenant, string>, ITenantRepository
    {
        public TenantRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<EasyIotSharp.Core.Domain.Tenant.Tenant> items)> Query(
            string keyword,
            int expiredType,
            DateTime? contractStartTime,
            DateTime? contractEndTime,
            int isFreeze,
            int pageIndex,
            int pageSize)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<EasyIotSharp.Core.Domain.Tenant.Tenant>(t => t.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(t => t.Name.Contains(keyword));
            }

            switch (expiredType)
            {
                case 0: // 待授权
                    predicate = predicate.And(t => t.ContractStartTime > DateTime.Now);
                    break;
                case 1: // 生效中
                    predicate = predicate.And(t => t.ContractStartTime <= DateTime.Now && t.ContractEndTime > DateTime.Now);
                    break;
                case 2: // 已过期
                    predicate = predicate.And(t => t.ContractEndTime <= DateTime.Now);
                    break;
            }

            if (contractStartTime.HasValue && contractEndTime.HasValue)
            {
                predicate = predicate.And(t => t.ContractStartTime >= contractStartTime.Value && t.ContractEndTime <= contractEndTime.Value);
            }
            else
            {
                if (contractStartTime.HasValue)
                {
                    predicate = predicate.And(t => t.ContractStartTime >= contractStartTime.Value);
                }
                if (contractEndTime.HasValue)
                {
                    predicate = predicate.And(t => t.ContractEndTime <= contractEndTime.Value);
                }
            }

            if (isFreeze > -1)
            {
                predicate = predicate.And(t => t.IsFreeze == (isFreeze == 1));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<EasyIotSharp.Core.Domain.Tenant.Tenant>());
            }

            // 分页查询
            var items = await GetPagedListAsync(predicate, pageIndex, pageSize);

            return (totalCount, items);
        }
    }
}
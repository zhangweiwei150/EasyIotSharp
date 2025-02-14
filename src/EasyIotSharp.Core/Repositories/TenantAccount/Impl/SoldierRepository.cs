using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class SoldierRepository:MySqlRepositoryBase<Soldier, string>, ISoldierRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public SoldierRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

        public async Task<(int totalCount, List<Soldier> items)> Query(int tenantId, string keyword, int isEnable, int pageIndex, int pageSize)
        {
            var sql = "SELECT * FROM Soldiers where 1=1 and IsDelete=false ";
            string whereStr = default;
            if (tenantId>0)
            {
                whereStr += $" and TenantId={tenantId}";
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                whereStr += $" and (Username like '%{keyword}%' or Mobile like '%{keyword}%')";
            }
            if (isEnable > -1)
            {
                whereStr += $" and IzEnable={(isEnable == 1 ? true : false)}";
            }
            string totalCountSql = "SELECT count(1) FROM Soldiers where 1=1 and IsDelete=false " + whereStr;
            var totalCount = await Client.Ado.GetIntAsync(totalCountSql);
            if (totalCount <= 0)
            {
                return (0, new List<Soldier>());
            }
            var items = await Client.Ado.SqlQueryAsync<Soldier>(sql + whereStr);
            return (totalCount, items);
        }
    }
}

using EasyIotSharp.Repositories.Mysql;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class RoleRepository : MySqlRepositoryBase<Role, string>, IRoleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public RoleRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

        public async Task<(int totalCount, List<Role> items)> Query(int tenantNumId,string keyword, int isEnable, int pageIndex, int pageSize)
        {
            var sql = "SELECT * FROM Roles where 1=1 and IsDelete=false ";
            string pageStr = $"LIMIT {pageSize} OFFSET ({pageIndex} - 1) * {pageSize}";
            string whereStr = default;
            if (tenantNumId>0)
            {
                whereStr += $" and TenantNumId={tenantNumId}";
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                whereStr += $" and Name like '%{keyword}%'";
            }
            if (isEnable > -1)
            {
                whereStr += $" and IsEnable={(isEnable == 1 ? true : false)}";
            }
            string totalCountSql = "SELECT count(1) FROM Roles where 1=1 and IsDelete=false " + whereStr + pageStr;
            var totalCount = await Client.Ado.GetIntAsync(totalCountSql);
            if (totalCount <= 0)
            {
                return (0, new List<Role>());
            }
            string sortStr = " ORDER BY CreationTime DESC";
            var items = await Client.Ado.SqlQueryAsync<Role>(sql + whereStr + sortStr + pageStr);
            return (totalCount, items);
        }
    }
}

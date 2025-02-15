using EasyIotSharp.Core.Repositories.Tenant;
using EasyIotSharp.Repositories.Mysql;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class MenuRepository : MySqlRepositoryBase<Menu, string>, IMenuRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public MenuRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

        public async Task<(int totalCount, List<Menu> items)> Query(string keyword, int isEnable, int pageIndex, int pageSize)
        {
            var sql = "SELECT * FROM Menus where 1=1 and IsDelete=false ";
            string pageStr = $"LIMIT {pageSize} OFFSET ({pageIndex} - 1) * {pageSize}";
            string whereStr = default;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                whereStr += $" and Name like '%{keyword}%'";
            }
            if (isEnable > -1)
            {
                whereStr += $" and IsEnable={(isEnable == 1 ? true : false)}";
            }
            string totalCountSql = "SELECT count(1) FROM Menus where 1=1 and IsDelete=false " + whereStr + pageStr;
            var totalCount = await Client.Ado.GetIntAsync(totalCountSql);
            if (totalCount <= 0)
            {
                return (0, new List<Menu>());
            }
            string sortStr = " ORDER BY CreationTime DESC";
            var items = await Client.Ado.SqlQueryAsync<Menu>(sql + whereStr + sortStr + pageStr);
            return (totalCount, items);
        }

        public async Task<List<Menu>> QueryByIds(List<string> ids)
        {
            if (ids.Count<=0)
            {
                return new List<Menu>();
            }
            string sql = $"select * from Menus where Id in({ids.JoinAsString(",")})";
            var items = await Client.Ado.SqlQueryAsync<Menu>(sql);
            return items;
        }
    }
}

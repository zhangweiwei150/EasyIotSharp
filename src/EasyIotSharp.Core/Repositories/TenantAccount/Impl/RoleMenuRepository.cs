using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class RoleMenuRepository : MySqlRepositoryBase<RoleMenu, string>, IRoleMenuRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public RoleMenuRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

        public async Task<List<RoleMenu>> QueryByRoleIds(List<string> roleIds)
        {
            if (roleIds.Count<=0)
            {
                return new List<RoleMenu>();
            }
            string sql = $"select * from RoleMenus where RoleId in({roleIds.JoinAsString(",")})";
            var items = await Client.Ado.SqlQueryAsync<RoleMenu>(sql);
            return items;
        }

        public async Task<int> DeleteManyByRoleId(string roleId)
        {
            var count = await Client.Deleteable<RoleMenu>().Where(x => x.RoleId == roleId).ExecuteCommandAsync();
            return count;
        }
    }
}

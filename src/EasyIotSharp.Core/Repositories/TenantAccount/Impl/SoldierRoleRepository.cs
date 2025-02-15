using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class SoldierRoleRepository : MySqlRepositoryBase<SoldierRole, string>, ISoldierRoleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public SoldierRoleRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

        public async Task<List<SoldierRole>> QueryBySoldierId(string soldierId)
        {
            if (string.IsNullOrWhiteSpace(soldierId))
            {
                return new List<SoldierRole>();
            }
            string sql = $"select * from SoldierRoles where SoldierId={soldierId}";
            var items = await Client.Ado.SqlQueryAsync<SoldierRole>(sql);
            return items;
        }

        public async Task<int> DeleteManyBySoldierId(string soldierId)
        {
            var count = await Client.Deleteable<SoldierRole>().Where(x => x.SoldierId == soldierId).ExecuteCommandAsync();
            return count;
        }
    }
}

using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
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

        public async Task<int> DeleteManyBySoldierId(string soldierId)
        {
            var count = await Client.Deleteable<SoldierRole>().Where(x => x.SoldierId == soldierId).ExecuteCommandAsync();
            return count;
        }
    }
}

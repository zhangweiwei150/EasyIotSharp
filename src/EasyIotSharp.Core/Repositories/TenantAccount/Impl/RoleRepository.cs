using EasyIotSharp.Repositories.Mysql;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class RoleRepository : MySqlRepositoryBase<Role, int>, IRoleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public RoleRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }
    }
}

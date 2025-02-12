using EasyIotSharp.Core.Repositories.Tenant;
using EasyIotSharp.Repositories.Mysql;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class MenuRepository : MySqlRepositoryBase<Menu, int>, IMenuRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public MenuRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }
    }
}

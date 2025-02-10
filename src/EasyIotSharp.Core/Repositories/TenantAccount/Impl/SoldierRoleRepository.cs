using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class SoldierRoleRepository : MySqlRepositoryBase<SoldierRole, int>, ISoldierRoleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public SoldierRoleRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }
    }
}

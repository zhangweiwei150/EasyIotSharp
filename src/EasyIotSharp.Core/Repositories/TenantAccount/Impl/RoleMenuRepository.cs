using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class RoleMenuRepository : MySqlRepositoryBase<RoleMenu, int>, IRoleMenuRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public RoleMenuRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }
    }
}

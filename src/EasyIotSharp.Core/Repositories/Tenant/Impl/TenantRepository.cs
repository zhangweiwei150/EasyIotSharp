using EasyIotSharp.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Repositories.Mysql;

namespace EasyIotSharp.Core.Repositories.Tenant.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TenantRepository : MySqlRepositoryBase<EasyIotSharp.Core.Domain.Tenant.Tenant,int>, ITenantRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public TenantRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

    }
}

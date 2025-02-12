using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class SoldierRepository:MySqlRepositoryBase<Soldier, int>, ISoldierRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public SoldierRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }
    }
}

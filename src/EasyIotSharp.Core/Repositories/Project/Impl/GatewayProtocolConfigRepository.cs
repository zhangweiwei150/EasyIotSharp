using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.Project.Impl
{
    public class GatewayProtocolConfigRepository : MySqlRepositoryBase<GatewayProtocolConfig, string>, IGatewayProtocolConfigRepository
    {
        public GatewayProtocolConfigRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}

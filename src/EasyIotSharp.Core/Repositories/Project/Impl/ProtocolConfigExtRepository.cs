using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.Project.Impl
{
    public  class ProtocolConfigExtRepository : MySqlRepositoryBase<ProtocolConfigExt, string>, IProtocolConfigExtRepository
    {
        public ProtocolConfigExtRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}

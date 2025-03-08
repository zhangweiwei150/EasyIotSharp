using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.Hardware.Impl
{
    public  class ProtocolConfigExtRepository : MySqlRepositoryBase<ProtocolConfigExt, string>, IProtocolConfigExtRepository
    {
        public ProtocolConfigExtRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}

using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Domain.Hardware;

namespace EasyIotSharp.Core.Repositories.Hardware.Impl
{
    public class ProtocolConfigRepository:MySqlRepositoryBase<ProtocolConfig, string>, IProtocolConfigRepository
    {
        public ProtocolConfigRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}

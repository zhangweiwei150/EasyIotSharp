using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Domain.Proejct;

namespace EasyIotSharp.Core.Repositories.Project.Impl
{
    public class ProtocolConfigRepository:MySqlRepositoryBase<ProtocolConfig, string>, IProtocolConfigRepository
    {
        public ProtocolConfigRepository(ISqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}

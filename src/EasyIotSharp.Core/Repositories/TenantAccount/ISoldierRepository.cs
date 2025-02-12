using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface ISoldierRepository : IMySqlRepositoryBase<Soldier, int>
    {
    }
}

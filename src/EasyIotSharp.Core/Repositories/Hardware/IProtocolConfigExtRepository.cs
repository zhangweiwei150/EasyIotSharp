using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Repositories.Hardware
{
    public interface IProtocolConfigExtRepository : IMySqlRepositoryBase<ProtocolConfigExt, string>
    {
    }
}

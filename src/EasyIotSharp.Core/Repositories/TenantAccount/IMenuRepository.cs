using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Core.Domain.TenantAccount;

namespace EasyIotSharp.Core.Repositories.TenantAccount
{
    public interface IMenuRepository : IMySqlRepositoryBase<Menu, int>
    {
    }
}

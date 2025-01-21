using EasyIotSharp.Core.Repositories.Mysql;

namespace EasyIotSharp.Core.Repositories.Tenant
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITenantRepository: IMySqlRepositoryBase<EasyIotSharp.Core.Domain.Tenant.Tenant, int>
    {
        
    }
}

using EasyIotSharp.Core.Repositories.Tenant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.Tenant.Params;

namespace EasyIotSharp.Core.Services.Tenant.Impl
{
    public class TenantService:ServiceBase, ITenantService
    {
        /// <summary>
        /// 
        /// </summary>
        public ITenantRepository _tenantRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantRepository"></param>
        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task InsertTenant(InsertTenantInput input) 
        {
            var model = new EasyIotSharp.Core.Domain.Tenant.Tenant();
            model.Name = input.Name;
            model.IsDelete = false;
            await _tenantRepository.InsertAsync(model);
        }
    }
}

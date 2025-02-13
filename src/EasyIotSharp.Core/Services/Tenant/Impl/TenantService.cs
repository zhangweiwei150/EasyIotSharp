using EasyIotSharp.Core.Repositories.Tenant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.Tenant.Params;
using EasyIotSharp.Core.Dto.Tenant;
using UPrime.Services.Dto;
using UPrime.AutoMapper;

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


        public async Task<TenantDto> GetTenant(int id)
        {
            var info=await _tenantRepository.GetByIdAsync(id);
            return info.MapTo<TenantDto>();
        }


        public async Task<PagedResultDto<TenantDto>> QueryTenant(QueryTenantInput input)
        {
            return new PagedResultDto<TenantDto>();
        }

        public async Task InsertTenant(InsertTenantInput input)
        {
            var model = new EasyIotSharp.Core.Domain.Tenant.Tenant();
            model.Name = input.Name;
            model.IsDelete = false;
            await _tenantRepository.InsertAsync(model);
        }

        public async Task UpdateTenant(UpdateTenantInput input)
        {

        }

        public async Task UpdateIsFreezeTenant(UpdateIsFreezeTenantTenantInput input)
        {

        }

        public async Task DeleteTenant(DeleteTenantInput input)
        {

        }
    }
}

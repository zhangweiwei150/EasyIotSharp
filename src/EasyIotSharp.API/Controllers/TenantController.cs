using EasyIotSharp.API.Filters;
using EasyIotSharp.Core.Dto.Tenant;
using EasyIotSharp.Core.Dto.Tenant.Params;
using EasyIotSharp.Core.Services.Tenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrime.Services.Dto;
using UPrime.WebApi;

namespace EasyIotSharp.API.Controllers
{
    
    public class TenantController : ApiControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController()
        {
            _tenantService = UPrime.UPrimeEngine.Instance.Resolve<ITenantService>();
        }

        /// <summary>
        ///通过租户id获取一条租户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Tenant/Get")]
        [Authorize]
        public async Task<UPrimeResponse<TenantDto>> GetTenant(int id)
        {
            UPrimeResponse<TenantDto> res = new UPrimeResponse<TenantDto>();
            res.Result= await _tenantService.GetTenant(id);
            return res;
        }

        /// <summary>
        ///通过条件分页查询租户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Tenant/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<TenantDto>>> QueryTenant([FromBody] QueryTenantInput input)
        {
            UPrimeResponse<PagedResultDto<TenantDto>> res = new UPrimeResponse<PagedResultDto<TenantDto>>();
            res.Result = await _tenantService.QueryTenant(input);
            return res;
        }

        /// <summary>
        /// 添加一条租户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Tenant/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertTenant([FromBody]InsertTenantInput input)
        {
            await _tenantService.InsertTenant(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一个租户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Tenant/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateTenant([FromBody] UpdateTenantInput input)
        {
            await _tenantService.UpdateTenant(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一个租户的冻结状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Tenant/UpdateFreeze")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateIsFreezeTenant([FromBody] UpdateIsFreezeTenantTenantInput input)
        {
            await _tenantService.UpdateIsFreezeTenant(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一个租户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Tenant/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteTenant(DeleteTenantInput input)
        {
            await _tenantService.DeleteTenant(input);
            return new UPrimeResponse();
        }
    }
}

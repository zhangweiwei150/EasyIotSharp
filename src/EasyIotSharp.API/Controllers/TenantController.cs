using EasyIotSharp.API.Filters;
using EasyIotSharp.Core.Dto.Tenant.Params;
using EasyIotSharp.Core.Services.Tenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// 
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
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Tenant/Get")]
        [Authorize]
        public async Task<UPrimeResponse> GetTenant([FromBody] InsertTenantInput input)
        {
            await _tenantService.InsertTenant(input);
            return new UPrimeResponse();
        }
    }
}

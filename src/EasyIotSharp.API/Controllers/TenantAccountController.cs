using EasyIotSharp.Core.Dto.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Services.TenantAccount;
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
    /// <summary>
    /// 租户账号控制器
    /// </summary>
    public class TenantAccountController : ApiControllerBase
    {
        private readonly ISoldierService _soldierService;

        public TenantAccountController()
        {
            _soldierService = UPrime.UPrimeEngine.Instance.Resolve<ISoldierService>();
        }

        /// <summary>
        /// 通过id获取一条用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Get")]
        public async Task<UPrimeResponse<SoldierDto>> GetSoldier(string id)
        {
            UPrimeResponse<SoldierDto> res = new UPrimeResponse<SoldierDto>();
            res.Result = await _soldierService.GetSoldier(id);
            return res;
        }

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Validate")]
        public async Task<UPrimeResponse<ValidateSoldierOutput>> ValidateSoldier([FromBody] ValidateSoldierInput input)
        {
            UPrimeResponse<ValidateSoldierOutput> res = new UPrimeResponse<ValidateSoldierOutput>();
            res.Result = await _soldierService.ValidateSoldier(input);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Query")]
        public async Task<UPrimeResponse<PagedResultDto<SoldierDto>>> QuerySoldier([FromBody]QuerySoldierInput input)
        {
            UPrimeResponse<PagedResultDto<SoldierDto>> res = new UPrimeResponse<PagedResultDto<SoldierDto>>();
            res.Result = await _soldierService.QuerySoldier(input);
            return res;
        }

        /// <summary>
        /// 添加一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Insert")]
        public async Task<UPrimeResponse> InsertSoldier([FromBody]InsertSoldierInput input)
        {
            await _soldierService.InsertSoldier(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Update")]
        public async Task<UPrimeResponse> UpdateSoldier([FromBody] UpdateSoldierInput input)
        {
            await _soldierService.UpdateSoldier(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条用户信息的启用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/UpdateIsEnable")]
        public async Task<UPrimeResponse> UpdateSoldierIsEnable([FromBody]UpdateSoldierIsEnableInput input)
        {
            await _soldierService.UpdateSoldierIsEnable(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Delete")]
        public async Task<UPrimeResponse> DeleteSoldier([FromBody]DeleteSoldierInput input)
        {
            await _soldierService.DeleteSoldier(input);
            return new UPrimeResponse();
        }
    }
}

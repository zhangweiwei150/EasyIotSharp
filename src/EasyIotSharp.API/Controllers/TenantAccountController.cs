using EasyIotSharp.API.Filters;
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
        private readonly IMenuService _menuService;
        private readonly IRoleService _roleService;
        private readonly ISoldierRoleService _soldierRoleService;

        public TenantAccountController()
        {
            _soldierService = UPrime.UPrimeEngine.Instance.Resolve<ISoldierService>();
            _menuService = UPrime.UPrimeEngine.Instance.Resolve<IMenuService>();
            _roleService = UPrime.UPrimeEngine.Instance.Resolve<IRoleService>();
            _soldierRoleService= UPrime.UPrimeEngine.Instance.Resolve<ISoldierRoleService>();
        }

        #region 用户

        /// <summary>
        /// 通过id获取一条用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Get")]
        [Authorize]
        public async Task<UPrimeResponse<SoldierDto>> GetSoldier()
        {
            UPrimeResponse<SoldierDto> res = new UPrimeResponse<SoldierDto>();
            res.Result = await _soldierService.GetSoldier(TokenUserId);
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
        public async Task<UPrimeResponse<PagedResultDto<SoldierDto>>> QuerySoldier([FromBody] QuerySoldierInput input)
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
        public async Task<UPrimeResponse> InsertSoldier([FromBody] InsertSoldierInput input)
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
        public async Task<UPrimeResponse> UpdateSoldierIsEnable([FromBody] UpdateSoldierIsEnableInput input)
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
        public async Task<UPrimeResponse> DeleteSoldier([FromBody] DeleteSoldierInput input)
        {
            await _soldierService.DeleteSoldier(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条用户的角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Soldier/Role/Update")]
        public async Task<UPrimeResponse> UpdateSoldierRole([FromBody] UpdateSoldierRoleInput input)
        {
            await _soldierRoleService.UpdateSoldierRole(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 菜单

        /// <summary>
        /// 通过id获取一条菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/Get")]
        public async Task<UPrimeResponse<MenuDto>> GetMenu(string id)
        {
            UPrimeResponse<MenuDto> res = new UPrimeResponse<MenuDto>();
            res.Result = await _menuService.GetMenu(id);
            return res;
        }

        /// <summary>
        /// 通通过条件分页查询菜单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/Query")]
        public async Task<UPrimeResponse<PagedResultDto<MenuDto>>> QueryMenu([FromBody]QueryMenuInput input)
        {
            UPrimeResponse<PagedResultDto<MenuDto>> res = new UPrimeResponse<PagedResultDto<MenuDto>>();
            res.Result = await _menuService.QueryMenu(input);
            return res;
        }

        /// <summary>
        /// 通过用户id获取用户对应的菜单列表
        /// </summary>
        /// <param name="soldierId"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/Soldier/Query")]
        public async Task<UPrimeResponse<List<QueryMenuBySoldierIdOutput>>> QueryMenuBySoldierId(string soldierId)
        {
            UPrimeResponse<List<QueryMenuBySoldierIdOutput>> res = new UPrimeResponse<List<QueryMenuBySoldierIdOutput>>();
            res.Result = await _menuService.QueryMenuBySoldierId(soldierId);
            return res;
        }

        /// <summary>
        /// 添加一条菜单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/Insert")]
        public async Task<UPrimeResponse> InsertMenu([FromBody]InsertMenuInput input)
        {
            await _menuService.InsertMenu(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条菜单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/Update")]
        public async Task<UPrimeResponse> UpdateMenu([FromBody]UpdateMenuInput input)
        {
            await _menuService.UpdateMenu(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过od修改一条菜单是否启用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/UpdateIsEnable")]
        public async Task<UPrimeResponse> UpdateIsEnableMenu([FromBody] UpdateIsEnableMenuInput input)
        {
            await _menuService.UpdateIsEnableMenu(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id获取一条菜单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Menu/Delete")]
        public async Task<UPrimeResponse> DeleteMenu([FromBody] DeleteMenuInput input)
        {
            await _menuService.DeleteMenu(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 角色

        /// <summary>
        /// 通过id获取一条角色菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Role/Menu/Get")]
        public async Task<UPrimeResponse<GetRoleMenuOutput>> GetRoleMenu(string id)
        {
            UPrimeResponse<GetRoleMenuOutput> res = new UPrimeResponse<GetRoleMenuOutput>();
            res.Result = await _roleService.GetRoleMenu(id);
            return res;
        }

        /// <summary>
        /// 通通过条件分页查询菜单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Role/Query")]
        public async Task<UPrimeResponse<PagedResultDto<RoleDto>>> QueryRole([FromBody] QueryRoleInput input)
        {
            UPrimeResponse<PagedResultDto<RoleDto>> res = new UPrimeResponse<PagedResultDto<RoleDto>>();
            res.Result = await _roleService.QueryRole(input);
            return res;
        }

        /// <summary>
        /// 添加一条角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Role/Insert")]
        public async Task<UPrimeResponse> InsertRole([FromBody] InsertRoleInput input)
        {
            await _roleService.InsertRole(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条菜单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Role/Update")]
        public async Task<UPrimeResponse> UpdateRole([FromBody] UpdateRoleInput input)
        {
            await _roleService.UpdateRole(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通通过id修改一条菜单是否启用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Role/UpdateIsEnable")]
        public async Task<UPrimeResponse> UpdateIsEnableRole([FromBody] UpdateIsEnableRoleInput input)
        {
            await _roleService.UpdateIsEnableRole(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id获取一条角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/TenantAccount/Role/Delete")]
        public async Task<UPrimeResponse> DeleteRole([FromBody] DeleteRoleInput input)
        {
            await _roleService.DeleteRole(input);
            return new UPrimeResponse();
        }

        #endregion
    }
}

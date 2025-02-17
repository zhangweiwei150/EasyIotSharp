using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.TenantAccount;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Dto.TenantAccount.Params;

namespace EasyIotSharp.Core.Services.TenantAccount
{
    public interface ISoldierService
    {
        /// <summary>
        /// 通过id获取一条用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SoldierDto> GetSoldier(string id);

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ValidateSoldierOutput> ValidateSoldier(ValidateSoldierInput input);

        /// <summary>
        /// 通过条件分页查询用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SoldierDto>> QuerySoldier(QuerySoldierInput input);

        /// <summary>
        /// 添加一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> InsertSoldier(InsertSoldierInput input);

        /// <summary>
        /// 创建租户，默认添加一条租户管理员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> InsertAdminSoldier(InsertAdminSoldierInput input);

        /// <summary>
        /// 通过id修改一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> UpdateSoldier(UpdateSoldierInput input);

        /// <summary>
        /// 通过id修改一条用户信息的启用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> UpdateSoldierIsEnable(UpdateSoldierIsEnableInput input);

        /// <summary>
        /// 通过id删除一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteSoldier(DeleteSoldierInput input);
    }
}

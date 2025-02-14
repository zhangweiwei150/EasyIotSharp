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
        Task<SoldierDto> GetSoldier(int id);

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
        Task InsertSoldier(InsertSoldierInput input);

        /// <summary>
        /// 通过id修改一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSoldier(UpdateSoldierInput input);

        /// <summary>
        /// 通过id修改一条用户信息的启用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSoldierIsEnable(UpdateSoldierIsEnableInput input);

        /// <summary>
        /// 通过id删除一条用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteSoldier(DeleteSoldierInput input);
    }
}

using EasyIotSharp.Core.Dto.TenantAccount.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Services.TenantAccount
{
    public interface ISoldierRoleService
    {
        /// <summary>
        /// 通过用户id修改对应的角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSoldierRole(UpdateSoldierRoleInput input);
    }
}

using System.Collections.Generic;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过用户id修改对应的角色的入参类
    /// </summary>
    public class UpdateSoldierRoleInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string SoldierId { get; set; }

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleDto> Roles { get; set; }
    }
}

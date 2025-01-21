using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.TenantAccount
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [SugarTable("RoleMenus")]
    public class RoleMenu : BaseEntity<int>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int SoldierId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单id
        /// </summary>
        public int MenuId { get; set; }
    }
}

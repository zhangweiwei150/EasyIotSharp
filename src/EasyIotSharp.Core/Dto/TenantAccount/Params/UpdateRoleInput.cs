using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过id修改一个角色信息的入参类
    /// </summary>
    public class UpdateRoleInput:OperateUserInput
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否修改菜单
        /// </summary>
        public bool IsUpdateMenu { get; set; }

        /// <summary>
        /// 菜单信息
        /// </summary>
        public List<MenuDto> Menus { get; set; }
    }
}

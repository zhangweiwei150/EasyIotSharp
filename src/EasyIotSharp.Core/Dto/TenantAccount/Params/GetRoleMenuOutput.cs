using EasyIotSharp.Core.Domain.TenantAccount;
using System;
using System.Collections.Generic;
using System.Text;
using UPrime.AutoMapper;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过id查询一条角色菜单信息的出参类
    /// </summary>
    [AutoMapFrom(typeof(Role))]
    public class GetRoleMenuOutput
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
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 菜单信息
        /// </summary>
        public List<MenuDto> Menus { get; set; }
    }
}

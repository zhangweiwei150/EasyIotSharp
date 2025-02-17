using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.TenantAccount
{
    /// <summary>
    /// 租户菜单表
    /// </summary>
    [SugarTable("Menus")]
    public class Menu : BaseEntity<string>
    {
        /// <summary>
        /// 父级id
        /// </summary>
        public string ParentId{ get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 类型
        /// 1=菜单
        /// 2=路由
        /// 3=按钮
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 是否admin(超级管理员，没有租户限制)
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}

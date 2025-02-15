using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过用户id查询对应菜单列表的出参类
    /// </summary>
    public class QueryMenuBySoldierIdOutput
    {
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
        /// 子类菜单
        /// </summary>
        public List<ChildrenMenu> Children { get; set; }
    }
    public class ChildrenMenu
    {
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
        /// 子类菜单
        /// </summary>
        public List<ChildrenMenu> Children { get; set; }
    }
}

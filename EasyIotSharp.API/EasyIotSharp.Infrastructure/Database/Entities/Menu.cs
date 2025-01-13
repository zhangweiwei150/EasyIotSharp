using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database.Entities
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [SugarTable("Menus", TableDescription = "菜单表")]
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "菜单主键")]
        public Guid Id { get; set; }

        [SugarColumn(ColumnDescription = "所属租户ID")]
        public Guid TenantId { get; set; }

        [SugarColumn(ColumnDescription = "父菜单ID")]
        public Guid? ParentId { get; set; }

        [SugarColumn(ColumnDescription = "菜单名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnDescription = "菜单路由")]
        public string Url { get; set; }

        [SugarColumn(ColumnDescription = "菜单图标")]
        public string Icon { get; set; }

        [SugarColumn(ColumnDescription = "权限标识")]
        public string PermissionKey { get; set; }

        [SugarColumn(ColumnDescription = "是否是按钮")]
        public bool IsButton { get; set; }

        [SugarColumn(ColumnDescription = "排序权重")]
        public int Order { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsActive { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}

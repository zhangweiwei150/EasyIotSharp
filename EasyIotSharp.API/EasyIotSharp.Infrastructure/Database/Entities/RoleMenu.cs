using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database.Entities
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [SugarTable("RoleMenus", TableDescription = "角色菜单表")]
    public class RoleMenu
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "角色ID")]
        public Guid RoleId { get; set; }

        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "菜单ID")]
        public Guid MenuId { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}

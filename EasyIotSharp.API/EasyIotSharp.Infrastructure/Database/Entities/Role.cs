using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database.Entities
{
    /// <summary>
    /// 角色表
    /// </summary>
    [SugarTable("Roles", TableDescription = "角色表")]
    public class Role
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "角色主键")]
        public Guid Id { get; set; }

        [SugarColumn(ColumnDescription = "所属租户ID")]
        public Guid TenantId { get; set; }

        [SugarColumn(ColumnDescription = "角色名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnDescription = "角色描述")]
        public string Description { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsActive { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}

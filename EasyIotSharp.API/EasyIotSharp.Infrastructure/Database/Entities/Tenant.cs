using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database.Entities
{
    /// <summary>
    /// 租户表
    /// </summary>
    [SugarTable("Tenants", TableDescription = "租户表")]
    public class Tenant
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "租户主键")]
        public Guid Id { get; set; }

        [SugarColumn(ColumnDescription = "租户名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnDescription = "租户描述")]
        public string Description { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsActive { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}

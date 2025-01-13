using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database.Entities
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [SugarTable("UserRoles", TableDescription = "用户角色表")]
    public class UserRole
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "用户ID")]
        public Guid UserId { get; set; }

        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "角色ID")]
        public Guid RoleId { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}

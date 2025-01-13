using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("Users", TableDescription = "用户表")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "用户主键")]
        public Guid Id { get; set; }

        [SugarColumn(ColumnDescription = "所属租户ID")]
        public Guid TenantId { get; set; }

        [SugarColumn(ColumnDescription = "用户名")]
        public string UserName { get; set; }

        [SugarColumn(ColumnDescription = "用户密码")]
        public string Password { get; set; }

        [SugarColumn(ColumnDescription = "用户真实姓名")]
        public string RealName { get; set; }

        [SugarColumn(ColumnDescription = "用户邮箱")]
        public string Email { get; set; }

        [SugarColumn(ColumnDescription = "用户手机号")]
        public string Phone { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsActive { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }

}

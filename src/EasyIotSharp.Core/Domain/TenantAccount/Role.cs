using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.TenantAccount
{
    /// <summary>
    /// 租户角色表
    /// </summary>
    [SugarTable("Roles")]
    public class Role : BaseEntity<int>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}

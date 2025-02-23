using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 测点类型表
    /// </summary>
    public class SensorPointType:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 测点名称
        /// </summary>
        public string Name { get; set; }
    }
}

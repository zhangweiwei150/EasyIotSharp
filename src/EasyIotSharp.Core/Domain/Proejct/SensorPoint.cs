using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 测点表
    /// </summary>
    public class SensorPoint:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 测点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public string ClassificationId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 测点类型Id
        /// </summary>
        public string SensorTypeId { get; set; }
    }
}

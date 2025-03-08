using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Hardware
{
    /// <summary>
    /// 传感器表
    /// </summary>
    /// <remarks>一个传感器设备</remarks>
    public class Sensor:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 测点类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string BriefName { get; set; }

        /// <summary>
        /// 厂家名称
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 传感器型号
        /// </summary>
        public string SensorModel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}

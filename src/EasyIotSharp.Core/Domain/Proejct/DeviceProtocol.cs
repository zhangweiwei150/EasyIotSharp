using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 设备协议表
    /// </summary>
    /// <remarks>定于当前设备所选的协议的存储数据</remarks>
    public class DeviceProtocol:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 协议id
        /// </summary>
        public string ProtocolId { get; set; }

        /// <summary>
        /// json定义
        /// </summary>
        public string ConfigJson { get; set; }
    }
}

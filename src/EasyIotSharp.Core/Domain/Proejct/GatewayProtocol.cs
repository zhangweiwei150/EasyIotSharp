using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 网关协议表
    /// </summary>
    /// <remarks>定于当前网关所选的协议传输过来的存储数据</remarks>
    public class GatewayProtocol:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 网关id
        /// </summary>
        public string GatewayId { get; set; }

        /// <summary>
        /// 协议id
        /// </summary>
        public string ProtocolId { get; set; }

        /// <summary>
        /// json定义
        /// 网关发送命令json数据
        /// </summary>
        public string ConfigJson { get; set; }
    }
}

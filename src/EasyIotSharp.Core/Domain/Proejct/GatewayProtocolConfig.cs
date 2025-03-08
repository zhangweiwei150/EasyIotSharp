using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 网关协议配置表
    /// </summary>
    /// <remarks>需要序列化的参数</remarks>
    public class GatewayProtocolConfig:BaseEntity<string>
    {
        /// <summary>
        /// 网关协议id
        /// </summary>
        public string GatewayProtocolId { get; set; }

        /// <summary>
        /// 标识符
        /// （反序列化到class里面对应的字段名）
        /// </summary>
        public string Identifier { get; set; }
    }
}

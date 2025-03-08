using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 田家园一条网关协议的入参类
    /// </summary>
    public class InsertGatewayProtocolInput
    {
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

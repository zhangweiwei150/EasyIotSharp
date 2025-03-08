using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过id修改一条网关协议的入参类
    /// </summary>
    public class UpdateGatewayProtocolInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// json定义
        /// 网关发送命令json数据
        /// </summary>
        public string ConfigJson { get; set; }
    }
}

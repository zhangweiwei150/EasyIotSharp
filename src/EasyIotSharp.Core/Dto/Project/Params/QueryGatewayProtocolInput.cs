using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过条件分页查询网关协议列表的入参类
    /// </summary>
    public class QueryGatewayProtocolInput:PagingInput
    {
        /// <summary>
        /// 网关id
        /// </summary>
        public string GatewayId { get; set; }
    }
}

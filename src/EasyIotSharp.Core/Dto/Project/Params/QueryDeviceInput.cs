using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过条件分页查询设备列表的入参类
    /// </summary>
    public class QueryDeviceInput:PagingInput
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 设备状态
        /// -1=全部
        /// 0=初始化
        /// 1=在线
        /// 2=离线
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 协议id
        /// </summary>
        public string ProtocolId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Hardware.Params
{
    /// <summary>
    /// 通过id修改一条协议信息的入参类
    /// </summary>
    public class UpdateProtocolInput
    {
        /// <summary>
        /// 协议id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 协议名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 协议描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}

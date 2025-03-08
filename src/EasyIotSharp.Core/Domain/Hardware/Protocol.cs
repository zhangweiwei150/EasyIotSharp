using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Hardware
{
    /// <summary>
    /// 协议表
    /// </summary>
    public class Protocol:BaseEntity<string>
    {
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

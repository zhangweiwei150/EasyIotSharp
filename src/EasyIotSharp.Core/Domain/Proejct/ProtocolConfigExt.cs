using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 协议配置扩展表
    /// </summary>
    /// <remarks>代表一条协议配置表单元素关联的额外扩展数据，例如下拉select的option</remarks>
    public class ProtocolConfigExt:BaseEntity<string>
    {
        /// <summary>
        /// 协议配置id
        /// </summary>
        public string ProtocolConfigId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}

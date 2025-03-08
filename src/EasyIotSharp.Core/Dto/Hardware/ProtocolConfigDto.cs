using EasyIotSharp.Core.Domain.Hardware;
using System;
using System.Collections.Generic;
using System.Text;
using UPrime.AutoMapper;

namespace EasyIotSharp.Core.Dto.Hardware
{
    /// <summary>
    /// 代表一条协议配置的"DTO"
    /// </summary>
    [AutoMapFrom(typeof(ProtocolConfig))]
    public class ProtocolConfigDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 协议id
        /// </summary>
        public string ProtocolId { get; set; }

        /// <summary>
        /// 协议名称
        /// </summary>
        public string ProtocolName { get; set; }

        /// <summary>
        /// 标识符
        /// （反序列化到class里面对应的字段名）
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 文本提示
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// <see cref="TagTypeMenu"/>
        /// 标签类型  select、number、text
        /// </summary>
        public TagTypeMenu TagType { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public int IsRequired { get; set; }

        /// <summary>
        /// <see cref="DataTypeMenu"/>
        /// 验证数据类型
        /// </summary>
        public DataTypeMenu ValidateType { get; set; }

        /// <summary>
        /// 验证数据类型不正确提示
        /// </summary>
        public int ValidateMessage { get; set; }

        /// <summary>
        /// 排序字段
        /// (数字越大越靠前)
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 操作人标识
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
    }
}

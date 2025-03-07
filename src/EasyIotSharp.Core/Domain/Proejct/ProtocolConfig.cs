using Confluent.Kafka;
using Nest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 协议表单配置表
    /// </summary>
    /// <remarks>构建动态表单</remarks>
    public class ProtocolConfig : BaseEntity<string>
    {
        /// <summary>
        /// 协议id
        /// </summary>
        public string ProtocolId { get; set; }

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
        public int TagType { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public int IsRequired { get; set; }

        /// <summary>
        /// <see cref="DataTypeMenu"/>
        /// 验证数据类型
        /// </summary>
        public int ValidateType { get; set; }

        /// <summary>
        /// 验证数据类型不正确提示
        /// </summary>
        public int ValidateMessage { get; set; }

        /// <summary>
        /// 排序字段
        /// (数字越大越靠前)
        /// </summary>
        public int Sort { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Hardware.Params
{
    /// <summary>
    /// 通过条件分页查询协议列表的入参类
    /// </summary>
    public class QueryProtocolInput:PagingInput
    {
        /// <summary>
        /// 协议名称/描述
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否启用
        /// -1=不参与查询
        /// 1=启用
        /// 2=禁用
        /// </summary>
        public int IsEnable { get; set; }

        /// <summary>
        /// 是否分页
        /// true=分页
        /// false=查询全部
        /// </summary>
        public bool IsPage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Hardware.Params
{
    /// <summary>
    /// 通过条件分页查询传感器的入参类
    /// </summary>
    public class QuerySensorInput:PagingInput
    {
        /// <summary>
        /// 名称/简称/厂家
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否分页
        /// true=分页
        /// false=查询全部
        /// </summary>
        public bool IsPage { get; set; }
    }
}

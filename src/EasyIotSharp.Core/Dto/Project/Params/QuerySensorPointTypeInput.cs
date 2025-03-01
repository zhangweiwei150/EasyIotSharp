using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过条件分页查询测点类型的入参类
    /// </summary>
    public class QuerySensorPointTypeInput:PagingInput
    {
        /// <summary>
        /// 是否分页
        /// true=分页
        /// false=查询全部
        /// </summary>
        public bool IsPage { get; set; }
    }
}

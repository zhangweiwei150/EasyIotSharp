using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 根据条件分页查询传感器类型指标列表的入参类
    /// </summary>
    public class QuerySensorPointTypeQuotaInput:PagingInput
    {
        /// <summary>
        /// 传感器类型id
        /// </summary>
        public string SensorPointTypeId { get; set; }

        /// <summary>
        /// 指标名称/标识符
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// <see cref="DataTypeMenu"/>
        /// 数据类型
        /// </summary>
        public DataTypeMenu DataType { get; set; }
    }
}

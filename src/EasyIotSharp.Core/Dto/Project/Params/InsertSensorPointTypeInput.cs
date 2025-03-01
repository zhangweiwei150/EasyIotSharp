using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 添加一条测点类型的入参类
    /// </summary>
    public class InsertSensorPointTypeInput
    {
        /// <summary>
        /// 测点类型名称
        /// </summary>
        public string Name { get; set; }
    }
}

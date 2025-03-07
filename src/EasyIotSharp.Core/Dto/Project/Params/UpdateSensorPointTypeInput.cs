using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过id修改一条测点类型的入参类
    /// </summary>
    public class UpdateSensorPointTypeInput
    {
        /// <summary>
        /// 测点类型id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 测点类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string BriefName { get; set; }

        /// <summary>
        /// 厂家名称
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 传感器型号
        /// </summary>
        public string SensorModel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}

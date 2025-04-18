﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Hardware.Params
{
    /// <summary>
    /// 添加一条传感器的入参类
    /// </summary>
    public class InsertSensorInput
    {
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

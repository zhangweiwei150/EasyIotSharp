using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Hardware.Params
{
    /// <summary>
    /// 通过id删除一条传感器类型指标的入参类
    /// </summary>
    public class DeleteSensorQuotaInput
    {
        /// <summary>
        /// 传感器指标id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否删除 
        /// </summary>
        public bool IsDelete { get; set; }
    }
}

using System.Collections.Generic;

namespace EasyIotSharp.GateWay.Core.Model.ConfigDTO
{
    /// <summary>
    /// Modbus配置模型
    /// </summary>
    public class ModbusConfigModel
    {
        /// <summary>
        /// 表单数据
        /// </summary>
        public ModbusFormData FormData { get; set; }

        /// <summary>
        /// 测量点
        /// </summary>
        public string MeasurementPoint { get; set; }

        /// <summary>
        /// 配置ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 网关ID
        /// </summary>
        public string GatewayId { get; set; }

        /// <summary>
        /// 协议ID
        /// </summary>
        public string ProtocolId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string projectId { get; set; }
    }

    /// <summary>
    /// Modbus表单数据
    /// </summary>
    public class ModbusFormData
    {
        /// <summary>
        /// 功能码
        /// </summary>
        public byte FunctionCode { get; set; }

        /// <summary>
        /// 轮询间隔(秒)
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 起始地址
        /// </summary>
        public ushort StartingAddress { get; set; }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        public ushort Quantity { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 从站地址
        /// </summary>
        public byte Address { get; set; }

        /// <summary>
        /// 是否标识符
        /// </summary>
        public bool IsIdentifier { get; set; }

        /// <summary>
        /// 系数K
        /// </summary>
        public double K { get; set; }
    }
}
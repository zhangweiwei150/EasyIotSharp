using HPSocket;
using System.Collections.Concurrent;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Models
{
    /// <summary>
    /// 设备上下文，存储设备相关信息
    /// </summary>
    public class DeviceContext
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string? DeviceId { get; set; }
        
        /// <summary>
        /// 连接ID
        /// </summary>
        public IntPtr ConnectionId { get; set; }
        
        /// <summary>
        /// 服务器实例
        /// </summary>
        public IServer? Server { get; set; }
        
        /// <summary>
        /// 协议类型
        /// </summary>
        public string? ProtocolType { get; set; }
        
        /// <summary>
        /// 设备配置
        /// </summary>
        public DeviceConfig? Config { get; set; }
        
        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActiveTime { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 是否新连接
        /// </summary>
        public bool IsNewConnection { get; set; }
        
        /// <summary>
        /// 设备属性字典
        /// </summary>
        public ConcurrentDictionary<string, object> Properties { get; } = new();
        
        /// <summary>
        /// 发送数据
        /// </summary>
        public async Task<bool> SendAsync(byte[] data)
        {
            if (Server == null || ConnectionId == IntPtr.Zero)
                return false;
                
            return await Task.Run(() => Server.Send(ConnectionId, data, data.Length));
        }
        
        public bool IsConnected { get; set; } = true;
        public int ReconnectAttempts { get; set; } = 0;
        public DateTime LastReconnectTime { get; set; }
        public string LastKnownIp { get; set; }
        public int LastKnownPort { get; set; }
    }
    
    /// <summary>
    /// 设备配置
    /// </summary>
    public class DeviceConfig
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string? DeviceId { get; set; }
        
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }
        
        /// <summary>
        /// Modbus从站地址
        /// </summary>
        public byte ModbusSlaveAddress { get; set; } = 1;
        
        /// <summary>
        /// 轮询间隔(毫秒)
        /// </summary>
        public int PollingInterval { get; set; } = 5000;
        
        /// <summary>
        /// 寄存器配置列表
        /// </summary>
        public List<ModbusRegisterConfig>? Registers { get; set; }
    }
    
    /// <summary>
    /// Modbus寄存器配置
    /// </summary>
    public class ModbusRegisterConfig
    {
        /// <summary>
        /// 寄存器地址
        /// </summary>
        public ushort Address { get; set; }
        
        /// <summary>
        /// 寄存器类型
        /// </summary>
        public ModbusRegisterType RegisterType { get; set; }
        
        /// <summary>
        /// 寄存器数量
        /// </summary>
        public ushort Count { get; set; } = 1;
        
        /// <summary>
        /// 数据类型
        /// </summary>
        public string? DataType { get; set; }
        
        /// <summary>
        /// 属性名称
        /// </summary>
        public string? PropertyName { get; set; }
    }
    
    /// <summary>
    /// Modbus寄存器类型
    /// </summary>
    public enum ModbusRegisterType
    {
        /// <summary>
        /// 线圈
        /// </summary>
        Coil = 0,
        
        /// <summary>
        /// 离散输入
        /// </summary>
        DiscreteInput = 1,
        
        /// <summary>
        /// 保持寄存器
        /// </summary>
        HoldingRegister = 2,
        
        /// <summary>
        /// 输入寄存器
        /// </summary>
        InputRegister = 3
    }
    
    /// <summary>
    /// 设备注册信息
    /// </summary>
    public class DeviceRegistration
    {
        /// <summary>
        /// 设备IMEI
        /// </summary>
        public string? imei { get; set; }
        
        /// <summary>
        /// SIM卡ICCID
        /// </summary>
        public string? iccid { get; set; }
        
        /// <summary>
        /// 固件版本
        /// </summary>
        public string? fver { get; set; }
        
        /// <summary>
        /// 信号强度
        /// </summary>
        public int csq { get; set; }
    }
}
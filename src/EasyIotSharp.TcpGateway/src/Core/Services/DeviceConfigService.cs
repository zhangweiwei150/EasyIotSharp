using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using Microsoft.Extensions.Logging;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Services
{
    /// <summary>
    /// 设备配置服务
    /// </summary>
    public class DeviceConfigService : IDeviceConfigService
    {
        private readonly ILogger<DeviceConfigService> _logger;
        
        // 模拟设备配置缓存
        private readonly Dictionary<string, DeviceConfig> _deviceConfigs = new();
        
        public DeviceConfigService(ILogger<DeviceConfigService> logger)
        {
            _logger = logger;
            
            // 初始化一些测试设备配置
            InitTestDeviceConfigs();
        }
        
        /// <summary>
        /// 获取设备配置
        /// </summary>
        public Task<DeviceConfig> GetDeviceConfigAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                _logger.LogWarning("设备ID为空");
                return Task.FromResult<DeviceConfig>(null);
            }
            
            if (_deviceConfigs.TryGetValue(deviceId, out var config))
            {
                _logger.LogInformation("找到设备 {DeviceId} 的配置", deviceId);
                return Task.FromResult(config);
            }
            
            _logger.LogWarning("未找到设备 {DeviceId} 的配置", deviceId);
            return Task.FromResult<DeviceConfig>(null);
        }
        
        /// <summary>
        /// 初始化测试设备配置
        /// </summary>
        private void InitTestDeviceConfigs()
        {
            var config = new DeviceConfig
            {
                DeviceId = "861290071163062",
                DeviceName = "测试DTU",
                ModbusSlaveAddress = 1,  // Modbus 从站地址
                PollingInterval = 5000,  // 5秒轮询一次
                Registers = new List<ModbusRegisterConfig>
                {
                    new ModbusRegisterConfig
                    {
                        Address = 0x0000,  // 起始地址
                        RegisterType = ModbusRegisterType.HoldingRegister,
                        Count = 2,  // 读取2个寄存器
                        DataType = "Int16",
                        PropertyName = "Temperature"
                    },
                    new ModbusRegisterConfig
                    {
                        Address = 0x0002,  // 第二个寄存器地址
                        RegisterType = ModbusRegisterType.HoldingRegister,
                        Count = 2,
                        DataType = "Int16",
                        PropertyName = "Humidity"
                    }
                }
            };
            
            _deviceConfigs[config.DeviceId] = config;
            _logger.LogInformation("已添加测试设备配置: {DeviceId}", config.DeviceId);
        }
    }
}
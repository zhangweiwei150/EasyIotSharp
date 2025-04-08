using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace EasyIotSharp.Cloud.TcpGateway.src.ProtocolServices.Modbus
{
    /// <summary>
    /// Modbus RTU协议处理器
    /// </summary>
    public class ModbusRtuProcessor : IProtocolProcessor
    {
        private readonly ILogger<ModbusRtuProcessor> _logger;
        private readonly IDeviceConfigService _deviceConfigService;
        
        public string ProtocolType => "ModbusRtu";
        
        public ModbusRtuProcessor(
            ILogger<ModbusRtuProcessor> logger,
            IDeviceConfigService deviceConfigService)
        {
            _logger = logger;
            _deviceConfigService = deviceConfigService;
        }
        
        /// <summary>
        /// 处理设备注册
        /// </summary>
        public async Task ProcessRegistrationAsync(DeviceRegistration registration, DeviceContext context)
        {
            _logger.LogInformation("处理Modbus RTU设备注册: {Imei}", registration.imei);
            
            // 设置设备ID
            context.DeviceId = registration.imei;
            
            // 获取设备配置
            var config = await _deviceConfigService.GetDeviceConfigAsync(registration.imei);
            if (config != null)
            {
                context.Config = config;
                context.ProtocolType = ProtocolType;
                
                // 存储Modbus相关配置
                context.Properties["ModbusSlaveAddress"] = config.ModbusSlaveAddress;
                context.Properties["Imei"] = registration.imei;
                context.Properties["Iccid"] = registration.iccid;
                context.Properties["FirmwareVersion"] = registration.fver;
                context.Properties["SignalStrength"] = registration.csq;
                
                _logger.LogInformation("已加载设备 {Imei} 的配置", registration.imei);
            }
            else
            {
                _logger.LogWarning("未找到设备 {Imei} 的配置", registration.imei);
            }
        }
        
        /// <summary>
        /// 处理设备数据
        /// </summary>
        public async Task ProcessDataAsync(byte[] data, DeviceContext context)
        {
            try
            {
                // 如果设备还未注册，尝试解析注册包
                if (string.IsNullOrEmpty(context.DeviceId))
                {
                    string jsonData = Encoding.UTF8.GetString(data);
                    if (jsonData.StartsWith("{") && jsonData.EndsWith("}"))
                    {
                        var registration = JsonSerializer.Deserialize<DeviceRegistration>(jsonData);
                        if (registration != null && !string.IsNullOrEmpty(registration.imei))
                        {
                            await ProcessRegistrationAsync(registration, context);
                            return;
                        }
                    }
                }

                // 已注册设备的数据处理
                _logger.LogInformation("处理Modbus RTU数据: {DeviceId}, 长度: {Length}", 
                    context.DeviceId, data.Length);
                
                // 更新最后活动时间
                context.LastActiveTime = DateTime.Now;
                
                // 解析Modbus响应
                if (data.Length >= 5)
                {
                    byte slaveAddress = data[0];
                    byte functionCode = data[1];
                    
                    _logger.LogInformation("Modbus响应: 从站地址={SlaveAddress}, 功能码={FunctionCode}", 
                        slaveAddress, functionCode);
                    
                    // 根据功能码解析不同类型的响应
                    switch (functionCode)
                    {
                        case 1: // 读线圈
                        case 2: // 读离散输入
                            ParseDigitalResponse(data, context);
                            break;
                        case 3: // 读保持寄存器
                        case 4: // 读输入寄存器
                            ParseRegisterResponse(data, context);
                            break;
                        default:
                            _logger.LogWarning("不支持的功能码: {FunctionCode}", functionCode);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理Modbus RTU数据时出错: {DeviceId}", context.DeviceId);
            }
        }
        
        /// <summary>
        /// 解析数字量响应
        /// </summary>
        private void ParseDigitalResponse(byte[] data, DeviceContext context)
        {
            if (data.Length < 4)
                return;
                
            byte slaveAddress = data[0];
            byte functionCode = data[1];
            byte byteCount = data[2];
            
            if (data.Length < 3 + byteCount)
                return;
                
            // 解析数字量数据
            for (int i = 0; i < byteCount; i++)
            {
                byte value = data[3 + i];
                
                // 处理每个位
                for (int bit = 0; bit < 8; bit++)
                {
                    bool bitValue = (value & (1 << bit)) != 0;
                    int address = i * 8 + bit;
                    
                    _logger.LogInformation("数字量: 地址={Address}, 值={Value}", address, bitValue);
                    
                    // 存储数据
                    string key = $"Digital_{address}";
                    context.Properties[key] = bitValue;
                }
            }
        }
        
        /// <summary>
        /// 解析寄存器响应
        /// </summary>
        private void ParseRegisterResponse(byte[] data, DeviceContext context)
        {
            if (data.Length < 4)
                return;
                
            byte slaveAddress = data[0];
            byte functionCode = data[1];
            byte byteCount = data[2];
            
            if (data.Length < 3 + byteCount)
                return;
                
            // 解析寄存器数据
            for (int i = 0; i < byteCount / 2; i++)
            {
                ushort value = (ushort)((data[3 + i * 2] << 8) | data[4 + i * 2]);
                
                _logger.LogInformation("寄存器: 地址={Address}, 值={Value}", i, value);
                
                // 存储数据
                string key = $"Register_{i}";
                context.Properties[key] = value;
            }
        }
        
        /// <summary>
        /// 构建Modbus命令
        /// </summary>
        public Task<byte[]> BuildCommandAsync(DeviceContext context)
        {
            try
            {
                if (context.Config == null || context.Config.Registers == null || !context.Config.Registers.Any())
                {
                    _logger.LogWarning("设备 {DeviceId} 没有配置寄存器", context.DeviceId);
                    return Task.FromResult<byte[]>(null);
                }
                
                // 获取从站地址
                byte slaveAddress = context.Config.ModbusSlaveAddress;
                
                // 选择第一个寄存器配置构建命令
                var register = context.Config.Registers.First();
                
                // 构建Modbus RTU命令
                byte[] command = BuildModbusRtuCommand(slaveAddress, register);
                
                return Task.FromResult(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "构建Modbus命令时出错: {DeviceId}", context.DeviceId);
                return Task.FromResult<byte[]>(null);
            }
        }
        
        /// <summary>
        /// 构建Modbus RTU命令
        /// </summary>
        private byte[] BuildModbusRtuCommand(byte slaveAddress, ModbusRegisterConfig register)
        {
            byte functionCode;
            
            // 根据寄存器类型选择功能码
            switch (register.RegisterType)
            {
                case ModbusRegisterType.Coil:
                    functionCode = 1; // 读线圈
                    break;
                case ModbusRegisterType.DiscreteInput:
                    functionCode = 2; // 读离散输入
                    break;
                case ModbusRegisterType.HoldingRegister:
                    functionCode = 3; // 读保持寄存器
                    break;
                case ModbusRegisterType.InputRegister:
                    functionCode = 4; // 读输入寄存器
                    break;
                default:
                    functionCode = 3; // 默认读保持寄存器
                    break;
            }
            
            // 构建Modbus RTU请求
            byte[] command = new byte[8];
            command[0] = slaveAddress;        // 从站地址
            command[1] = functionCode;        // 功能码
            command[2] = (byte)(register.Address >> 8);    // 起始地址高字节
            command[3] = (byte)(register.Address & 0xFF);  // 起始地址低字节
            command[4] = (byte)(register.Count >> 8);      // 寄存器数量高字节
            command[5] = (byte)(register.Count & 0xFF);    // 寄存器数量低字节
            
            // 计算CRC校验
            ushort crc = CalculateCrc(command, 0, 6);
            command[6] = (byte)(crc & 0xFF);       // CRC低字节
            command[7] = (byte)(crc >> 8);         // CRC高字节
            
            _logger.LogInformation("构建Modbus RTU命令: 从站地址={SlaveAddress}, 功能码={FunctionCode}, 起始地址={Address}, 数量={Count}",
                slaveAddress, functionCode, register.Address, register.Count);
            
            return command;
        }
        
        /// <summary>
        /// 计算CRC校验
        /// </summary>
        private ushort CalculateCrc(byte[] data, int offset, int count)
        {
            ushort crc = 0xFFFF;
            
            for (int i = offset; i < offset + count; i++)
            {
                crc ^= data[i];
                
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            
            return crc;
        }
        
        /// <summary>
        /// 验证CRC校验
        /// </summary>
        private bool ValidateCrc(byte[] data)
        {
            if (data.Length < 2)
                return false;
                
            int dataLength = data.Length - 2;
            ushort receivedCrc = (ushort)((data[dataLength + 1] << 8) | data[dataLength]);
            ushort calculatedCrc = CalculateCrc(data, 0, dataLength);
            
            return receivedCrc == calculatedCrc;
        }
    }
}
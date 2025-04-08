using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.ProtocolServices.Modbus
{
    /// <summary>
    /// Modbus RTU协议识别器
    /// </summary>
    public class ModbusRtuIdentifier : IProtocolIdentifier
    {
        private readonly ILogger<ModbusRtuIdentifier> _logger;
        private readonly IProtocolProcessor _processor;

        public ModbusRtuIdentifier(
            ILogger<ModbusRtuIdentifier> logger,
            IProtocolProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        /// <summary>
        /// 识别Modbus RTU协议
        /// </summary>
        public Task<string> IdentifyAsync(byte[] data, DeviceContext device)
        {
            // 检查是否是JSON注册包
            if (IsJsonRegistrationPacket(data, device))
            {
                _logger.LogInformation("识别到JSON格式Modbus RTU注册包: {DeviceId}", device.DeviceId);
                return Task.FromResult("ModbusRtu");
            }
            
            // 检查是否是ASCII注册包
            if (IsAsciiRegistrationPacket(data, device))
            {
                _logger.LogInformation("识别到ASCII格式Modbus RTU注册包: {DeviceId}", device.DeviceId);
                return Task.FromResult("ModbusRtu");
            }

            // 检查是否是Modbus RTU协议
            if (IsModbusRtuPacket(data))
            {
                _logger.LogInformation("识别到Modbus RTU协议: {DeviceId}", device.DeviceId);
                return Task.FromResult("ModbusRtu");
            }

            return Task.FromResult<string>(null);
        }
        
        /// <summary>
        /// 检查是否是JSON注册包
        /// </summary>
        private bool IsJsonRegistrationPacket(byte[] data, DeviceContext device)
        {
            try
            {
                // 尝试将数据转换为UTF8字符串
                string jsonData = Encoding.UTF8.GetString(data);
                
                // 检查是否是有效的JSON格式
                if (jsonData.StartsWith("{") && jsonData.EndsWith("}"))
                {
                    // 尝试解析为DeviceRegistration对象
                    var registration = JsonSerializer.Deserialize<DeviceRegistration>(jsonData);
                    
                    if (registration != null && !string.IsNullOrEmpty(registration.imei))
                    {
                        // 设置设备ID为IMEI
                        device.DeviceId = registration.imei;
                        
                        // 存储设备信息到上下文
                        device.Properties["Imei"] = registration.imei;
                        device.Properties["Iccid"] = registration.iccid;
                        device.Properties["FirmwareVersion"] = registration.fver;
                        device.Properties["SignalStrength"] = registration.csq;
                        
                        // 设置Modbus相关配置
                        device.Properties["ModbusMode"] = "RTU";
                        device.Properties["ModbusUnitId"] = 1; // 默认单元ID
                        
                        // 异步处理注册信息
                        _ = _processor.ProcessRegistrationAsync(registration, device);
                        
                        return true;
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogInformation(ex, "JSON解析失败，可能不是JSON注册包");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析JSON注册包时出错");
            }
            
            return false;
        }

        /// <summary>
        /// 检查是否是ASCII注册包
        /// </summary>
        private bool IsAsciiRegistrationPacket(byte[] data, DeviceContext device)
        {
            try
            {
                // 尝试将数据转换为ASCII字符串
                string asciiData = Encoding.ASCII.GetString(data);
                
                // 检查是否包含Modbus RTU设备的特定标识
                if (asciiData.Contains("MODBUS_RTU") || asciiData.Contains("RTU") || 
                    asciiData.Contains("485") || asciiData.Contains("RS485"))
                {
                    // 从注册包中提取设备ID等信息
                    string deviceId = ExtractDeviceIdFromRegistration(asciiData);
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        device.DeviceId = deviceId;
                    }
                    
                    // 存储设备特定的Modbus配置
                    device.Properties["ModbusMode"] = "RTU";
                    device.Properties["ModbusUnitId"] = 1; // 默认单元ID，可从注册包中提取
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析ASCII注册包时出错");
            }
            
            return false;
        }

        /// <summary>
        /// 从注册包中提取设备ID
        /// </summary>
        private string ExtractDeviceIdFromRegistration(string asciiData)
        {
            // 这里需要根据实际的注册包格式进行修改
            // 示例：假设注册包格式为 "REGISTER:MODBUS_RTU:DeviceID:12345"
            try
            {
                string[] parts = asciiData.Split(':');
                if (parts.Length >= 4 && 
                    (parts[0] == "REGISTER" || parts[0].Contains("REG")) && 
                    (parts[1] == "MODBUS_RTU" || parts[1].Contains("RTU")))
                {
                    return parts[3];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从注册包中提取设备ID时出错");
            }
            
            return null;
        }

        /// <summary>
        /// 检查是否是Modbus RTU协议
        /// </summary>
        private bool IsModbusRtuPacket(byte[] data)
        {
            // Modbus RTU协议判断逻辑
            if (data.Length < 4)
                return false;

            // 检查功能码是否在有效范围内
            byte functionCode = data[1];
            if (functionCode < 1 || functionCode > 127)
                return false;

            // 验证CRC校验 (最后两个字节)
            // 注意：这里应该实现完整的CRC校验逻辑
            // 简化版本：检查是否有足够的字节包含CRC
            if (data.Length < 4) // 至少需要地址(1) + 功能码(1) + CRC(2)
                return false;

            return true;
        }
    }
}
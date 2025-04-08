using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;

namespace EasyIotSharp.Cloud.TcpGateway.src.ProtocolServices.Modbus
{
    /// <summary>
    /// Modbus协议处理器
    /// </summary>
    public class ModbusProcessor : IProtocolProcessor
    {
        private readonly ILogger<ModbusProcessor> _logger;
        private readonly IDeviceConfigService _deviceConfigService;

        public ModbusProcessor(
            ILogger<ModbusProcessor> logger,
            IDeviceConfigService deviceConfigService)
        {
            _logger = logger;
            _deviceConfigService = deviceConfigService;
        }

        /// <summary>
        /// 协议类型
        /// </summary>
        public string ProtocolType => "Modbus";

        /// <summary>
        /// 处理设备注册
        /// </summary>
        public async Task ProcessRegistrationAsync(DeviceRegistration registration, DeviceContext context)
        {
            _logger.LogInformation("处理Modbus设备注册: {Imei}", registration.imei);
            
            // 设置设备ID
            context.DeviceId = registration.imei;
            
            // 获取设备配置
            var config = await _deviceConfigService.GetDeviceConfigAsync(registration.imei);
            if (config != null)
            {
                context.Config = config;
                context.ProtocolType = ProtocolType;
                
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
        public Task ProcessDataAsync(byte[] data, DeviceContext context)
        {
            try
            {
                _logger.LogInformation("处理Modbus数据: {Data}", BitConverter.ToString(data));
                
                // 确保数据长度足够
                if (data.Length < 8)
                {
                    _logger.LogWarning("Modbus数据长度不足: {Length}", data.Length);
                    return Task.CompletedTask;
                }
                
                // 解析Modbus数据
                ushort transactionId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(0, 2));
                ushort protocolId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(2, 2));
                ushort length = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(4, 2));
                byte unitId = data[6];
                byte functionCode = data[7];
                
                _logger.LogInformation("Modbus数据解析: 事务ID={TransactionId}, 功能码={FunctionCode}, 单元ID={UnitId}", 
                    transactionId, functionCode, unitId);
                
                // 根据功能码处理不同类型的请求
                switch (functionCode)
                {
                    case 1: // 读线圈
                    case 2: // 读离散输入
                    case 3: // 读保持寄存器
                    case 4: // 读输入寄存器
                        ProcessReadResponse(context, functionCode, data);
                        break;
                    case 5: // 写单个线圈
                    case 6: // 写单个寄存器
                        ProcessWriteSingleResponse(context, functionCode, data);
                        break;
                    case 15: // 写多个线圈
                    case 16: // 写多个寄存器
                        ProcessWriteMultipleResponse(context, functionCode, data);
                        break;
                    default:
                        _logger.LogWarning("不支持的Modbus功能码: {FunctionCode}", functionCode);
                        break;
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Modbus处理错误");
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 构建命令
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
                
                // 选择第一个寄存器配置构建命令
                var register = context.Config.Registers.First();
                
                // 构建Modbus TCP命令
                byte[] command = BuildModbusTcpCommand(register);
                
                return Task.FromResult(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "构建Modbus命令时出错: {DeviceId}", context.DeviceId);
                return Task.FromResult<byte[]>(null);
            }
        }

        /// <summary>
        /// 构建Modbus TCP命令
        /// </summary>
        private byte[] BuildModbusTcpCommand(ModbusRegisterConfig register)
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
            
            // 构建Modbus TCP请求
            byte[] command = new byte[12];
            
            // MBAP头部
            BinaryPrimitives.WriteUInt16BigEndian(command.AsSpan(0, 2), 1);  // 事务ID
            BinaryPrimitives.WriteUInt16BigEndian(command.AsSpan(2, 2), 0);  // 协议ID (0 for Modbus TCP)
            BinaryPrimitives.WriteUInt16BigEndian(command.AsSpan(4, 2), 6);  // 后续字节长度
            command[6] = 1;  // 单元ID (默认为1)
            
            // PDU
            command[7] = functionCode;  // 功能码
            BinaryPrimitives.WriteUInt16BigEndian(command.AsSpan(8, 2), register.Address);  // 起始地址
            BinaryPrimitives.WriteUInt16BigEndian(command.AsSpan(10, 2), register.Count);   // 寄存器数量
            
            _logger.LogInformation("构建Modbus TCP命令: 功能码={FunctionCode}, 起始地址={Address}, 数量={Count}",
                functionCode, register.Address, register.Count);
            
            return command;
        }

        private void ProcessReadResponse(DeviceContext context, byte functionCode, byte[] data)
        {
            // 实现读响应处理逻辑
            _logger.LogInformation("处理Modbus读响应: 功能码={FunctionCode}", functionCode);
            
            if (data.Length < 10)  // MBAP头(7) + 功能码(1) + 字节数(1) + 至少1个数据字节(1)
                return;
                
            byte byteCount = data[8];
            
            if (data.Length < 9 + byteCount)
                return;
                
            // 解析数据
            for (int i = 0; i < byteCount / 2; i++)
            {
                ushort value = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(9 + i * 2, 2));
                
                // 存储数据
                string key = $"Register_{i}";
                context.Properties[key] = value;
                
                _logger.LogInformation("寄存器值: {Key}={Value}", key, value);
            }
        }

        private void ProcessWriteSingleResponse(DeviceContext context, byte functionCode, byte[] data)
        {
            // 实现写单个响应处理逻辑
            _logger.LogInformation("处理Modbus写单个响应: 功能码={FunctionCode}", functionCode);
            
            if (data.Length < 12)  // MBAP头(7) + 功能码(1) + 地址(2) + 值(2)
                return;
                
            ushort address = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(8, 2));
            ushort value = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(10, 2));
            
            _logger.LogInformation("写入确认: 地址={Address}, 值={Value}", address, value);
        }

        private void ProcessWriteMultipleResponse(DeviceContext context, byte functionCode, byte[] data)
        {
            // 实现写多个响应处理逻辑
            _logger.LogInformation("处理Modbus写多个响应: 功能码={FunctionCode}", functionCode);
            
            if (data.Length < 12)  // MBAP头(7) + 功能码(1) + 起始地址(2) + 寄存器数量(2)
                return;
                
            ushort startAddress = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(8, 2));
            ushort quantity = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(10, 2));
            
            _logger.LogInformation("写入多个确认: 起始地址={StartAddress}, 数量={Quantity}", startAddress, quantity);
        }
    }
}

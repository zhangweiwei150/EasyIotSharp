using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.ProtocolServices.Modbus
{
    public class ModbusProcessor : IProtocolProcessor
    {
        private readonly ILogger<ModbusProcessor> _logger;

        public ModbusProcessor(ILogger<ModbusProcessor> logger)
        {
            _logger = logger;
        }

        public Task ProcessAsync(ConnectionContext context, byte[] data)
        {
            try
            {
                if (data.Length < 8)
                {
                    _logger.LogWarning("Invalid MODBUS frame length: {Length}", data.Length);
                    return Task.CompletedTask;
                }

                // 解析协议头
                var transactionId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(0, 2));
                var protocolId = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(2, 2));
                var length = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(4, 2));
                var unitId = data[6];
                var functionCode = data[7];

                _logger.LogInformation(
                    "MODBUS Frame[Transaction:{TransactionId}, Function:{FunctionCode}] from {RemoteEndPoint}",
                    transactionId, functionCode, context.RemoteEndPoint);

                // 业务处理逻辑
                switch (functionCode)
                {
                    case 0x03:
                        ProcessReadRegisters(context, data.AsSpan(8));
                        break;
                    case 0x10:
                        ProcessWriteRegisters(context, data.AsSpan(8));
                        break;
                    default:
                        _logger.LogWarning("Unsupported function code: 0x{FunctionCode:X2}", functionCode);
                        break;
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MODBUS processing error");
                return Task.FromException(ex);
            }
        }

        private void ProcessReadRegisters(ConnectionContext context, ReadOnlySpan<byte> data)
        {
            // 实现读寄存器逻辑
            var startAddress = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(0, 2));
            var quantity = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(2, 2));

            _logger.LogInformation("Read Holding Registers: Start={Start}, Count={Count}",
                startAddress, quantity);
        }

        private void ProcessWriteRegisters(ConnectionContext context, ReadOnlySpan<byte> data)
        {
            // 实现写寄存器逻辑
            var startAddress = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(0, 2));
            var quantity = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(2, 2));

            _logger.LogInformation("Write Multiple Registers: Start={Start}, Count={Count}",
                startAddress, quantity);
        }
    }
}

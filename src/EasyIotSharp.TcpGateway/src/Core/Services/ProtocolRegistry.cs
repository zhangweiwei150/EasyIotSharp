using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Services
{
    /// <summary>
    /// 协议注册表
    /// </summary>
    public class ProtocolRegistry : IProtocolRegistry
    {
        private readonly ILogger<ProtocolRegistry> _logger;
        private readonly ConcurrentDictionary<string, IProtocolProcessor> _processors = new();
        private readonly List<IProtocolIdentifier> _identifiers = new();
        
        public ProtocolRegistry(
            ILogger<ProtocolRegistry> logger,
            IEnumerable<IProtocolProcessor> processors,
            IEnumerable<IProtocolIdentifier> identifiers)
        {
            _logger = logger;
            
            // 注册所有协议处理器
            foreach (var processor in processors)
            {
                RegisterProcessor(processor);
            }
            
            // 注册所有协议识别器
            foreach (var identifier in identifiers)
            {
                _identifiers.Add(identifier);
                _logger.LogInformation("已注册协议识别器: {Type}", identifier.GetType().Name);
            }
        }
        
        /// <summary>
        /// 注册协议处理器
        /// </summary>
        public void RegisterProcessor(IProtocolProcessor processor)
        {
            if (_processors.TryAdd(processor.ProtocolType, processor))
            {
                _logger.LogInformation("已注册协议处理器: {ProtocolType}", processor.ProtocolType);
            }
            else
            {
                _logger.LogWarning("协议处理器已存在: {ProtocolType}", processor.ProtocolType);
            }
        }
        
        /// <summary>
        /// 获取协议处理器
        /// </summary>
        public IProtocolProcessor GetProcessor(string protocolType)
        {
            if (string.IsNullOrEmpty(protocolType))
            {
                _logger.LogWarning("尝试获取空协议类型的处理器");
                return null;
            }
            
            if (_processors.TryGetValue(protocolType, out var processor))
            {
                return processor;
            }
            
            _logger.LogWarning("未找到协议处理器: {ProtocolType}", protocolType);
            return null;
        }
        
        /// <summary>
        /// 获取所有协议处理器
        /// </summary>
        public IEnumerable<IProtocolProcessor> GetAllProcessors()
        {
            return _processors.Values;
        }
        
        /// <summary>
        /// 获取所有协议识别器
        /// </summary>
        public IEnumerable<IProtocolIdentifier> GetAllIdentifiers()
        {
            return _identifiers;
        }
    }
}
namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions
{
    /// <summary>
    /// 协议注册表接口
    /// </summary>
    public interface IProtocolRegistry
    {
        /// <summary>
        /// 注册协议处理器
        /// </summary>
        void RegisterProcessor(IProtocolProcessor processor);
        
        /// <summary>
        /// 获取协议处理器
        /// </summary>
        IProtocolProcessor GetProcessor(string protocolType);
        
        /// <summary>
        /// 获取所有协议处理器
        /// </summary>
        IEnumerable<IProtocolProcessor> GetAllProcessors();
        
        /// <summary>
        /// 获取所有协议识别器
        /// </summary>
        IEnumerable<IProtocolIdentifier> GetAllIdentifiers();
    }
}
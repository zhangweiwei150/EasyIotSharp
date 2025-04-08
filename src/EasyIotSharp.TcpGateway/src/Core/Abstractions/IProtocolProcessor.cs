using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions
{
    /// <summary>
    /// 协议处理器接口
    /// </summary>
    public interface IProtocolProcessor
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        string ProtocolType { get; }
        
        /// <summary>
        /// 处理设备注册
        /// </summary>
        Task ProcessRegistrationAsync(DeviceRegistration registration, DeviceContext context);
        
        /// <summary>
        /// 处理设备数据
        /// </summary>
        Task ProcessDataAsync(byte[] data, DeviceContext context);
        
        /// <summary>
        /// 构建命令
        /// </summary>
        Task<byte[]> BuildCommandAsync(DeviceContext context);
    }
}

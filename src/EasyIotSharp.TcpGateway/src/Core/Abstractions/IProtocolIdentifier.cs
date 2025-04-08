using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions
{
    /// <summary>
    /// 协议识别器接口
    /// </summary>
    public interface IProtocolIdentifier
    {
        /// <summary>
        /// 识别协议类型
        /// </summary>
        Task<string> IdentifyAsync(byte[] data, DeviceContext device);
    }
}
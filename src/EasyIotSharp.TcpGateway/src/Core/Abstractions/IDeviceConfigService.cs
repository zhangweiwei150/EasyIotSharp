using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions
{
    /// <summary>
    /// 设备配置服务接口
    /// </summary>
    public interface IDeviceConfigService
    {
        /// <summary>
        /// 获取设备配置
        /// </summary>
        Task<DeviceConfig> GetDeviceConfigAsync(string deviceId);
    }
}
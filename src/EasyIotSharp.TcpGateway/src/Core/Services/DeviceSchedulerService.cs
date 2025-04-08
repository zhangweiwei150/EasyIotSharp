using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Services
{
    /// <summary>
    /// 设备调度服务
    /// </summary>
    public class DeviceSchedulerService : BackgroundService
    {
        private readonly ILogger<DeviceSchedulerService> _logger;
        private readonly IProtocolRegistry _protocolRegistry;
        
        // 设备字典，键为设备ID
        private readonly ConcurrentDictionary<string, DeviceContext> _devices = new();
        
        // 设备轮询任务字典，键为设备ID
        private readonly ConcurrentDictionary<string, Timer> _pollingTimers = new();
        
        public DeviceSchedulerService(
            ILogger<DeviceSchedulerService> logger,
            IProtocolRegistry protocolRegistry)
        {
            _logger = logger;
            _protocolRegistry = protocolRegistry;
        }
        
        /// <summary>
        /// 注册设备
        /// </summary>
        public void RegisterDevice(DeviceContext device)
        {
            if (string.IsNullOrEmpty(device.DeviceId))
            {
                _logger.LogWarning("尝试注册设备ID为空的设备");
                return;
            }
            
            if (_devices.TryAdd(device.DeviceId, device))
            {
                _logger.LogInformation("设备 {DeviceId} 已注册", device.DeviceId);
                
                // 启动设备轮询
                StartDevicePolling(device);
            }
            else
            {
                // 更新设备信息
                if (_devices.TryGetValue(device.DeviceId, out var existingDevice))
                {
                    // 更新连接ID
                    existingDevice.ConnectionId = device.ConnectionId;
                    existingDevice.Server = device.Server;
                    existingDevice.LastActiveTime = DateTime.Now;
                    
                    _logger.LogInformation("设备 {DeviceId} 已更新", device.DeviceId);
                }
            }
        }
        
        /// <summary>
        /// 启动设备轮询
        /// </summary>
        private void StartDevicePolling(DeviceContext device)
        {
            // 获取轮询间隔
            int interval = device.Config?.PollingInterval ?? 5000;
            
            // 创建轮询定时器
            var timer = new Timer(async state => await PollDeviceAsync((DeviceContext)state), 
                device, 1000, interval);
            
            // 存储定时器
            _pollingTimers[device.DeviceId] = timer;
            
            _logger.LogInformation("设备 {DeviceId} 轮询已启动，间隔: {Interval}ms", 
                device.DeviceId, interval);
        }
        
        /// <summary>
        /// 轮询设备
        /// </summary>
        private async Task PollDeviceAsync(DeviceContext deviceContext)
        {
            if (deviceContext.ConnectionId == IntPtr.Zero)
            {
                _logger.LogInformation("设备 {DeviceId} 已断开连接，跳过轮询", deviceContext.DeviceId);
                return;
            }
            
            _logger.LogInformation("轮询设备: {DeviceId}", deviceContext.DeviceId);
            
            try
            {
                // 获取协议处理器
                var processor = _protocolRegistry.GetProcessor(deviceContext.ProtocolType);
                if (processor != null)
                {
                    // 构建并发送命令
                    var command = await processor.BuildCommandAsync(deviceContext);
                    if (command != null && command.Length > 0)
                    {
                        bool success = await deviceContext.SendAsync(command);
                        if (success)
                        {
                            _logger.LogInformation("成功向设备 {DeviceId} 发送命令", deviceContext.DeviceId);
                        }
                        else
                        {
                            _logger.LogWarning("向设备 {DeviceId} 发送命令失败", deviceContext.DeviceId);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("未找到设备 {DeviceId} 的协议处理器: {ProtocolType}", 
                        deviceContext.DeviceId, deviceContext.ProtocolType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "轮询设备 {DeviceId} 时出错", deviceContext.DeviceId);
            }
        }
        
        /// <summary>
        /// 后台服务执行
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("设备调度服务已启动");
            
            try
            {
                // 定期清理断开连接的设备
                while (!stoppingToken.IsCancellationRequested)
                {
                    CleanupDisconnectedDevices();
                    await Task.Delay(30000, stoppingToken); // 每30秒检查一次
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消，不需要处理
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "设备调度服务执行异常");
            }
            
            _logger.LogInformation("设备调度服务已停止");
        }
        
        /// <summary>
        /// 清理断开连接的设备
        /// </summary>
        private void CleanupDisconnectedDevices()
        {
            // 获取断开连接超过5分钟的设备
            var disconnectedDevices = _devices.Values
                .Where(d => d.ConnectionId == IntPtr.Zero && 
                       (DateTime.Now - d.LastActiveTime).TotalMinutes > 5)
                .ToList();
            
            foreach (var device in disconnectedDevices)
            {
                // 停止轮询
                if (_pollingTimers.TryRemove(device.DeviceId, out var timer))
                {
                    timer.Dispose();
                }
                
                // 移除设备
                if (_devices.TryRemove(device.DeviceId, out _))
                {
                    _logger.LogInformation("已清理断开连接的设备: {DeviceId}", device.DeviceId);
                }
            }
            
            _logger.LogInformation("已清理 {Count} 个断开连接的设备", disconnectedDevices.Count);
        }
        
        /// <summary>
        /// 停止服务
        /// </summary>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("设备调度服务正在停止");
            
            // 停止所有轮询定时器
            foreach (var timer in _pollingTimers.Values)
            {
                timer.Dispose();
            }
            
            _pollingTimers.Clear();
            
            return base.StopAsync(cancellationToken);
        }
    }
}
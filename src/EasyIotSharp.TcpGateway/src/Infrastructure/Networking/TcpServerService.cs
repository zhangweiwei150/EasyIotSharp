using HPSocket.Sdk;
using HPSocket.Tcp;
using HPSocket;
using HPSocket.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using System.Diagnostics.Metrics;
using Prometheus;
using ThreadPool = HPSocket.Thread.ThreadPool;
using HPSocket.Base;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Services;
using EasyIotSharp.Core.Services.Project;
using EasyIotSharp.Core.Services.Project.Impl;
using EasyIotSharp.Core.Dto.Project.Params;
namespace EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking
{
    /// <summary>
    /// TCP服务器服务，负责处理TCP连接和数据传输
    /// 实现为后台服务，可以在应用程序生命周期内持续运行
    /// </summary>
    public sealed class TcpServerService : BackgroundService
    {
        // TCP服务器实例
        private readonly ITcpServer _server;
        // 日志记录器
        private readonly ILogger<TcpServerService> _logger;
        // 服务器配置选项
        private readonly TcpServerOptions _options;
        // 协议处理器，用于处理接收到的数据
        private readonly IProtocolProcessor _processor;
        private readonly DeviceSchedulerService _deviceScheduler;
        
        // Prometheus指标：接收数据包计数器
        private static readonly Counter _receivedCounter = Metrics
            .CreateCounter("iot_tcp_received_total", "Total received packets");
        // Prometheus指标：当前连接数量
        private static readonly Gauge _connectionsGauge = Metrics
            .CreateGauge("iot_tcp_connections", "Current active connections");
        
        // 配置变更监听器
        private IDisposable? _optionsChangeToken;
        
        // 修改为使用HPSocket.Thread.ThreadPool
        private readonly ThreadPool? _threadPool;
        /// <summary>
        /// 线程池回调函数
        /// </summary>
        private TaskProcEx _taskTaskProc;

        // 服务运行状态标志
        public bool IsRunning { get; internal set; } = true;
        private readonly IGatewayProtocolService _gatewayProtocolService;
        /// <summary>
        /// 构造函数，初始化TCP服务器及其依赖项
        /// </summary>
        public TcpServerService(
            IGatewayProtocolService gatewayProtocolService,
            IOptionsSnapshot<TcpServerOptions> options,
            ILogger<TcpServerService> logger,
            IProtocolProcessor processor,
            DeviceSchedulerService deviceScheduler)  // 添加设备调度器
        {
            _gatewayProtocolService = gatewayProtocolService;
            _options = options.Value;
            _logger = logger;
            _processor = processor;
            _deviceScheduler = deviceScheduler;
            _server = new TcpServer();
            
            // 根据配置初始化线程池
            if (_options.UseThreadPool)
            {
                _threadPool = new ThreadPool();
            }
            
            // 配置服务器参数
            ConfigureServer();
        }

        /// <summary>
        /// 配置TCP服务器参数和事件处理
        /// </summary>
        private void ConfigureServer()
        {
            // 设置基本参数
            _server.Port = (ushort)_options.Port;
            _server.SocketBufferSize = (uint)_options.BufferSize;
            _server.MaxConnectionCount = (uint)_options.MaxConnections;
            
            // 配置线程池参数（如果启用）
            if (_threadPool != null)
            {
                // 启动线程池，设置线程数和拒绝策略
                if (!_threadPool.Start((int)_options.MaxThreadCount, RejectedPolicy.WaitFor))
                {
                    _logger.LogError("线程池启动失败，错误码: {ErrorCode}", _threadPool.SysErrorCode);
                }
                else
                {
                    _logger.LogInformation("线程池已启动，线程数: {ThreadCount}", _options.MaxThreadCount);
                }
            }

            // 注册事件处理器
            _server.OnPrepareListen += OnPrepareListen;
            _server.OnAccept += OnAccept;
            _server.OnReceive += OnReceive;
            _server.OnClose += OnClose;
            _server.OnShutdown += OnShutdown;
            
            // 线程池回调函数
            _taskTaskProc = TaskProc;
            
            // 优化：设置保活选项，防止连接异常断开
            //_server.KeepAliveTime = 0; // 30秒
            //_server.KeepAliveInterval = 0; // 10秒
        }

        /// <summary>
        /// 服务器准备监听时的回调
        /// </summary>
        private HandleResult OnPrepareListen(IServer sender, IntPtr listen)
        {
            _logger.LogInformation("TCP服务器准备在端口 {Port} 上启动", _options.Port);
            return HandleResult.Ok;
        }

        /// <summary>
        /// 服务器关闭时的回调
        /// </summary>
        private HandleResult OnShutdown(IServer sender)
        {
            _logger.LogInformation("TCP服务器已关闭，地址: {Address}，端口: {Port}", sender.Address, sender.Port);
            return HandleResult.Ok;
        }

        /// <summary>
        /// 后台服务执行方法
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IsRunning = true;
            
            // 优化：取消注释以启用配置变更监听
            //_optionsChangeToken = _options.OnChange(ReloadConfiguration);
            
            // 启动服务器
            if (!_server.Start())
            {
                _logger.LogCritical("TCP服务器启动失败。错误代码: {Code}", _server.ErrorCode);
                return;
            }

            _logger.LogInformation("TCP服务器已在端口 {Port} 上启动", _options.Port);

            // 主循环：定期检查并断开空闲连接
            while (!stoppingToken.IsCancellationRequested)
            {                 
                await Task.Delay(1000, stoppingToken);
                // 断开超时未活动的连接
                _server.DisconnectSilenceConnections((uint)_options.SilenceTimeout);
            }

            // 停止服务器
            await StopServerAsync();
        }
        
        /// <summary>
        /// 重新加载配置并重启服务器
        /// </summary>
        private void ReloadConfiguration(TcpServerOptions newOptions)
        {
            _logger.LogInformation("正在重新加载TCP服务器配置");
            _ = StopServerAsync().ContinueWith(_ => StartServerAsync());
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        private async Task StartServerAsync()
        {
            // 优化：添加重试逻辑
            int retryCount = 0;
            const int maxRetries = 3;
            
            while (retryCount < maxRetries)
            {
                if (_server.Start())
                {
                    IsRunning = true;
                    _logger.LogInformation("TCP服务器已使用新配置重新启动");
                    return;
                }
                
                retryCount++;
                _logger.LogWarning("TCP服务器重启失败，尝试重试 ({Current}/{Max})", retryCount, maxRetries);
                await Task.Delay(1000 * retryCount); // 指数退避
            }
            
            _logger.LogCritical("TCP服务器重启失败，已达到最大重试次数");
        }
        
        /// <summary>
        /// 停止服务器
        /// </summary>
        private async Task StopServerAsync()
        {
            IsRunning = false;
            _server.Stop();
            
            // 停止线程池
            if (_threadPool != null && _threadPool.HasStarted)
            {
                await _threadPool.StopAsync();
                _logger.LogInformation("线程池已停止");
            }
            
            await _server.WaitAsync();
            _logger.LogInformation("TCP服务器已停止");
        }

        /// <summary>
        /// 处理新连接接受事件
        /// </summary>
        private HandleResult OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            _connectionsGauge.Inc();
            
            if (!sender.GetRemoteAddress(connId, out var ipStr, out var port))
                return HandleResult.Error;

            // 创建 DeviceContext 时传入 Server 实例
            var context = new DeviceContext
            {
                ConnectionId = connId,
                LastActiveTime = DateTime.UtcNow,
                Server = sender  // 添加这行
            };

            sender.SetExtra(connId, context);
            _logger.LogInformation("新连接建立: {ConnectionId}, IP: {IP}, 端口: {Port}", 
                context.ConnectionId, ipStr, port);
            
            return HandleResult.Ok;
        }
 

        /// <summary>
        /// 处理数据接收事件
        /// </summary>
        private HandleResult OnReceive(IServer sender, IntPtr connId, byte[] data)
        {
            var ls= _gatewayProtocolService.QueryGatewayProtocol(new QueryGatewayProtocolInput { IsPage=false,GatewayId=""});
            // 增加接收计数
            _receivedCounter.Inc();
            
            // 获取连接上下文
            var context = sender.GetExtra<DeviceContext>(connId);
            if (context == null) return HandleResult.Error;

            // 更新最后活动时间
            context.LastActiveTime = DateTime.UtcNow;
            HandleResult result;
            // 异步处理数据
            result = ProcessDataAsync(sender,context, data);

            return result;
        }

        /// <summary>
        /// 异步处理接收到的数据
        /// </summary>
        private HandleResult ProcessDataAsync(IServer sender, DeviceContext context, byte[] data)
        {
            var result = HandleResult.Ok;
            try
            {
                // 优化：添加数据大小日志
                _logger.LogInformation("从 {ConnectionId} 接收到 {Size} 字节数据", 
                    context.ConnectionId, data.Length);
                // 这里来的都是完整的包, 但是这里不做耗时操作, 仅把数据放入队列
                // 向线程池提交任务
                var packet = new Packet()
                {
                    BData = data
                };
                if (!_threadPool.Submit(_taskTaskProc, new TaskInfo
                {
                    Client = context,
                    Server = sender,
                    Packet = packet,
                }))
                {
                    result = HandleResult.Error;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理来自 {ConnectionId} 的数据时出错", context.ConnectionId);
                result = HandleResult.Error;
                return result;
            }
        }

        /// <summary>
        /// 线程池任务处理函数 - 修改为接收单个对象参数
        /// </summary>
        private void TaskProc(object obj)
        {
            try
            {
                if (!(obj is TaskInfo taskInfo))
                {
                    _logger.LogError("线程池处理数据时出错：无效的任务信息类型");
                    return;
                }
                var deviceContext = taskInfo.Client as DeviceContext;
                if (deviceContext == null)
                {
                    _logger.LogError("无效的设备上下文类型");
                    return;
                }

                // 处理数据
                _processor.ProcessDataAsync(taskInfo.Packet.BData, deviceContext).GetAwaiter().GetResult();

                // 如果设备已注册且配置有效，启动轮询
                if (!string.IsNullOrEmpty(deviceContext.DeviceId) && deviceContext.Config != null)
                {
                    // 注册设备到调度器并启动轮询
                    _deviceScheduler.RegisterDevice(deviceContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "线程池处理数据时出错");
            }
        }

        /// <summary>
        /// 处理连接关闭事件
        /// </summary>
        private HandleResult OnClose(IServer sender, IntPtr connId, SocketOperation socketOperation, int errorCode)
        {
            _connectionsGauge.Dec();
            
            var context = sender.GetExtra<DeviceContext>(connId);
            if (context == null) return HandleResult.Ok;

            sender.RemoveExtra(connId);
            
            string reason = socketOperation == SocketOperation.Close ? "正常关闭" : 
                $"异常关闭: {socketOperation}, 错误码: {errorCode}, 最后活动时间: {context.LastActiveTime}";
            _logger.LogWarning("连接断开: {ConnectionId}, 原因: {Reason}, 持续时间: {Duration}秒", 
                context.ConnectionId, 
                reason,
                (DateTime.UtcNow - context.LastActiveTime).TotalSeconds);
            
            return HandleResult.Ok;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            // 释放配置变更监听器
            _optionsChangeToken?.Dispose();
            // 释放线程池
            _threadPool?.Dispose();
            // 释放服务器
            _server?.Dispose();
            // 调用基类释放方法
            base.Dispose();
        }
    }
}

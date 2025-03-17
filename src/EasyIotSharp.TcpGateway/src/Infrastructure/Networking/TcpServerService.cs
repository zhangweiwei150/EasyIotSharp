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

namespace EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking
{
    public sealed class TcpServerService : BackgroundService
    {
        private readonly ITcpServer _server;
        private readonly ILogger<TcpServerService> _logger;
        private readonly TcpServerOptions _options;
        private readonly IProtocolProcessor _processor;
        private static readonly Counter _receivedCounter = Metrics
      .CreateCounter("iot_tcp_received_total", "Total received packets");

        private static readonly Gauge _connectionsGauge = Metrics
            .CreateGauge("iot_tcp_connections", "Current active connections");
        // 添加配置监听
        private IDisposable? _optionsChangeToken;

        public bool IsRunning { get; internal set; } = true;

        public TcpServerService(
          IOptionsSnapshot<TcpServerOptions> options, // 修改这里
            ILogger<TcpServerService> logger,
            IProtocolProcessor processor)
        {
            _options = options.Value;
            _logger = logger;
            _processor = processor;
            _server = new TcpServer();
            ConfigureServer();
        }

        private void ConfigureServer()
        {
            _server.Port = (ushort)_options.Port;
            _server.SocketBufferSize = (uint)_options.BufferSize;
            _server.MaxConnectionCount = (uint)_options.MaxConnections;

            _server.OnPrepareListen += OnPrepareListen;
            _server.OnAccept += OnAccept;
            _server.OnReceive += OnReceive;
            _server.OnClose += OnClose;
        }

        private HandleResult OnPrepareListen(IServer sender, IntPtr listen)
        {
            _logger.LogInformation("Starting TCP server on port {Port}", _options.Port);
            return HandleResult.Ok;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IsRunning = true;
            //_optionsChangeToken = _options.OnChange(ReloadConfiguration);
            if (!_server.Start())
            {
                _logger.LogCritical("Failed to start TCP server. ErrorCode: {Code}", _server.ErrorCode);
                return;
            }

            _logger.LogInformation("TCP server started on port {Port}", _options.Port);

            while (!stoppingToken.IsCancellationRequested)
            {                 
                await Task.Delay(1000, stoppingToken);
                _server.DisconnectSilenceConnections((uint)_options.SilenceTimeout);
            }

            await StopServerAsync();
        }
        private void ReloadConfiguration(TcpServerOptions newOptions)
        {
            _logger.LogInformation("Reloading TCP server configuration");
            _ = StopServerAsync().ContinueWith(_ => StartServerAsync());
        }

        private async Task StartServerAsync()
        {
            if (!_server.Start())
            {
                _logger.LogCritical("Failed to restart TCP server");
                return;
            }
            IsRunning = true;
            _logger.LogInformation("TCP server restarted with new configuration");
        }
        private async Task StopServerAsync()
        {
            IsRunning = false;
            _server.Stop();
            await _server.WaitAsync();
            _logger.LogInformation("TCP server stopped");
        }

        private HandleResult OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            // 在OnAccept事件中
            _connectionsGauge.Inc();
            if (!sender.GetRemoteAddress(connId, out var ipStr, out var port))
                return HandleResult.Error;

            var context = new ConnectionContext
            {
                ConnectionId = connId.ToString(),
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port)
            };

            sender.SetExtra(connId, context);
            _logger.LogInformation("New connection: {ConnectionId}", context.ConnectionId);
            return HandleResult.Ok;
        }

        private HandleResult OnReceive(IServer sender, IntPtr connId, byte[] data)
        {
            // 在OnReceive事件中
            _receivedCounter.Inc();
            var context = sender.GetExtra<ConnectionContext>(connId);
            if (context == null) return HandleResult.Error;

            context.LastActivity = DateTime.UtcNow;
            _ = ProcessDataAsync(context, data);
            return HandleResult.Ok;
        }

        private async Task ProcessDataAsync(ConnectionContext context, byte[] data)
        {
            try
            {
                await _processor.ProcessAsync(context, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data from {ConnectionId}", context.ConnectionId);
            }
        }

        private HandleResult OnClose(IServer sender, IntPtr connId, SocketOperation operation, int errorCode)
        {
            // 在OnClose事件中
            _connectionsGauge.Dec();
            var context = sender.GetExtra<ConnectionContext>(connId);
            if (context == null) return HandleResult.Ok;

            sender.RemoveExtra(connId);
            _logger.LogInformation("Connection closed: {ConnectionId}", context.ConnectionId);
            return HandleResult.Ok;
        }

        public override void Dispose()
        {
            _server?.Dispose();
            base.Dispose();
        }
    }
}

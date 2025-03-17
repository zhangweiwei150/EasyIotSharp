using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking
{
    /// <summary>
    /// 用于检测 TCP 服务健康状态的健康检查类
    /// </summary>
    public class TcpServerHealthCheck : IHealthCheck
    {
        private readonly TcpServerService _tcpServer;

        // 确保有构造函数用于依赖注入
        public TcpServerHealthCheck(TcpServerService tcpServer)
        {
            _tcpServer = tcpServer ?? throw new ArgumentNullException(nameof(tcpServer));
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(_tcpServer.IsRunning
                    ? HealthCheckResult.Healthy("TCP server is running")
                    : HealthCheckResult.Unhealthy("TCP server is stopped"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Health check failed", ex));
            }
        }
    }
}

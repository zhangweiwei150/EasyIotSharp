using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking
{
    /// <summary>
    /// TCP服务器健康检查
    /// </summary>
    public class TcpServerHealthCheck : IHealthCheck
    {
        private readonly TcpServerService _tcpServerService;
        private readonly ILogger<TcpServerHealthCheck> _logger;

        public TcpServerHealthCheck(
            TcpServerService tcpServerService,
            ILogger<TcpServerHealthCheck> logger)
        {
            _tcpServerService = tcpServerService;
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_tcpServerService.IsRunning)
                {
                    return Task.FromResult(HealthCheckResult.Healthy("TCP服务器运行正常"));
                }
                else
                {
                    return Task.FromResult(HealthCheckResult.Degraded("TCP服务器未运行"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TCP服务器健康检查失败");
                return Task.FromResult(HealthCheckResult.Unhealthy("TCP服务器健康检查异常", ex));
            }
        }
    }
}

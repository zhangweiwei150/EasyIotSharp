// Program.cs
using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking;
using EasyIotSharp.Cloud.TcpGateway.src.ProtocolServices.Modbus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// 日志配置
builder.Logging.AddConsole()
       .AddDebug()
       .AddJsonConsole()
       .SetMinimumLevel(LogLevel.Information);

// 添加以下配置
//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// TCP服务配置
builder.Services.AddOptions<TcpServerOptions>()
    .BindConfiguration("TcpServer") // .NET 8新语法
    .ValidateDataAnnotations();

// 服务注册
builder.Services.AddSingleton<IProtocolProcessor, ModbusProcessor>();
builder.Services.AddHostedService<TcpServerService>();
builder.Services.AddSingleton<TcpServerService>();
// 健康检查
builder.Services.AddHealthChecks()
    .AddCheck<TcpServerHealthCheck>("tcp_server");

var host = builder.Build();
host.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status200OK // 仅用于调试
    }
});

// Prometheus配置
host.UseMetricServer("/metrics");
host.UseHttpMetrics();

await host.RunAsync();
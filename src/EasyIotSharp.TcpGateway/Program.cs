// Program.cs
using EasyIotSharp.Cloud.TcpGateway;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions;
using EasyIotSharp.Cloud.TcpGateway.src.Core.Services;
using EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking;
using EasyIotSharp.Cloud.TcpGateway.src.ProtocolServices.Modbus;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Services.Project;
using EasyIotSharp.Core.Services.Project.Impl;
using HPSocket;
using HPSocket.Tcp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Prometheus;
using System.Text;
using UPrime;
using UPrime.Configuration;
using Castle.Facilities.Logging;

using EasyIotSharp.Core.Extensions;
using UPrime.Castle.Log4Net;


var builder = WebApplication.CreateBuilder(args);

// 日志配置
builder.Logging.ClearProviders()  // 先清除默认提供程序
    .AddConsole(options =>
    {
        options.FormatterName = "simple";
        // 设置控制台输出编码为 UTF-8 并启用 BOM
        Console.OutputEncoding = new UTF8Encoding(true);
    })
    .AddDebug()
    .AddJsonConsole(options =>
    {
        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = true
        };
        options.UseUtcTimestamp = true;
    })
    .SetMinimumLevel(LogLevel.Information);

// TCP服务配置
builder.Services.AddOptions<TcpServerOptions>()
    .BindConfiguration("TcpServer") // .NET 8新语法
    .ValidateDataAnnotations();


var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true)
                  .AddYamlFile($"appsettings.dev.yml", optional: true, reloadOnChange: true)
                  .AddCommandLine(args)
                  .Build();
// 服务注册
var appOptions = AppOptions.ReadFromConfiguration(config);
UPrimeStarter.Create<EasyIotSharpTcpGatwayModule>((options) =>
{
    options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseUpLog4Net().WithConfig("log4net.config"));
    options.IocManager.AddAppOptions(appOptions);
}).Initialize();
// Add services to the container
builder.Services.AddSingleton<IDeviceConfigService, DeviceConfigService>();
builder.Services.AddSingleton<IProtocolRegistry, ProtocolRegistry>();
builder.Services.AddSingleton<IProtocolProcessor, ModbusProcessor>();
builder.Services.AddSingleton<IProtocolProcessor, ModbusRtuProcessor>();
builder.Services.AddSingleton<IProtocolIdentifier, ModbusRtuIdentifier>();


builder.Services.AddSingleton<IGatewayProtocolService, GatewayProtocolService>();
builder.Services.AddSingleton<ITcpServer>(sp => 
{
    var server = new TcpServer();
    return server;
});

builder.Services.AddSingleton<DeviceSchedulerService>();
builder.Services.AddSingleton<TcpServerService>();

// 注册TCP服务
builder.Services.AddHostedService(sp => sp.GetRequiredService<TcpServerService>());

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
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Middlewares
{
    public static class SerilogExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, configuration) =>
            {
                configuration
                    .WriteTo.Console() // 控制台输出
                    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // 日志写入文件
                    .Enrich.FromLogContext(); // 增强日志上下文
            });
        }
    }
}

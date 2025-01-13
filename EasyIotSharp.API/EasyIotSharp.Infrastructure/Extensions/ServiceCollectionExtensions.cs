using EasyIotSharp.Domain.Repositories;
using EasyIotSharp.Infrastructure.Database;
using EasyIotSharp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, string connectionString)
        {
            // 注册 SqlSugarDbContext 到 DI 容器
            services.AddSingleton<SqlSugarDbContext>();
            // 注册通用仓储
            services.AddScoped(typeof(IRepository<>), typeof(SqlSugarRepository<>));

            return services;
        }
    }
}

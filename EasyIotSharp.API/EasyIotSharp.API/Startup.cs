using EasyIotSharp.Infrastructure.Database;
using EasyIotSharp.Infrastructure.Extensions;
using EasyIotSharp.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyIotSharp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // 加载 AppSettings 配置到静态类 Config（你的 Config 类中应该有类似的逻辑）
            Config.AppSettings = Configuration.GetSection("AppSettings").Get<Dictionary<string, string>>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册跨域支持
            services.AddCustomCors();
            // 注册 Swagger
            services.AddCustomSwagger();
            // 获取连接字符串
            string dbConnectionString = Config.AppSettings["ConnectionStrings"];
            // 注册 SqlSugarDbContext
            services.AddSqlSugar(dbConnectionString);
            // 注入 CodeFirstInitializer
            services.AddSingleton<CodeFirstInitializer>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CodeFirstInitializer initializer)
        {    // 初始化数据库表（调用封装好的方法）
            initializer.InitializeTables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 使用全局异常处理中间件
                app.UseMiddleware<ExceptionMiddleware>();
            }
            // 启用 CORS
            app.UseCors("AllowAllOrigins");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using UPrime;
using EasyIotSharp.API.Admin.Filters;
using EasyIotSharp.API.Admin.Middleware;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.API.Admin
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var appOptions = UPrimeEngine.Instance.Resolve<AppOptions>();
            services.AddCors(options =>
            {
                options.AddPolicy($"{appOptions.Name}-policy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .WithMethods("POST", "GET", "OPTIONS");
                    });
            });
            services.AddHttpClient();
            services.AddMvc(options =>
            {
                //filters
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            //swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{appOptions.Name}：XXX项目Admin服务"
                });

                var basePath = Directory.GetCurrentDirectory();
                options.IncludeXmlComments(Path.Combine(basePath, "EasyIotSharp.Core.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "EasyIotSharp.API.Admin.xml"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var appOptions = UPrimeEngine.Instance.Resolve<AppOptions>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors($"{appOptions.Name}-policy");
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/static",
                EnableDirectoryBrowsing = false
            });
            app.UseRouting();
            //使用全局捕捉异常记录异常日志
            app.UseEagleException();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", appOptions.Name);
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(-1);
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
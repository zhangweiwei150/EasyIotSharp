using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UPrime;
using UPrime.WebApi;
using EasyIotSharp.Core;

namespace EasyIotSharp.API.Admin.Middleware
{
    /// <summary>
    /// 全局错误的中间件
    /// </summary>
    public class EagleExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private IEagleLogService _eagleLogService;

        public EagleExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            //_eagleLogService = UPrimeEngine.Instance.Resolve<IEagleLogService>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                #region EagleLog

                //string code = "";
                //string body = "";
                //string errType = EagleType.LOG_FATAL;
                //string errMsg = e.Message;
                //string stackTrace = JsonConvert.SerializeObject(e.StackTrace ?? e.StackTrace.ToString());

                ////验证是否自定义BizException
                //if (e is BizException bizException)
                //{
                //    if (bizException.CommonError.IsNotNull())
                //    {
                //        code = bizException.CommonError.Code;
                //        errMsg = bizException.CommonError.Message;
                //        errType = EagleType.LOG_ERROR;
                //    }
                //}

                ////设定从Body流起始位置开始，读取整个Htttp请求的Body数据
                //context.Request.Body.Position = 0;

                //using (var mem = new MemoryStream())
                //using (var reader = new StreamReader(mem))
                //{
                //    await context.Request.Body.CopyToAsync(mem);
                //    mem.Seek(0, SeekOrigin.Begin);
                //    body = await reader.ReadToEndAsync();
                //}

                ////读取到Body后，重新设置Stream到起始位置，方便后面的Filter或Middleware使用Body的数据
                //context.Request.Body.Position = 0;

                ////记录异常日志
                //try
                //{
                //    _eagleLogService.LogExceptionAsync(errType, errMsg, context.Request.Path, stackTrace, body);
                //}
                //catch (Exception ex)
                //{
                //    var logger = UPrime.UPrimeEngine.Instance.Resolve<ILogger>();
                //    logger.Error(JsonConvert.SerializeObject(ex.Message));
                //}
                //finally
                //{
                //}

                #endregion EagleLog

                #region Response

                //var serializerSettings = new JsonSerializerSettings
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //};
                //var content = JsonConvert.SerializeObject(new UPrimeResponse()
                //{
                //    Code = code,
                //    FullMessage = "",
                //    Message = errMsg,
                //    Timestamp = DateTime.Now
                //}, Formatting.None, serializerSettings);
                //context.Response.StatusCode = (int)HttpStatusCode.OK;
                //context.Response.ContentType = "application/json";
                //await context.Response.WriteAsync(content);

                #endregion Response
            }
        }
    }
}
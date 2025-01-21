using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;

namespace EasyIotSharp.API
{
    public static partial class Extensions
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpContext context)
        {
            if (context == null || (context != null && context.Request == null))
                return string.Empty;

            var ip = context.Request.Headers["x-forwarded-for"].FirstOrDefault();

            if (ip.IsNullOrEmpty())
            {
                ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            ip = ip.Replace("，", ",");
            if (ip.Contains(","))
            {
                var ipItems = ip.Split(",");
                if (ipItems.Length > 0)
                {
                    ip = ipItems[0];
                }
            }
            return ip;
        }

        /// <summary>
        /// 获取全路径Url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            var proto = request.Headers["x-forwarded-proto"].ToString();
            if (proto.IsNullOrEmpty())
            {
                proto = request.Scheme;
            }

            return new StringBuilder()
                .Append(proto)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
    }
}
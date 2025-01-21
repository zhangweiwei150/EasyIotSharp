using Microsoft.AspNetCore.Builder;

namespace EasyIotSharp.API.Admin.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseEagleException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EagleExceptionMiddleware>();
        }
    }
}
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using Trivial.Security;
using EasyIotSharp.Core;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.API.Filters
{
    /// <summary>
    /// 拦截器(验证用户token信息)
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headersIsExistsToken = context.HttpContext.Request.Headers.TryGetValue("u-token", out StringValues bearerToken);
            if (headersIsExistsToken)
            {
                RSASignatureProvider sign;
                sign = RSASignatureProvider.CreateRS256(JWTTokenOptions.PublicKey);
                var isExpired = false;
                var userId = string.Empty;
                if (sign.IsNotNull())
                {
                    var token = bearerToken.ToString().ReplaceByEmpty("Bearer")
                                                      .ReplaceByEmpty("bearer").Trim();

                    if (token.IsNotNullOrEmpty())
                    {
                        //try
                        //{
                            var json = JsonWebToken<string>.Parse(token.ToString(), sign);
                            var parsedObj = json.DeserializeFromJson<JWT_User>();
                            var nowTimeStamp = ConvertToTimeStamp(DateTime.Now);
                            var nowTimeStamp_10 = Convert.ToInt64(nowTimeStamp.ToString().Substring(0, 10));
                            isExpired = parsedObj.ExpireTime <= nowTimeStamp_10;
                            userId = parsedObj.UserId;
                        //}
                        //catch (Exception ex)
                        //{
                        //    //换证书兼容PV4Y，PV4Y只验证了40003跳出登录
                        //    throw new BizException(BizError.TOKEN_EXCEPTION);
                        //}
                    }
                    else
                    {
                        throw new BizException(BizError.TOKEN_NULLOREMPTY);
                    }

                    if (!isExpired)
                    {
                        context.HttpContext.User = new UserTokenPrincipal(new UserTokenIdentity(new UserTokenData { UserId = userId }));
                    }
                    else
                    {
                        throw new BizException(BizError.TOKEN_EXPIRED);
                    }
                }
            }
            else
            {
                throw new BizException(BizError.TOKEN_NULLOREMPTY);
            }

            base.OnActionExecuting(context);
        }

        private long ConvertToTimeStamp(DateTime time)
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(time.AddHours(-8) - Jan1st1970).TotalMilliseconds;
        }
    }
}
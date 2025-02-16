using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using Trivial.Security;
using EasyIotSharp.Core;
using EasyIotSharp.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Security.Cryptography;

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
                var publicKey = JWTTokenOptions.PublicKey;
                // 去掉 PEM 文件的头尾
                var base64Key = publicKey
                    .Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "") // 去掉换行符
                    .Replace("\r", "") // 去掉回车符
                    .Trim();
                var rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(base64Key), out _);

                // 配置 Token 验证参数
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateIssuer = false, // 如果需要验证 Issuer，请设置为 true 并提供 Issuer
                    ValidateAudience = false, // 如果需要验证 Audience，请设置为 true 并提供 Audience
                    ValidateLifetime = true, // 验证 Token 是否过期
                    ClockSkew = TimeSpan.Zero // 不允许时间偏差
                };
                // 解析和验证 Token
                var tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal;
                var token = bearerToken.ToString().ReplaceByEmpty("Bearer")
                                                  .ReplaceByEmpty("bearer").Trim();

                if (token.IsNotNullOrEmpty())
                {
                    try
                    {
                        principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                        // 提取用户信息
                        var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");
                        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                        {
                            throw new BizException(BizError.TOKEN_INVALID_USER_CLAIMS);
                        }

                        var userId = userIdClaim.Value;
                        // 设置用户身份信息
                        context.HttpContext.User = new UserTokenPrincipal(new UserTokenIdentity(new UserTokenData { UserId = userId }));
                    }
                    catch (SecurityTokenExpiredException)
                    {
                        throw new BizException(BizError.TOKEN_EXPIRED);
                    }
                    catch (SecurityTokenInvalidSignatureException)
                    {
                        throw new BizException(BizError.TOKEN_SIGNATURE);
                    }
                    catch (Exception ex)
                    {
                        throw new BizException(BizError.TOKEN_EXCEPTION);
                    }

                    
                }
                else
                {
                    throw new BizException(BizError.TOKEN_NULLOREMPTY);
                }
            }
            else
            {
                throw new BizException(BizError.TOKEN_NULLOREMPTY);
            }

            base.OnActionExecuting(context);
        }
    }
}
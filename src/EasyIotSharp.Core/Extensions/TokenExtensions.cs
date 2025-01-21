using System.Security.Principal;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Core.Extensions
{
    public static class TokenExtensions
    {
        /// <summary>
        /// 获取Token用户身份对象
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static UserTokenData GetUserTokenIdentifier(this IIdentity identity)
        {
            return (identity as UserTokenIdentity)?.GetIdentifier();
        }
    }
}
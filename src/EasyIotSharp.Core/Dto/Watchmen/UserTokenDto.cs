using System;

namespace EasyIotSharp.Core.Dto.Users
{
    /// <summary>
    /// 用户Token令牌DTO
    /// </summary>
    public class UserTokenDto
    {
        /// <summary>
        /// Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }
    }
}
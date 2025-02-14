using Newtonsoft.Json;
using System.IO;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// JWTToken验证配置
    /// </summary>
    public static class JWTTokenOptions
    {
        static JWTTokenOptions()
        {
            PublicKey = File.ReadAllText("Config/token_public_key.pem");
            PrivateKey = File.ReadAllText("Config/token_private_key.pem");
        }

        /// <summary>
        /// RS256公钥
        /// </summary>
        public static string PublicKey { get; set; }

        /// <summary>
        /// RS256私钥
        /// </summary>
        public static string PrivateKey { get; set; }
    }

    /// <summary>
    /// JWT Token解析后的用户对象
    /// </summary>
    public class JWT_User
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("uid")]
        public string UserId { get; set; }

        /// <summary>
        /// 签发时间
        /// </summary>
        [JsonProperty("iat")]
        public long IssuedAt { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonProperty("exp")]
        public long ExpireTime { get; set; }
    }
}
using Newtonsoft.Json;

namespace EasyIotSharp.Core.Dto.Watchmen.Params
{
    /// <summary>
    /// 获取用户Token入参
    /// </summary>
    public class GetUserTokenInput
    {
        /// <summary>
        /// 小程序唯一凭证 appId
        /// </summary>
        [JsonProperty("appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 小程序唯一凭证密钥 appSecret
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [JsonProperty("uid")]
        public string UId { get; set; }
    }
}
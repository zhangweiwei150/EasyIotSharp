using Newtonsoft.Json;

namespace EasyIotSharp.Core.Dto.Watchmen.Params
{
    /// <summary>
    /// 获取用户Token返回参数
    /// </summary>
    public class GetUserTokenOutput
    {
        /// <summary>
        /// 请求状态码
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 获取到的凭证
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 凭证有效时间
        /// </summary>
        [JsonProperty("expired_in")]
        public int ExpiredIn { get; set; }
    }
}
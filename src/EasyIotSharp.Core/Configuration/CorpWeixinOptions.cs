using Microsoft.Extensions.Configuration;
using UPrime.SDK.Weixin.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 企业微信配置
    /// </summary>
    public class CorpWeixinOptions : ICorpWeixinSettings
    {
        public CorpWeixinOptions()
        {
        }

        public CorpWeixinOptions(AppOptions appOptions)
        {
            CorpId = appOptions.CorpWeixinOptions.CorpId;
            AuthAgentId = appOptions.CorpWeixinOptions.AuthAgentId;
            AuthSecret = appOptions.CorpWeixinOptions.AuthSecret;
        }

        /// <summary>
        /// 企业微信ID
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        ///代理商授权ID
        /// </summary>
        public string AuthAgentId { get; set; }

        /// <summary>
        ///密钥
        /// </summary>
        public string AuthSecret { get; set; }

        /// <summary>
        /// 初始化读取
        /// </summary>
        public static CorpWeixinOptions ReadFromConfiguration(IConfiguration config)
        {
            CorpWeixinOptions options = new CorpWeixinOptions();
            var cs = config.GetSection("CorpWeixin");
            options.CorpId = cs.GetValue<string>(nameof(CorpId));
            options.AuthAgentId = cs.GetValue<string>(nameof(AuthAgentId));
            options.AuthSecret = cs.GetValue<string>(nameof(AuthSecret));

            return options;
        }
    }
}
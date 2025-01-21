using Microsoft.Extensions.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 【watchmen】token配置项
    /// </summary>
    public class WatchmenOptions
    {
        /// <summary>
        /// API地址
        /// </summary>
        public string APIBaseUrl { get; internal set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppId { get; internal set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string Secret { get; internal set; }

        /// <summary>
        /// 授权模式
        /// </summary>
        public string GrantType { get; internal set; }

        public static WatchmenOptions ReadFromConfiguration(IConfiguration config)
        {
            WatchmenOptions options = new WatchmenOptions();
            var cs = config.GetSection("Watchmen");
            options.APIBaseUrl = cs.GetValue<string>(nameof(APIBaseUrl));
            options.AppId = cs.GetValue<string>(nameof(AppId));
            options.Secret = cs.GetValue<string>(nameof(Secret));
            options.GrantType = cs.GetValue<string>(nameof(GrantType));
            return options;
        }
    }
}
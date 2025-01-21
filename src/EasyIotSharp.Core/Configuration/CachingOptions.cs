using Microsoft.Extensions.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 应用缓存配置
    /// </summary>
    public class CachingOptions
    {
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 是否开启Redis cache true = redis false = 内存
        /// </summary>
        public bool RedisEnabled { get; set; }

        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnStr { get; set; }

        /// <summary>
        /// 是否开启 twemproxy 分片代理
        /// </summary>
        public bool RedisTwemProxyEnabled { get; set; }

        /// <summary>
        /// 数据库Id
        /// </summary>
        public int RedisDatabaseId { get; set; }

        public static CachingOptions ReadFromConfiguration(IConfiguration config)
        {
            CachingOptions options = new CachingOptions();
            var cs = config.GetSection("Caching");

            options.IsOpen = cs.GetValue<bool>(nameof(IsOpen));
            options.RedisEnabled = cs.GetValue<bool>(nameof(RedisEnabled));
            options.RedisConnStr = cs.GetValue<string>(nameof(RedisConnStr));
            options.RedisTwemProxyEnabled = cs.GetValue<bool>(nameof(RedisTwemProxyEnabled));
            options.RedisDatabaseId = cs.GetValue<int>(nameof(RedisDatabaseId));

            return options;
        }
    }
}
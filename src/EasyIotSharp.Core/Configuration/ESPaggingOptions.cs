using Microsoft.Extensions.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// es分页相关配置
    /// </summary>
    public class ESPaggingOptions
    {
        /// <summary>
        /// 单次读取的最大数量
        /// </summary>
        public int MaxReadCount { get; set; }

        /// <summary>
        /// 单次写入的最大数量
        /// </summary>
        public int MaxWriteCount { get; set; }

        public static ESPaggingOptions ReadFromConfiguration(IConfiguration config)
        {
            ESPaggingOptions options = new ESPaggingOptions();
            var cs = config.GetSection("ESPagging");

            options.MaxReadCount = cs.GetValue<int>(nameof(MaxReadCount));
            options.MaxWriteCount = cs.GetValue<int>(nameof(MaxWriteCount));

            return options;
        }
    }
}
using Microsoft.Extensions.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 依赖的外部服务配置项
    /// </summary>
    public class APIServiceOptions
    {

        /// <summary>
        /// datalib url
        /// </summary>
        public string BasicLib { get; set; }

        public static APIServiceOptions ReadFromConfiguration(IConfiguration config)
        {
            APIServiceOptions options = new APIServiceOptions();
            var cs = config.GetSection("APIService");
            options.BasicLib = cs.GetValue<string>(nameof(BasicLib));
            return options;
        }
    }
}
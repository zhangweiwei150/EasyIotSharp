using Microsoft.Extensions.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    public class MinIOOptions
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Servers { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 文件存储的桶名称
        /// </summary>
        public string BucketName { get; set; }

        public static MinIOOptions ReadFromConfiguration(IConfiguration config)
        {
            MinIOOptions options = new MinIOOptions();
            var cs = config.GetSection("MinIO");

            options.Servers = cs.GetValue<string>(nameof(Servers));
            options.AccessKey = cs.GetValue<string>(nameof(AccessKey));
            options.SecretKey = cs.GetValue<string>(nameof(SecretKey));
            options.BucketName = cs.GetValue<string>(nameof(BucketName));

            return options;
        }
    }
}

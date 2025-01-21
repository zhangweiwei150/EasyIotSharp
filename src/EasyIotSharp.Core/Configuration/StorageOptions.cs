using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 应用的数据库存储配置
    /// </summary>
    public class StorageOptions
    {
        /// <summary>
        /// elasticsearch 连接配置
        /// ["node1:9200", "node2:9200", .. ]
        /// </summary>
        public IList<string> Elasticsearch { get; internal set; }

        public IList<MysqlServerAddress> MysqlServers { get; internal set; }

        public string MysqlConnectionMode { get; internal set; }

        public string MysqlDbName { get; internal set; }

        public string MysqlUsername { get; internal set; }

        public string MysqlPassword { get; internal set; }

        public static StorageOptions ReadFromConfiguration(IConfiguration config)
        {
            StorageOptions options = new StorageOptions();
            var cs = config.GetSection("Storage");
            options.Elasticsearch = new List<string>();
            var esSource = cs.GetSection(nameof(Elasticsearch)).Get<List<string>>();
            if (esSource.IsNotNull() && esSource.Count > 0)
            {
                esSource.ForEach(x =>
                {
                    options.Elasticsearch.Add(x);
                });
            }

            options.MysqlServers = new List<MysqlServerAddress>();
            var mongoServersSource = cs.GetValue<string>(nameof(MysqlServers));
            if (mongoServersSource.IsNotNullOrEmpty())
            {
                mongoServersSource.Split(",").ForEach(item =>
                {
                    var host = item.Replace("：", ":").Split(':');
                    if (host.Length == 2)
                    {
                        options.MysqlServers.Add(new MysqlServerAddress() { Host = host[0], Port = host[1].ToInt() });
                    }
                });
            }

            options.MysqlConnectionMode = cs.GetValue<string>(nameof(MysqlConnectionMode));
            options.MysqlDbName = cs.GetValue<string>(nameof(MysqlDbName));
            options.MysqlUsername = cs.GetValue<string>(nameof(MysqlUsername));
            options.MysqlPassword = cs.GetValue<string>(nameof(MysqlPassword));

            return options;
        }
    }
}
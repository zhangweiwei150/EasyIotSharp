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

        public IList<ServerAddress> MysqlServers { get; internal set; }

        public string MysqlConnectionMode { get; internal set; }

        public string MysqlDbName { get; internal set; }

        public string MysqlUsername { get; internal set; }

        public string MysqlPassword { get; internal set; }

        public List<ServerAddress> InfluxdbServers { get; internal set; }

        public string InfluxdbConnectionMode { get; internal set; }

        public string InfluxdbName { get; internal set; }

        public string InfluxdbUsername { get; internal set; }

        public string InfluxdbPassword { get; internal set; }

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

            options.MysqlServers = new List<ServerAddress>();
            var mysqlServersSource = cs.GetValue<string>(nameof(MysqlServers));
            if (mysqlServersSource.IsNotNullOrEmpty())
            {
                mysqlServersSource.Split(",").ForEach(item =>
                {
                    var host = item.Replace("：", ":").Split(':');
                    if (host.Length == 2)
                    {
                        options.MysqlServers.Add(new ServerAddress() { Host = host[0], Port = host[1].ToInt() });
                    }
                });
            }

            options.MysqlConnectionMode = cs.GetValue<string>(nameof(MysqlConnectionMode));
            options.MysqlDbName = cs.GetValue<string>(nameof(MysqlDbName));
            options.MysqlUsername = cs.GetValue<string>(nameof(MysqlUsername));
            options.MysqlPassword = cs.GetValue<string>(nameof(MysqlPassword));

            options.InfluxdbServers = new List<ServerAddress>();
            var influxdbServersSource = cs.GetValue<string>(nameof(InfluxdbServers));
            if (influxdbServersSource.IsNotNullOrEmpty())
            {
                influxdbServersSource.Split(",").ForEach(item =>
                {
                    var host = item.Replace("：", ":").Split(':');
                    if (host.Length == 2)
                    {
                        options.MysqlServers.Add(new ServerAddress() { Host = host[0], Port = host[1].ToInt() });
                    }
                });
            }

            options.InfluxdbConnectionMode = cs.GetValue<string>(nameof(InfluxdbConnectionMode));
            options.InfluxdbName = cs.GetValue<string>(nameof(InfluxdbName));
            options.InfluxdbUsername = cs.GetValue<string>(nameof(InfluxdbUsername));
            options.InfluxdbPassword = cs.GetValue<string>(nameof(InfluxdbPassword));

            return options;
        }
    }
}
using EasyIotSharp.Core.Configuration;
using InfluxDB.Client;
using System.Linq;
using UPrime;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public class InfluxdbDatabaseProvider:IInfluxdbDatabaseProvider
    {
        public IInfluxDBClient Client { get; }

        public string Org { get; }

        public InfluxdbDatabaseProvider(string org)
        {
            Org = org;

            var storageOptions = UPrimeEngine.Instance.Resolve<StorageOptions>();

            string connectionString = "";
            string token = $"{storageOptions.InfluxdbUsername}:{storageOptions.InfluxdbPassword}";

            if (storageOptions.InfluxdbConnectionMode.ToLower() == "replicaset" && storageOptions.InfluxdbServers.Count > 1)
            {
                // 集群模式连接
                connectionString = string.Join(",", storageOptions.InfluxdbServers
                    .Select(server => $"{server.Host}:{server.Port}"));

                Client = InfluxDBClientFactory.Create($"http://{connectionString}", token.ToCharArray());
            }
            else
            {
                // 单节点连接
                var server = storageOptions.InfluxdbServers[0];
                connectionString = $"http://{server.Host}:{server.Port}";

                Client = InfluxDBClientFactory.Create(connectionString, token.ToCharArray());
            }

            InitializeDatabase(storageOptions.InfluxdbName);
        }

        private void InitializeDatabase(string bucket)
        {
            // 检查并创建bucket
            var bucketsApi = Client.GetBucketsApi();
            var existingBucket = bucketsApi.FindBucketByNameAsync(bucket).GetAwaiter().GetResult();

            if (existingBucket == null)
            {
                bucketsApi.CreateBucketAsync(bucket, Org).GetAwaiter().GetResult();
            }

            // InfluxDB是无模式的，不需要像关系型数据库那样初始化表结构
            // 但可以在这里添加初始化标签或测量(measurement)的逻辑
        }
    }
}

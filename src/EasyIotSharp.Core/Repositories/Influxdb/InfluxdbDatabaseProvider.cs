using EasyIotSharp.Core.Configuration;
using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using System;
using System.Linq;
using System.Threading.Tasks;
using UPrime;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public class InfluxdbDatabaseProvider : IInfluxdbDatabaseProvider
    {
        public InfluxdbDatabaseProvider()
        {
            var storageOptions = UPrimeEngine.Instance.Resolve<StorageOptions>();

            string connectionString = "";
            string token = $"{storageOptions.InfluxdbUsername}:{storageOptions.InfluxdbPassword}";

            if (storageOptions.InfluxdbConnectionMode.ToLower() == "replicaset" && storageOptions.InfluxdbServers.Count > 1)
            {
                // 集群模式连接
                connectionString = string.Join(",", storageOptions.InfluxdbServers
                    .Select(server => $"{server.Host}:{server.Port}"));

                Client = new InfluxDbClient(connectionString, storageOptions.InfluxdbUsername, storageOptions.InfluxdbPassword, InfluxDbVersion.Latest);
            }
            else
            {
                // 单节点连接
                var server = storageOptions.InfluxdbServers[0];
                connectionString = $"http://{server.Host}:{server.Port}";

                Client = new InfluxDbClient(connectionString, storageOptions.InfluxdbUsername, storageOptions.InfluxdbPassword, InfluxDbVersion.Latest);
            }
        }

        public IInfluxDbClient Client { get; }
    }
}

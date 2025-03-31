using InfluxDB.Client;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public interface IInfluxdbDatabaseProvider
    {
        public IInfluxDBClient Client { get; }

        public string Org { get; }
    }
}

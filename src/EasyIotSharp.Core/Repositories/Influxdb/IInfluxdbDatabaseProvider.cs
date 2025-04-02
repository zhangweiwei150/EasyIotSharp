using InfluxData.Net.InfluxDb;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public interface IInfluxdbDatabaseProvider
    {
        public IInfluxDbClient Client { get; }
    }
}

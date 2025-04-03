using UPrime;
using UPrime.Dependency;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public class InfluxdbRepositoryFactory: ISingletonDependency
    {
        public static IInfluxdbRepositoryBase<TEntity> Create<TEntity>(string measurementName, string tenantDatabase)
        {
            return new InfluxdbRepositoryBase<TEntity>(UPrimeEngine.Instance.Resolve<IInfluxdbDatabaseProvider>(), measurementName, tenantDatabase);
        }
    }
}

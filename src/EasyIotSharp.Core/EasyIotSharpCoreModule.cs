using MongoDB.Bson.Serialization.Conventions;
using System.Reflection;
using UPrime;
using UPrime.AutoMapper;
using UPrime.Dependency;
using UPrime.Elasticsearch;
using UPrime.Modules;
using UPrime.MongoDb;
using UPrime.Runtime.Caching.Redis;
using UPrime.SDK.Weixin.Configuration;
using EasyIotSharp.Core.Caching;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Queue;
using EasyIotSharp.Core.Queue.Impl;
using EasyIotSharp.Repositories.Elasticsearch;
using EasyIotSharp.Repositories.Mysql;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Core.Repositories.Tenant;
using EasyIotSharp.Core.Repositories.Tenant.Impl;
using EasyIotSharp.Core.Repositories.TenantAccount;
using EasyIotSharp.Core.Repositories.TenantAccount.Impl;
using EasyIotSharp.Core.Repositories.Project;
using EasyIotSharp.Core.Repositories.Project.Impl;
using EasyIotSharp.Core.Repositories.Hardware;
using EasyIotSharp.Core.Repositories.Hardware.Impl;
using EasyIotSharp.Core.Repositories.Influxdb;

namespace EasyIotSharp.Core
{
    [DependsOn(
        typeof(UPrimeLeadershipModule),
        typeof(UPrimeRedisCacheModule),
        typeof(UPrimeAutoMapperModule)
       )]
    public class EasyIotSharpCoreModule : UPrimeModule
    {
        public override void PreInitialize()
        {
            var options = IocManager.Resolve<AppOptions>();
            IocManager.Register<IElasticsearchDatabaseProvider, EasyIotSharpElasticsearchDatabaseProvider>();

            IocManager.Register<ISqlSugarDatabaseProvider, SqlSugarDatabaseProvider>();


            var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

            if (options.CachingOptions.RedisTwemProxyEnabled)
            {
                IocManager.Register<IUPrimeRedisCacheDatabaseProvider, CurrentRedisCacheDatabaseProvider_TwemProxy>(DependencyLifeStyle.Singleton);
            }
            else
            {
                IocManager.Register<IUPrimeRedisCacheDatabaseProvider, CurrentRedisCacheDatabaseProvider>(DependencyLifeStyle.Singleton);
            }

            IocManager.Register<IKafkaService, KafkaService>();

            IocManager.Register<ICorpWeixinSettings, CorpWeixinOptions>();

            //仓储
            IocManager.Register<ITenantRepository, TenantRepository>();
            //权限
            IocManager.Register<IMenuRepository, MenuRepository>();
            IocManager.Register<IRoleMenuRepository, RoleMenuRepository>();
            IocManager.Register<IRoleRepository, RoleRepository>();
            IocManager.Register<ISoldierRepository, SoldierRepository>();
            IocManager.Register<ISoldierRoleRepository, SoldierRoleRepository>();
            //项目
            IocManager.Register<IProjectBaseRepository, ProjectBaseRepository>();
            IocManager.Register<IClassificationRepository, ClassificationRepository>();
            IocManager.Register<IGatewayProtocolRepository, GatewayProtocolRepository>();
            IocManager.Register<IGatewayProtocolConfigRepository, GatewayProtocolConfigRepository>();
            IocManager.Register<IGatewayRepository, GatewayRepository>();
            IocManager.Register<ISensorPointRepository, SensorPointRepository>();
            //硬件
            IocManager.Register<IProtocolRepository, ProtocolRepository>();
            IocManager.Register<IProtocolConfigRepository, ProtocolConfigRepository>();
            IocManager.Register<IProtocolConfigExtRepository, ProtocolConfigExtRepository>();
            IocManager.Register<ISensorRepository, SensorRepository>();
            IocManager.Register<ISensorQuotaRepository, SensorQuotaRepository>();

        }

        /// <summary>
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
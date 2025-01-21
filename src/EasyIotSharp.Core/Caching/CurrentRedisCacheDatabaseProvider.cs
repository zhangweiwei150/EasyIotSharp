using StackExchange.Redis;
using System;
using System.Net;
using UPrime;
using UPrime.Dependency;
using UPrime.Runtime.Caching.Redis;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Core.Caching
{
    /// <summary>
    /// 自定义的配置
    /// </summary>
    public class CurrentRedisCacheDatabaseProvider : IUPrimeRedisCacheDatabaseProvider, ISingletonDependency
    {
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly CachingOptions _cachingOptions;

        public CurrentRedisCacheDatabaseProvider()
        {
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
            _cachingOptions = UPrimeEngine.Instance.Resolve<CachingOptions>();
        }

        //public bool IsOpenedCQRS => GlobalConfigs.Caching.RedisIsOpenedCQRS;

        /// <summary>
        ///获取数据库连接
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_cachingOptions.RedisDatabaseId);
        }

        public IServer GetServer(EndPoint endPoint)
        {
            return _connectionMultiplexer.Value.GetServer(endPoint);
        }

        public EndPoint[] GetEndPoints()
        {
            return _connectionMultiplexer.Value.GetEndPoints();
        }

        public IServer GetMasterServer()
        {
            EndPoint[] endpoints = _connectionMultiplexer.Value.GetEndPoints();
            IServer result = null;
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.Value.GetServer(endpoint);
                if (server.IsSlave || !server.IsConnected) continue;
                if (result != null) throw new InvalidOperationException("Requires exactly one master endpoint (found " + server.EndPoint + " and " + result.EndPoint + ")");
                result = server;
            }
            if (result == null) throw new InvalidOperationException("Requires exactly one master endpoint (found none)");
            return result;
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_cachingOptions.RedisConnStr);
        }
    }
}
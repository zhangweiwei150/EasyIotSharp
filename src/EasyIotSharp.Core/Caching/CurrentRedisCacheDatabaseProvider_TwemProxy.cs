using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using UPrime;
using UPrime.Dependency;
using UPrime.Runtime.Caching.Redis;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Core.Caching
{
    /// <summary>
    /// 自定义的配置(分片)
    /// </summary>
    public class CurrentRedisCacheDatabaseProvider_TwemProxy : IUPrimeRedisCacheDatabaseProvider, ISingletonDependency
    {
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly CachingOptions _cachingOptions;

        public CurrentRedisCacheDatabaseProvider_TwemProxy()
        {
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
            _cachingOptions = UPrimeEngine.Instance.Resolve<CachingOptions>();
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

        public IServer GetServer(EndPoint endPoint)
        {
            return _connectionMultiplexer.Value.GetServer(endPoint);
        }

        public EndPoint[] GetEndPoints()
        {
            return _connectionMultiplexer.Value.GetEndPoints();
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_cachingOptions.RedisDatabaseId);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            //twemproxy: https://github.com/twitter/twemproxy/blob/master/notes/redis.md

            var options = new ConfigurationOptions
            {
                EndPoints = { _cachingOptions.RedisConnStr },
                AbortOnConnectFail = false
            };

            options.Proxy = Proxy.Twemproxy;
            options.CommandMap = CommandMap.Create(new HashSet<string> {
                "ping",
                "get", "set", "del", "incr", "incrby", "scan","setex","psetex"
            }, true);
            return ConnectionMultiplexer.Connect(options);
        }
    }
}
using Elasticsearch.Net;
using Nest;
using System;
using System.Linq;
using UPrime.Elasticsearch;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Repositories.Elasticsearch
{
    public class EasyIotSharpElasticsearchDatabaseProvider : IElasticsearchDatabaseProvider
    {
        private readonly StorageOptions _options;

        public EasyIotSharpElasticsearchDatabaseProvider(StorageOptions options)
        {
            _options = options;
        }

        public ElasticClient GetClient()
        {
            ElasticClient _client = null;

            if (_options.Elasticsearch.Count == 0)
                throw new Exception("Elasticsearch 配置有误");

            if (_options.Elasticsearch.Count > 1)
            {
                var nodes = _options.Elasticsearch.Select(x => new Uri(x)).ToArray();
                var pool = new SniffingConnectionPool(nodes);

                _client = new ElasticClient(new ConnectionSettings(pool));
            }
            else
            {
                var node = _options.Elasticsearch.Select(x => new Uri(x)).FirstOrDefault();

                var settings = new ConnectionSettings(node);
                _client = new ElasticClient(settings);
            }
            return _client;
        }
    }
}
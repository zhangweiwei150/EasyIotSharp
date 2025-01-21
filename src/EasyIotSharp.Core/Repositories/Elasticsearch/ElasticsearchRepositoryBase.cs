using UPrime.Elasticsearch;

namespace EasyIotSharp.Repositories.Elasticsearch
{
    /// <summary>
    ///  nest: https://github.com/elastic/elasticsearch-net
    /// </summary>
    public abstract class ElasticsearchRepositoryBase : UPrime.Elasticsearch.ElasticsearchRepositoryBase
    {
        public ElasticsearchRepositoryBase(IElasticsearchDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}
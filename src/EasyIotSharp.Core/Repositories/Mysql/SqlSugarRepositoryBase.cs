using System;
using SqlSugar;
using EasyIotSharp.Core.Domain;
using UPrime.Domain.Entities;
using EasyIotSharp.Core.Repositories.Mysql;
using System.Reflection;

namespace EasyIotSharp.Repositories.Mysql
{
    public abstract class SqlSugarRepositoryBase
    {
        private readonly ISqlSugarDatabaseProvider _dbProvider;

        public SqlSugarRepositoryBase(ISqlSugarDatabaseProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public ISqlSugarClient Client => _dbProvider.Client;
    }

    public abstract class SqlSugerRepositoryBase<TEntity> : MysqlRepositoryBase<TEntity, int> where TEntity : class, IEntity<int>, new()
    {
        public SqlSugerRepositoryBase(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }

    public abstract class MysqlRepositoryBase<TEntity, TPrimaryKey> : MySqlRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        public MysqlRepositoryBase(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }
    }
}
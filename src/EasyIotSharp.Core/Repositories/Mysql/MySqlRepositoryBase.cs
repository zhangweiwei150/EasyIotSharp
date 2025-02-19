using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UPrime.Domain.Entities;

namespace EasyIotSharp.Core.Repositories.Mysql
{
    public abstract class MySqlRepositoryBase<TEntity, TPrimaryKey> : IMySqlRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        private ISqlSugarDatabaseProvider _databaseProvider;

        public MySqlRepositoryBase(ISqlSugarDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        protected ISqlSugarClient GetDbClient()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _databaseProvider.Client.CurrentConnectionConfig.ConnectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            using var db = GetDbClient();
            return await db.Insertable(entity).ExecuteCommandAsync();
        }

        public virtual async Task<int> InserManyAsync(List<TEntity> entities)
        {
            using var db = GetDbClient();
            return await db.Insertable(entities).ExecuteCommandAsync();
        }

        public virtual async Task<bool> DeleteByIdAsync(object id)
        {
            using var db = GetDbClient();
            return await db.Deleteable<TEntity>().In(id).ExecuteCommandAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            using var db = GetDbClient();
            return await db.Deleteable(entity).ExecuteCommandAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            using var db = GetDbClient();
            var result = await db.Updateable(entity).ExecuteCommandAsync();
            return result > 0;
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().InSingleAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().FirstAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().CountAsync(predicate);
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            using var db = GetDbClient();
            return await db.Queryable<TEntity>()
                .Where(predicate)
                .ToPageListAsync(pageIndex, pageSize);
        }

        public void Dispose()
        {
            // 不再直接调用 Client.Dispose，因为每个方法都使用独立的 Client 实例
        }
    }
}
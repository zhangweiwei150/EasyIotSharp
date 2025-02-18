using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// 确保连接已打开
        /// </summary>
        public void EnsureConnectionOpen()
        {
            if (!_databaseProvider.Client.CurrentConnectionConfig.IsAutoCloseConnection)
            {
                if (_databaseProvider.Client.Ado.Connection.State != ConnectionState.Open)
                {
                    _databaseProvider.Client.Ado.Connection.Open();
                }
            }
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Insertable(entity).ExecuteCommandAsync();
        }

        public virtual async Task<int> InserManyAsync(List<TEntity> entities)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Insertable(entities).ExecuteCommandAsync();
        }

        public virtual async Task<bool> DeleteByIdAsync(object id)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Deleteable<TEntity>().In(id).ExecuteCommandAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Deleteable(entity).ExecuteCommandAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            EnsureConnectionOpen();
            var result = await _databaseProvider.Client.Updateable(entity).ExecuteCommandAsync();
            return result > 0;
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().InSingleAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().FirstAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().CountAsync(predicate);
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            EnsureConnectionOpen();
            return await _databaseProvider.Client.Queryable<TEntity>()
                .Where(predicate)
                .ToPageListAsync(pageIndex, pageSize);
        }

        public void Dispose()
        {
            // 不再直接调用 Client.Dispose，因为每个方法都使用独立的 Client 实例
        }
    }
}
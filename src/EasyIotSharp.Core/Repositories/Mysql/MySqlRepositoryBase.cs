using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UPrime.Domain.Entities;

namespace EasyIotSharp.Core.Repositories.Mysql
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class MySqlRepositoryBase<TEntity, TPrimaryKey>: IMySqlRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        private ISqlSugarDatabaseProvider _databaseProvider;
        public ISqlSugarClient Client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public MySqlRepositoryBase(ISqlSugarDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            Client = _databaseProvider.Client;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            return await _databaseProvider.Client.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> InserManyAsync(List<TEntity> entities)
        {
            return await _databaseProvider.Client.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteByIdAsync(object id)
        {
            return await _databaseProvider.Client.Deleteable<TEntity>().In(id).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            return await _databaseProvider.Client.Deleteable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var result = await _databaseProvider.Client.Updateable(entity).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _databaseProvider.Client.Queryable<TEntity>().InSingleAsync(id);
        }

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _databaseProvider.Client.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 根据条件查询第一个实体，如果没有则返回默认值
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _databaseProvider.Client.Queryable<TEntity>().FirstAsync(predicate);
        }

        /// <summary>
        /// 根据条件查询是否存在符合条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _databaseProvider.Client.Queryable<TEntity>().AnyAsync(predicate);
        }

        /// <summary>
        /// 获取实体的总数
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> CountAsync()
        {
            return await _databaseProvider.Client.Queryable<TEntity>().CountAsync();
        }

        /// <summary>
        /// 根据条件获取实体的总数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _databaseProvider.Client.Queryable<TEntity>().CountAsync(predicate);
        }

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _databaseProvider.Client.Queryable<TEntity>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 分页查询实体列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            return await _databaseProvider.Client.Queryable<TEntity>()
                .Where(predicate)
                .ToPageListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _databaseProvider.Client.Dispose();
        }
    }
}

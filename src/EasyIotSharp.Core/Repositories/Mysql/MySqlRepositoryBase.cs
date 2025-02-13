using SqlSugar;
using System.Collections.Generic;
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
        /// 
        /// </summary>
        public void Dispose()
        {
            _databaseProvider.Client.Dispose();
        }
    }
}

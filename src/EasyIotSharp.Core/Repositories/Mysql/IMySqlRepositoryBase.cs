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
    public interface IMySqlRepositoryBase<TEntity, in TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InserManyAsync(List<TEntity> entities);

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(object id);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync();
    }
}

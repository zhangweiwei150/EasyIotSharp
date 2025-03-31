using InfluxDB.Client.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UPrime.Domain.Entities;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public interface IInfluxdbRepositoryBase<TEntity, in TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 写入单个数据点
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="precision">时间精度</param>
        /// <returns>是否写入成功</returns>
        Task<bool> WriteAsync(TEntity entity, WritePrecision precision = WritePrecision.Ns);

        /// <summary>
        /// 批量写入数据点
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="precision">时间精度</param>
        /// <returns>是否写入成功</returns>
        Task<bool> WriteManyAsync(IEnumerable<TEntity> entities, WritePrecision precision = WritePrecision.Ns);

        /// <summary>
        /// 根据时间范围查询数据
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="stop">结束时间</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryByTimeRangeAsync(DateTime start, DateTime stop);

        /// <summary>
        /// 根据Flux查询数据
        /// </summary>
        /// <param name="fluxQuery">Flux查询语句</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryByFluxAsync(string fluxQuery);

        /// <summary>
        /// 根据条件查询第一个数据点
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="timeRange">时间范围</param>
        /// <returns>数据点或null</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null);

        /// <summary>
        /// 根据条件查询数据点列表
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="timeRange">时间范围</param>
        /// <returns>数据点列表</returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null);

        /// <summary>
        /// 分页查询数据点
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="timeRange">时间范围</param>
        /// <returns>(总数量, 数据点列表)</returns>
        Task<(long TotalCount, List<TEntity> Items)> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate,
            int pageIndex,
            int pageSize,
            (DateTime Start, DateTime End)? timeRange = null);

        /// <summary>
        /// 根据条件判断是否存在数据点
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="timeRange">时间范围</param>
        /// <returns>是否存在</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null);

        /// <summary>
        /// 获取数据点总数
        /// </summary>
        /// <param name="timeRange">时间范围</param>
        /// <returns>总数</returns>
        Task<long> CountAsync((DateTime Start, DateTime End)? timeRange = null);

        /// <summary>
        /// 根据条件获取数据点数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="timeRange">时间范围</param>
        /// <returns>数量</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null);

        /// <summary>
        /// 删除满足条件的数据点
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="timeRange">时间范围</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, (DateTime Start, DateTime End)? timeRange = null);
    }
}

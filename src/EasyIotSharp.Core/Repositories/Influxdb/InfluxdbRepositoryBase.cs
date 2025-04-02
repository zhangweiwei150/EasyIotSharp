using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using UPrime.Domain.Entities;
using InfluxData.Net.InfluxDb.Models.Responses;
using UPrime.Domain.Entities.Auditing;
using UPrime.Domain.Repositories;

namespace EasyIotSharp.Core.Repositories.Influxdb
{
    public abstract class InfluxDbRepositoryBase<TEntity, TPrimaryKey> : IInfluxdbRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        private readonly IInfluxdbDatabaseProvider _databaseProvider;

        /// <summary>
        /// InfluxDB 客户端
        /// </summary>
        public virtual IInfluxDbClient Client => _databaseProvider.Client;

        /// <summary>
        /// 测量名称(相当于表名)
        /// </summary>
        public abstract string MeasurementName { get; }

        /// <summary>
        /// 租户数据库名称
        /// </summary>
        public abstract string TenantDatabase { get; }

        /// <summary>
        /// 默认数据库名称
        /// </summary>
        public virtual string DefaultDatabase => "default";

        public InfluxDbRepositoryBase(IInfluxdbDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;

            InitializeDatabase(_databaseProvider).Wait();
        }

        public async Task InitializeDatabase(IInfluxdbDatabaseProvider provider)
        {
            var requiredDatabases = new[] { DefaultDatabase, TenantDatabase };
            var dataBases = await provider.Client.Database.GetDatabasesAsync();
            foreach (var dbName in requiredDatabases)
            {
                var exists = dataBases.FirstOrDefault(s => s.Name == dbName);
                if (exists.IsNull())
                {
                    await provider.Client.Database.CreateDatabaseAsync(dbName);
                }
            }
        }

        #region 查询操作

        /// <summary>
        /// 获取所有数据
        /// </summary>
        public virtual IQueryable<TEntity> GetAll()
        {
            var query = $"SELECT * FROM {MeasurementName}";
            var result = _databaseProvider.Client.Client.QueryAsync(query, DefaultDatabase).Result;
            return ConvertSeriesToEntities(result.FirstOrDefault()).AsQueryable();
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public virtual TEntity Get(TPrimaryKey id)
        {
            var query = $"SELECT * FROM {MeasurementName} WHERE id='{id}'";
            var result = _databaseProvider.Client.Client.QueryAsync(query, DefaultDatabase).Result;
            var entity = ConvertSeriesToEntities(result.FirstOrDefault()).FirstOrDefault();

            if (entity == null)
            {
                throw new EntityNotFoundException($"There is no such an entity with given primary key. Entity type: {typeof(TEntity).FullName}, primary key: {id}");
            }

            return entity;
        }

        /// <summary>
        /// 根据ID异步获取实体
        /// </summary>
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var query = $"SELECT * FROM {MeasurementName} WHERE id='{id}'";
            var result = await _databaseProvider.Client.Client.QueryAsync(query, DefaultDatabase);
            var entity = ConvertSeriesToEntities(result.FirstOrDefault()).FirstOrDefault();

            if (entity == null)
            {
                throw new EntityNotFoundException($"There is no such an entity with given primary key. Entity type: {typeof(TEntity).FullName}, primary key: {id}");
            }

            return entity;
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        public virtual async Task<List<TEntity>> QueryAsync(string whereClause)
        {
            var query = $"SELECT * FROM {MeasurementName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                query += $" WHERE {whereClause}";
            }

            var result = await _databaseProvider.Client.Client.QueryAsync(query, DefaultDatabase);
            return ConvertSeriesToEntities(result.FirstOrDefault()).ToList();
        }

        #endregion

        #region 写入操作

        /// <summary>
        /// 插入实体
        /// </summary>
        public virtual TEntity Insert(TEntity entity)
        {
            SetCreationAuditProperties(entity);
            CheckAndSetDefaultValue(entity);

            var point = ConvertEntityToPoint(entity);
            _databaseProvider.Client.Client.WriteAsync(point, DefaultDatabase).Wait();

            return entity;
        }

        /// <summary>
        /// 异步插入实体
        /// </summary>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            SetCreationAuditProperties(entity);
            CheckAndSetDefaultValue(entity);

            var point = ConvertEntityToPoint(entity);
            await _databaseProvider.Client.Client.WriteAsync(point, DefaultDatabase);

            return entity;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            var points = entities.Select(entity =>
            {
                SetCreationAuditProperties(entity);
                CheckAndSetDefaultValue(entity);
                return ConvertEntityToPoint(entity);
            });

            await _databaseProvider.Client.Client.WriteAsync(points, DefaultDatabase);
        }

        #endregion

        #region 更新操作

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual TEntity Update(TEntity entity)
        {
            var point = ConvertEntityToPoint(entity);
            _databaseProvider.Client.Client.WriteAsync(point, DefaultDatabase).Wait();
            return entity;
        }

        /// <summary>
        /// 异步更新实体
        /// </summary>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var point = ConvertEntityToPoint(entity);
            await _databaseProvider.Client.Client.WriteAsync(point, DefaultDatabase);
            return entity;
        }

        #endregion

        #region 删除操作

        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                Update(entity);
            }
            else
            {
                DeleteById(entity.Id);
            }
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        public virtual void Delete(TPrimaryKey id)
        {
            DeleteById(id);
        }

        /// <summary>
        /// 异步删除实体
        /// </summary>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                await UpdateAsync(entity);
            }
            else
            {
                await DeleteByIdAsync(entity.Id);
            }
        }

        /// <summary>
        /// 根据ID异步删除
        /// </summary>
        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            await DeleteByIdAsync(id);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 将实体转换为Point
        /// </summary>
        protected virtual Point ConvertEntityToPoint(TEntity entity)
        {
            var point = new Point
            {
                Name = MeasurementName,
                Timestamp = GetEntityTimestamp(entity),
                Tags = new Dictionary<string, object>(),
                Fields = new Dictionary<string, object>()
            };

            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                if (value == null) continue;

                // ID作为Tag
                if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    point.Tags["id"] = value.ToString();
                    continue;
                }

                // 审计字段处理
                if (prop.Name.Equals("CreationTime", StringComparison.OrdinalIgnoreCase))
                    continue;

                // 根据类型决定存储方式
                if (ShouldStoreAsTag(prop))
                {
                    point.Tags[prop.Name] = value.ToString();
                }
                else
                {
                    point.Fields[prop.Name] = value;
                }
            }

            return point;
        }

        /// <summary>
        /// 判断属性是否应存储为Tag
        /// </summary>
        protected virtual bool ShouldStoreAsTag(PropertyInfo property)
        {
            var type = property.PropertyType;
            return type == typeof(string) || type.IsEnum || type == typeof(bool);
        }

        /// <summary>
        /// 将查询结果转换为实体列表
        /// </summary>
        protected virtual IEnumerable<TEntity> ConvertSeriesToEntities(Serie serie)
        {
            if (serie == null || serie.Values == null)
                return Enumerable.Empty<TEntity>();

            var entities = new List<TEntity>();
            var columns = serie.Columns.ToList();

            foreach (var valueArray in serie.Values)
            {
                var entity = new TEntity();

                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    var value = valueArray[i];

                    var prop = typeof(TEntity).GetProperty(column);
                    if (prop != null && value != null && prop.CanWrite)
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                            prop.SetValue(entity, convertedValue);
                        }
                        catch
                        {
                            // 忽略转换错误
                        }
                    }
                }

                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// 获取实体时间戳
        /// </summary>
        protected virtual DateTime GetEntityTimestamp(TEntity entity)
        {
            if (entity is IHasCreationTime hasCreationTime)
                return hasCreationTime.CreationTime;

            return DateTime.UtcNow;
        }

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        protected virtual void DeleteById(TPrimaryKey id)
        {
            var query = $"DROP SERIES FROM {MeasurementName} WHERE id='{id}'";
            _databaseProvider.Client.Client.QueryAsync(query, DefaultDatabase).Wait();
        }

        /// <summary>
        /// 异步根据ID删除数据
        /// </summary>
        protected virtual async Task DeleteByIdAsync(TPrimaryKey id)
        {
            var query = $"DROP SERIES FROM {MeasurementName} WHERE id='{id}'";
            await _databaseProvider.Client.Client.QueryAsync(query, DefaultDatabase);
        }

        #endregion

        #region 继承自基类的方法

        protected virtual void SetCreationAuditProperties(object entityAsObj)
        {
            if (entityAsObj is IHasCreationTime hasCreationTime)
            {
                if (hasCreationTime.CreationTime == default(DateTime))
                {
                    hasCreationTime.CreationTime = DateTime.UtcNow;
                }
            }
        }

        protected virtual void CheckAndSetDefaultValue(object entityAsObj)
        {
            var properties = entityAsObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(string) && prop.GetValue(entityAsObj) == null)
                {
                    prop.SetValue(entityAsObj, string.Empty);
                }
            }
        }

        #endregion
    }
}